namespace RB_Skin_Forge.Core.Models;

/// <summary>
/// A single user-provided file entering the pipeline (image or mesh).
/// The raw bytes are kept in memory so the same model works across the
/// HTML (Blazor), .exe and .apk front-ends without a filesystem dependency.
/// </summary>
public sealed class AssetInput
{
    public required string FileName { get; init; }
    public required byte[] Data { get; init; }

    /// <summary>Target body part, if the user assigned one.</summary>
    public BodyPart BodyPart { get; set; } = BodyPart.Unknown;

    /// <summary>Lower-case extension without the leading dot.</summary>
    public string Extension =>
        System.IO.Path.GetExtension(FileName).TrimStart('.').ToLowerInvariant();

    public AssetType Type
    {
        get
        {
            var ext = Extension;
            if (System.Array.IndexOf(RobloxSpec.ImageExtensions, ext) >= 0) return AssetType.Image;
            if (System.Array.IndexOf(RobloxSpec.MeshExtensions, ext) >= 0) return AssetType.Mesh;
            return AssetType.Unknown;
        }
    }
}
