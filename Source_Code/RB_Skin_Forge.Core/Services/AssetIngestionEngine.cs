using RB_Skin_Forge.Core.Abstractions;
using RB_Skin_Forge.Core.Models;

namespace RB_Skin_Forge.Core.Services;

/// <summary>Default ingestion engine: classifies input and checks it is supported.</summary>
public sealed class AssetIngestionEngine : IAssetIngestionEngine
{
    public AssetType Classify(AssetInput input) => input.Type;

    public ValidationResult ValidateInput(AssetInput input)
    {
        var result = new ValidationResult();

        if (input.Data.Length == 0)
            result.Add(Severity.Error, $"'{input.FileName}' is empty.");

        switch (input.Type)
        {
            case AssetType.Unknown:
                result.Add(Severity.Error,
                    $"Unsupported file type '.{input.Extension}'. " +
                    $"Images: {string.Join(", ", RobloxSpec.ImageExtensions)}; " +
                    $"Meshes: {string.Join(", ", RobloxSpec.MeshExtensions)}.");
                break;
            case AssetType.Mesh:
                if (input.Extension != "obj")
                    result.Add(Severity.Warning,
                        $"'.{input.Extension}' meshes (and auto-rigging) arrive in Phase 3. " +
                        "OBJ is supported now.");
                break;
        }

        return result;
    }
}
