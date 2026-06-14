using System.Collections.ObjectModel;
using System.ComponentModel;
using RB_Skin_Forge.Core.Abstractions;
using RB_Skin_Forge.Core.Localization;
using RB_Skin_Forge.Core.Models;

namespace RB_Skin_Forge.Maui;

public partial class MainPage : ContentPage
{
	private readonly IAssetPipeline _pipeline;
	private readonly Localizer _loc;

	private readonly BodyPart[] _bodyParts = Enum.GetValues<BodyPart>();
	private readonly ObservableCollection<BatchItem> _items = new();
	private BatchItem? _selected;
	private string _theme = "auto";

	public MainPage(IAssetPipeline pipeline, Localizer loc)
	{
		InitializeComponent();
		_pipeline = pipeline;
		_loc = loc;

		FileList.ItemsSource = _items;

		LanguagePicker.ItemsSource = Languages.All.Select(l => l.NativeName).ToList();
		LanguagePicker.SelectedIndex = Languages.IndexOf(_loc.CurrentCode);

		// Restore the saved theme.
		_theme = Preferences.Get("rb-theme", "auto");
		ApplyTheme(_theme);

		RebuildBodyParts(BodyPart.Torso);
		ApplyLanguage();
	}

	// --- Language ----------------------------------------------------------

	private void OnLanguageChanged(object? sender, EventArgs e)
	{
		if (LanguagePicker.SelectedIndex < 0) return;
		_loc.SetLanguage(Languages.All[LanguagePicker.SelectedIndex].Code);
		ApplyLanguage();
	}

	private void ApplyLanguage()
	{
		TaglineLabel.Text = _loc.T("app.tagline");
		DropZoneHeading.Text = _loc.T("panel.dropzone");
		BodyPartHeading.Text = _loc.T("field.bodypart");
		PickBtn.Text = _loc.T("btn.choose");
		RunBtn.Text = _loc.T("btn.generate");
		PreviewHeading.Text = _loc.T("panel.preview");
		ConsoleHeading.Text = _loc.T("panel.console");
		SaveTemplateBtn.Text = _loc.T("btn.saveoutput");
		SaveLogBtn.Text = _loc.T("btn.savelog");
		ThemeAutoBtn.Text = _loc.T("theme.auto");
		ThemeLightBtn.Text = _loc.T("theme.light");
		ThemeDarkBtn.Text = _loc.T("theme.dark");
		LanguageHeading.Text = _loc.T("nav.language");
		ThemeHeading.Text = _loc.T("nav.theme");

		if (_selected is null)
			FileLabel.Text = _loc.T("file.none");

		// Re-localize body-part names (keep the current selection).
		var current = BodyPartPicker.SelectedIndex >= 0 ? _bodyParts[BodyPartPicker.SelectedIndex] : BodyPart.Torso;
		RebuildBodyParts(current);

		// Re-localize the per-file state words and the selected item's panels.
		foreach (var item in _items) item.Refresh(_loc);
		RefreshSelectedPanels();
		RefreshBatchSummary();
	}

	private void RebuildBodyParts(BodyPart keep)
	{
		BodyPartPicker.ItemsSource = _bodyParts.Select(p => _loc.BodyPartName(p)).ToList();
		BodyPartPicker.SelectedIndex = Array.IndexOf(_bodyParts, keep);
		if (BodyPartPicker.SelectedIndex < 0) BodyPartPicker.SelectedIndex = Array.IndexOf(_bodyParts, BodyPart.Torso);
	}

	// --- Theme -------------------------------------------------------------

	private void OnThemeAuto(object? sender, EventArgs e) => SetTheme("auto");
	private void OnThemeLight(object? sender, EventArgs e) => SetTheme("light");
	private void OnThemeDark(object? sender, EventArgs e) => SetTheme("dark");

	private void SetTheme(string mode)
	{
		_theme = mode;
		Preferences.Set("rb-theme", mode);
		ApplyTheme(mode);
	}

	private void ApplyTheme(string mode)
	{
		if (Application.Current is { } app)
		{
			app.UserAppTheme = mode switch
			{
				"light" => AppTheme.Light,
				"dark" => AppTheme.Dark,
				_ => AppTheme.Unspecified
			};
		}

		// Highlight the active segment (filled accent vs. transparent).
		StyleSeg(ThemeAutoBtn, mode == "auto");
		StyleSeg(ThemeLightBtn, mode == "light");
		StyleSeg(ThemeDarkBtn, mode == "dark");
	}

