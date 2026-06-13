using RB_Skin_Forge.Core.Abstractions;
using RB_Skin_Forge.Core.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace RB_Skin_Forge.Core.Services;

/// <summary>
/// Phase 1 image pipeline: validates the input, generates a Roblox clothing
/// template from it, measures the output, and validates it against the spec.
/// Produces a <see cref="ProcessingResult"/> with log lines for the console panel.
/// </summary>
public sealed class ImageProcessor : IImageProcessor
{
    private readonly IAssetIngestionEngine _ingestion;
    private readonly ITemplateGenerator _templates;
    private readonly ISpecValidator _validator;

    public ImageProcessor(
        IAssetIngestionEngine ingestion,
        ITemplateGenerator templates,
        ISpecValidator validator)
    {
        _ingestion = ingestion;
        _templates = templates;
        _validator = validator;
    }

    public async Task<ProcessingResult> ProcessAsync(AssetInput input, CancellationToken ct = default)
    {
        var result = new ProcessingResult { SourceFileName = input.FileName, State = TaskState.Processing };
        result.Log.Add($"Ingesting '{input.FileName}' ({input.Data.Length:N0} bytes).");

        // 1. Validate input.
        var inputCheck = _ingestion.ValidateInput(input);
        foreach (var f in inputCheck.Findings)
            result.Log.Add($"[{f.Severity}] {f.Message}");

        if (input.Type != AssetType.Image)
        {
            result.State = TaskState.Failed;
            result.Log.Add("Phase 1 only processes images. Stopping.");
            return result;
        }
        if (inputCheck.HasErrors)
        {
            result.State = TaskState.Failed;
            result.Log.Add("Input failed validation. Stopping.");
            return result;
        }

        try
        {
            // 2. Generate the template.
            result.Log.Add($"Generating {RobloxSpec.ClothingTemplateWidth}x{RobloxSpec.ClothingTemplateHeight} " +
                           $"template for {input.BodyPart}...");
            byte[] output = await _templates.GenerateAsync(input, input.BodyPart, ct);
            result.Output = output;
            result.PreviewImage = output;   // for images, the preview IS the output
            result.OutputFileName = Path.GetFileNameWithoutExtension(input.FileName) + "_template.png";
            result.OutputContentType = "image/png";

            // 3. Measure the output.
            var size = ImageInfoFor(output);
            result.Report.OutputWidth = size.Width;
            result.Report.OutputHeight = size.Height;
            result.Report.TextureSizeBytes = output.Length;
            result.Log.Add($"Template generated: {size.Width}x{size.Height}, {output.Length:N0} bytes.");

            // 4. Validate against Roblox spec.
            var specCheck = _validator.Validate(result.Report);
            result.Report.Validation = specCheck;
            foreach (var f in specCheck.Findings)
                result.Log.Add($"[{f.Severity}] {f.Message}");

            result.State = specCheck.HasErrors ? TaskState.Failed : TaskState.Synchronized;
        }
        catch (OperationCanceledException)
        {
            result.State = TaskState.Failed;
            result.Log.Add("Processing cancelled.");
        }
        catch (Exception ex)
        {
            result.State = TaskState.Failed;
            result.Log.Add($"[Error] {ex.Message}");
        }

        return result;
    }

    private static (int Width, int Height) ImageInfoFor(byte[] bytes)
    {
        using var img = Image.Load<Rgba32>(bytes);
        return (img.Width, img.Height);
    }
}
