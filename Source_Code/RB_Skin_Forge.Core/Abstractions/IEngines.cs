using RB_Skin_Forge.Core.Models;

namespace RB_Skin_Forge.Core.Abstractions;

/// <summary>
/// Top-level entry point. Routes an asset to the right engine (image vs mesh)
/// so the UIs depend on one thing regardless of input type.
/// </summary>
public interface IAssetPipeline
{
    Task<ProcessingResult> ProcessAsync(AssetInput input, CancellationToken ct = default);
}

/// <summary>
/// Inspects raw input and decides what kind of asset it is and whether the
/// app can handle it. First stage of the pipeline (Asset Ingestion Engine).
/// </summary>
public interface IAssetIngestionEngine
{
    AssetType Classify(AssetInput input);
    ValidationResult ValidateInput(AssetInput input);
}

/// <summary>
/// 2D image work: slicing into template regions and edge-completion so the
/// clothing fills the Roblox template without gaps. (Phase 1 — image based.)
/// </summary>
public interface IImageProcessor
{
    Task<ProcessingResult> ProcessAsync(AssetInput input, CancellationToken ct = default);
}

/// <summary>
/// Smart Template Generator: extends/fills blank areas and lays out the image
/// onto the standard Roblox clothing template. (Phase 1 — image based.)
/// </summary>
public interface ITemplateGenerator
{
    Task<byte[]> GenerateAsync(AssetInput input, BodyPart part, CancellationToken ct = default);
}

/// <summary>
/// Validates a finished asset (2D or 3D) against <see cref="RobloxSpec"/>.
/// </summary>
public interface ISpecValidator
{
    ValidationResult Validate(QualityReport report);
}

// --- 3D engines: contracts defined now, implemented in Phase 2/3 -----------

/// <summary>
/// Geometry processing engine: mesh optimization, scale adjustment, and
/// attachment-node implantation. NOT YET IMPLEMENTED — see GeometryEngineStub.
/// </summary>
public interface IGeometryEngine
{
    Task<ProcessingResult> ProcessAsync(AssetInput meshInput, CancellationToken ct = default);
}

/// <summary>
/// Auto-Rigging: detects head/body/accessory and inserts Roblox Attachment
/// nodes. NOT YET IMPLEMENTED — see AutoRiggerStub.
/// </summary>
public interface IAutoRigger
{
    Task<ProcessingResult> RigAsync(AssetInput meshInput, CancellationToken ct = default);
}
