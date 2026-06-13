using RB_Skin_Forge.Core.Abstractions;
using RB_Skin_Forge.Core.Models;

namespace RB_Skin_Forge.Core.Services;

/// <summary>Validates a produced asset's metrics against <see cref="RobloxSpec"/>.</summary>
public sealed class RobloxSpecValidator : ISpecValidator
{
    public ValidationResult Validate(QualityReport report)
    {
        var result = new ValidationResult();

        if (report.OutputWidth > RobloxSpec.MaxTextureDimension ||
            report.OutputHeight > RobloxSpec.MaxTextureDimension)
        {
            result.Add(Severity.Error,
                $"Texture {report.OutputWidth}x{report.OutputHeight} exceeds the " +
                $"{RobloxSpec.MaxTextureDimension}px limit on at least one side.");
        }

        if (report.TriangleCount > RobloxSpec.MaxTrianglesPerMesh)
        {
            result.Add(Severity.Error,
                $"Mesh has {report.TriangleCount:N0} triangles; limit is " +
                $"{RobloxSpec.MaxTrianglesPerMesh:N0}.");
        }

        if (result.Findings.Count == 0)
            result.Add(Severity.Info, "Asset meets all checked Roblox specifications.");

        return result;
    }
}
