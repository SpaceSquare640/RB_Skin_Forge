using RB_Skin_Forge.Core.Abstractions;
using RB_Skin_Forge.Core.Models;

namespace RB_Skin_Forge.Maui;

public partial class MainPage : ContentPage
{
	private readonly IAssetPipeline _pipeline;

	private byte[]? _inputData;
	private string? _inputName;
	private ProcessingResult? _result;

	public MainPage(IAssetPipeline pipeline)
	{
		InitializeComponent();
		_pipeline = pipeline;

		BodyPartPicker.ItemsSource = Enum.GetValues<BodyPart>().ToList();
		BodyPartPicker.SelectedItem = BodyPart.Torso;
	}

	private static readonly FilePickerFileType AssetFileTypes = new(
		new Dictionary<DevicePlatform, IEnumerable<string>>
		{
			{ DevicePlatform.WinUI, new[] { ".png", ".jpg", ".jpeg", ".bmp", ".tga", ".obj" } },
			{ DevicePlatform.macOS, new[] { "png", "jpg", "jpeg", "bmp", "tga", "obj" } },
			{ DevicePlatform.Android, new[] { "image/*", "application/octet-stream" } },
			{ DevicePlatform.iOS, new[] { "public.image", "public.item" } },
		});

	private async void OnPickClicked(object? sender, EventArgs e)
	{
		try
		{
			var file = await FilePicker.Default.PickAsync(new PickOptions
			{
				PickerTitle = "Select an image or OBJ mesh",
				FileTypes = AssetFileTypes
			});

			if (file is null) return;

			using var stream = await file.OpenReadAsync();
			using var ms = new MemoryStream();
			await stream.CopyToAsync(ms);
			_inputData = ms.ToArray();
			_inputName = file.FileName;

			FileLabel.Text = $"{_inputName} ({_inputData.Length:N0} bytes)";
			RunBtn.IsEnabled = true;
			PreviewImage.Source = null;
			SaveTemplateBtn.IsEnabled = false;
		}
		catch (Exception ex)
		{
			await DisplayAlert("Error", $"Could not load file: {ex.Message}", "OK");
		}
	}

	private async void OnRunClicked(object? sender, EventArgs e)
	{
		if (_inputData is null) return;

		RunBtn.IsEnabled = false;
		RunBtn.Text = "Processing…";
		StateLabel.Text = "Processing";

		var part = (BodyPart)(BodyPartPicker.SelectedItem ?? BodyPart.Torso);
		var input = new AssetInput { FileName = _inputName!, Data = _inputData, BodyPart = part };

		_result = await _pipeline.ProcessAsync(input);

		StateLabel.Text = _result.State.ToString();
		StateLabel.TextColor = _result.State == TaskState.Synchronized ? Colors.LimeGreen
			: _result.State == TaskState.Failed ? Colors.OrangeRed : Colors.Goldenrod;

		StatsLabel.Text = BuildStats(_result.Report);

		ConsoleLabel.Text = string.Join('\n', _result.Log);
		SaveLogBtn.IsEnabled = _result.Log.Count > 0;

		if (_result.PreviewImage is not null)
		{
			var bytes = _result.PreviewImage;
			PreviewImage.Source = ImageSource.FromStream(() => new MemoryStream(bytes));
		}
		SaveTemplateBtn.IsEnabled = _result.Output is not null;

		RunBtn.IsEnabled = true;
		RunBtn.Text = "Generate";
	}

	private async void OnSaveTemplateClicked(object? sender, EventArgs e)
	{
		if (_result?.Output is null) return;
		var name = _result.OutputFileName ?? $"RB_Skin_Forge_output_{DateTime.Now:yyyyMMdd_HHmmss}";
		var path = Path.Combine(FileSystem.Current.AppDataDirectory, name);
		await File.WriteAllBytesAsync(path, _result.Output);
		await DisplayAlert("Saved", $"Output saved to:\n{path}", "OK");
	}

	private async void OnSaveLogClicked(object? sender, EventArgs e)
	{
		if (_result is null) return;
		var path = Path.Combine(FileSystem.Current.AppDataDirectory,
			$"RB_Skin_Forge_log_{DateTime.Now:yyyyMMdd_HHmmss}.txt");
		await File.WriteAllTextAsync(path, string.Join(Environment.NewLine, _result.Log));
		await DisplayAlert("Saved", $"Log saved to:\n{path}", "OK");
	}

	private static string BuildStats(QualityReport r)
	{
		var parts = new List<string>();
		if (r.OutputWidth > 0) parts.Add($"Dimensions: {r.OutputWidth} × {r.OutputHeight} px");
		if (r.TriangleCount > 0) parts.Add($"Triangles: {r.TriangleCount:N0}");
		parts.Add($"Output size: {FormatBytes(r.TextureSizeBytes)}");
		return string.Join("   |   ", parts);
	}

	private static string FormatBytes(long bytes) =>
		bytes < 1024 ? $"{bytes} B" :
		bytes < 1024 * 1024 ? $"{bytes / 1024.0:0.0} KB" :
		$"{bytes / (1024.0 * 1024.0):0.0} MB";
}
