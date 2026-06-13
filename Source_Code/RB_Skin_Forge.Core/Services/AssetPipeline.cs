using RB_Skin_Forge.Core.Abstractions;
using RB_Skin_Forge.Core.Models;

namespace RB_Skin_Forge.Core.Services;

/// <summary>
/// Routes an asset to the correct engine: images to the Phase 1 image processor,
/// meshes to the Phase 2 geometry engine. Single entry point for the UIs.
/// </summary>
public sealed class AssetPipeline : IAssetPipeline
{
    private readonly IImageProcessor _image;
    private readonly IGeometryEngine _geometry;

    public AssetPipeline(IImageProcessor image, IGeometryEngine geometry)
    {
        _image = image;
        _geometry = geometry;
    }

    public Task<ProcessingResult> ProcessAsync(AssetInput input, CancellationToken ct = default)
    {
        return input.Type switch
        {
            AssetType.Image => _image.ProcessAsync(input, ct),
            AssetType.Mesh => _geometry.ProcessAsync(input, ct),
            _ => Unsupported(input)
        };
    }

    private static Task<ProcessingResult> Unsupported(AssetInput input)
    {
        var result = new ProcessingResult { SourceFileName = input.FileName, State = TaskState.Failed };
        result.Log.Add($"[Error] Unsupported file type '.{input.Extension}'. " +
                       $"Images: {string.Join(", ", RobloxSpec.ImageExtensions)}; " +
                       $"Meshes: {string.Join(", ", RobloxSpec.MeshExtensions)}.");
        return Task.FromResult(result);
    }
}