	private void StyleSeg(Button b, bool active)
	{
		if (active)
		{
			b.BackgroundColor = GetColor("RbAccent", Color.FromArgb("#2F6DF6"));
			b.TextColor = Colors.White;
			b.FontAttributes = FontAttributes.Bold;
		}
		else
		{
			b.BackgroundColor = Colors.Transparent;
			b.TextColor = Colors.Gray;
			b.FontAttributes = FontAttributes.None;
		}
	}

	private static Color GetColor(string key, Color fallback) =>
		Application.Current?.Resources.TryGetValue(key, out var v) == true && v is Color c ? c : fallback;

	// --- File picking (batch) ---------------------------------------------

	private static readonly FilePickerFileType AssetFileTypes = new(
		new Dictionary<DevicePlatform, IEnumerable<string>>
		{
			{ DevicePlatform.WinUI, new[] { ".png", ".jpg", ".jpeg", ".bmp", ".tga", ".obj", ".fbx" } },
			{ DevicePlatform.macOS, new[] { "png", "jpg", "jpeg", "bmp", "tga", "obj", "fbx" } },
			{ DevicePlatform.Android, new[] { "image/*", "application/octet-stream" } },
			{ DevicePlatform.iOS, new[] { "public.image", "public.item" } },
		});

	private async void OnPickClicked(object? sender, EventArgs e)
	{
		try
		{
			var files = await FilePicker.Default.PickMultipleAsync(new PickOptions
			{
				PickerTitle = _loc.T("dropzone.hint"),
				FileTypes = AssetFileTypes
			});

			if (files is null) return;

			foreach (var file in files)
			{
				using var stream = await file.OpenReadAsync();
				using var ms = new MemoryStream();
				await stream.CopyToAsync(ms);
				var item = new BatchItem(file.FileName, ms.ToArray());
				item.Refresh(_loc);
				_items.Add(item);
				_selected ??= item;
			}

			if (_items.Count > 0)
			{
				FileLabel.Text = $"{_items.Count} file(s) loaded";
				RunBtn.IsEnabled = true;
			}
		}
		catch (Exception ex)
		{
			await DisplayAlert("Error", $"Could not load file(s): {ex.Message}", "OK");
		}
	}

	private void OnFileSelected(object? sender, SelectionChangedEventArgs e)
	{
		_selected = e.CurrentSelection.FirstOrDefault() as BatchItem ?? _selected;
		RefreshSelectedPanels();
	}

	// --- Run (batch) -------------------------------------------------------

	private async void OnRunClicked(object? sender, EventArgs e)
	{
		if (_items.Count == 0) return;

		RunBtn.IsEnabled = false;
		RunBtn.Text = _loc.T("btn.processing");

		int success = 0;
		var part = BodyPartPicker.SelectedIndex >= 0 ? _bodyParts[BodyPartPicker.SelectedIndex] : BodyPart.Torso;

		foreach (var item in _items)
		{
			var input = new AssetInput { FileName = item.Name, Data = item.Data, BodyPart = part };
			item.Result = await _pipeline.ProcessAsync(input);
			item.Refresh(_loc);
			if (item.Result.State == TaskState.Synchronized) success++;

			_selected = item;
			RefreshSelectedPanels();
		}

		RefreshBatchSummary(success);
		RunBtn.IsEnabled = true;
		RunBtn.Text = _loc.T("btn.generate");
	}

	private void RefreshSelectedPanels()
	{
		var result = _selected?.Result;
		if (result is null)
		{
			StateLabel.Text = "";
			StatsLabel.Text = "";
			ConsoleLabel.Text = "";
			PreviewImage.Source = null;
			SaveTemplateBtn.IsEnabled = false;
			SaveLogBtn.IsEnabled = false;
			return;
		}

		StateLabel.Text = _loc.StateName(result.State);
		StateLabel.TextColor = result.State == TaskState.Synchronized ? Colors.LimeGreen
			: result.State == TaskState.Failed ? Colors.OrangeRed : Colors.Goldenrod;

		StatsLabel.Text = BuildStats(result.Report);
		ConsoleLabel.Text = string.Join('\n', result.Log);

		if (result.PreviewImage is not null)
		{
			var bytes = result.PreviewImage;
			PreviewImage.Source = ImageSource.FromStream(() => new MemoryStream(bytes));
		}
		else
		{
			PreviewImage.Source = null;
		}

		SaveTemplateBtn.IsEnabled = result.Output is not null;
		SaveLogBtn.IsEnabled = result.Log.Count > 0;
	}

