namespace RB_Skin_Forge.Core.Models;

/// <summary>
/// Roblox platform specifications used to validate generated assets.
/// Values are sourced from Roblox's published creator limits and are kept
/// in one place so they are easy to update if Roblox changes them.
/// </summary>
public static class RobloxSpec
{
    // --- 2D clothing templates ---------------------------------------------

    /// <summary>Classic clothing template canvas width, in pixels.</summary>
    public const int ClothingTemplateWidth = 585;

    /// <summary>Classic clothing template canvas height, in pixels.</summary>
    public const int ClothingTemplateHeight = 559;

    /// <summary>Maximum texture dimension accepted by image uploads.</summary>
    public const int MaxTextureDimension = 1024;

    // --- 3D mesh limits (used in Phase 2/3) --------------------------------

    /// <summary>Maximum triangles for a single mesh part.</summary>
    public const int MaxTrianglesPerMesh = 10_000;

    /// <summary>Maximum bounding-box size per axis, in studs.</summary>
    public const int MaxMeshBoundsStuds = 2_048;

    /// <summary>Accepted image input extensions (lower-case, no dot).</summary>
    public static readonly string[] ImageExtensions = { "png", "jpg", "jpeg", "bmp", "tga" };

    /// <summary>Accepted mesh input extensions (lower-case, no dot).</summary>
    public static readonly string[] MeshExtensions = { "obj", "fbx" };
}