	private void RefreshBatchSummary(int? success = null)
	{
		int done = _items.Count(i => i.Result is not null);
		if (_items.Count <= 1 || done == 0)
		{
			BatchSummaryLabel.IsVisible = false;
			return;
		}
		int ok = success ?? _items.Count(i => i.Result?.State == TaskState.Synchronized);
		BatchSummaryLabel.Text = string.Format(_loc.T("batch.summary"), ok, _items.Count);
		BatchSummaryLabel.IsVisible = true;
	}

	// --- Save --------------------------------------------------------------

	private async void OnSaveTemplateClicked(object? sender, EventArgs e)
	{
		var result = _selected?.Result;
		if (result?.Output is null) return;
		var name = result.OutputFileName ?? $"RB_Skin_Forge_output_{DateTime.Now:yyyyMMdd_HHmmss}";
		var path = Path.Combine(FileSystem.Current.AppDataDirectory, name);
		await File.WriteAllBytesAsync(path, result.Output);
		await DisplayAlert("Saved", $"Output saved to:\n{path}", "OK");
	}

	private async void OnSaveLogClicked(object? sender, EventArgs e)
	{
		var result = _selected?.Result;
		if (result is null) return;
		var path = Path.Combine(FileSystem.Current.AppDataDirectory,
			$"RB_Skin_Forge_log_{DateTime.Now:yyyyMMdd_HHmmss}.txt");
		await File.WriteAllTextAsync(path, string.Join(Environment.NewLine, result.Log));
		await DisplayAlert("Saved", $"Log saved to:\n{path}", "OK");
	}

	private string BuildStats(QualityReport r)
	{
		var parts = new List<string>();
		if (r.OutputWidth > 0) parts.Add($"{_loc.T("stats.dimensions")}: {r.OutputWidth} × {r.OutputHeight} px");
		if (r.TriangleCount > 0)
		{
			var tri = $"{_loc.T("stats.triangles")}: {r.TriangleCount:N0}";
			if (r.WasDecimated) tri += $" ({_loc.T("stats.decimated")})";
			parts.Add(tri);
		}
		if (r.VertexCount > 0) parts.Add($"{_loc.T("stats.vertices")}: {r.VertexCount:N0}");
		if (r.MaterialCount > 0) parts.Add($"{_loc.T("stats.materials")}: {r.MaterialCount}");
		if (r.AttachmentCount > 0) parts.Add($"{_loc.T("stats.attachments")}: {r.AttachmentCount}");
		if (r.BoundsX > 0 || r.BoundsY > 0 || r.BoundsZ > 0)
			parts.Add($"{_loc.T("stats.bounds")}: {r.BoundsX:0.##} × {r.BoundsY:0.##} × {r.BoundsZ:0.##}");
		parts.Add($"{_loc.T("stats.outputsize")}: {FormatBytes(r.TextureSizeBytes)}");
		return string.Join("   |   ", parts);
	}

	private static string FormatBytes(long bytes) =>
		bytes < 1024 ? $"{bytes} B" :
		bytes < 1024 * 1024 ? $"{bytes / 1024.0:0.0} KB" :
		$"{bytes / (1024.0 * 1024.0):0.0} MB";

	/// <summary>One file in the batch queue. Raises change notifications so the list updates.</summary>
	private sealed class BatchItem : INotifyPropertyChanged
	{
		public string Name { get; }
		public byte[] Data { get; }
		public ProcessingResult? Result { get; set; }

		private string _stateText = "";
		public string StateText
		{
			get => _stateText;
			private set { _stateText = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StateText))); }
		}

		public BatchItem(string name, byte[] data)
		{
			Name = name;
			Data = data;
		}

		public void Refresh(Localizer loc) =>
			StateText = Result is null ? "" : loc.StateName(Result.State);

		public event PropertyChangedEventHandler? PropertyChanged;
	}
}
