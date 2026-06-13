using System.Numerics;

namespace RB_Skin_Forge.Core.Models;

/// <summary>One corner of a triangle: indices into the mesh's position/uv/normal lists (0-based, -1 if absent).</summary>
public struct Corner
{
    public int P;
    public int T;
    public int N;

    public Corner(int p, int t, int n) { P = p; T = t; N = n; }
}

/// <summary>A triangle (three corners) plus the material that was active when it was defined.</summary>
public struct Face
{
    public Corner A;
    public Corner B;
    public Corner C;
    public string? Material;

    public Face(Corner a, Corner b, Corner c, string? material)
    {
        A = a; B = b; C = c; Material = material;
    }
}

/// <summary>
/// A triangulated mesh: raw position/uv/normal lists plus faces that index into them.
/// Kept format-agnostic so OBJ now (and FBX in Phase 3) can populate the same structure.
/// </summary>
public sealed class MeshData
{
    public List<Vector3> Positions { get; } = new();
    public List<Vector2> TexCoords { get; } = new();
    public List<Vector3> Normals { get; } = new();
    public List<Face> Faces { get; } = new();
    public HashSet<string> Materials { get; } = new();

    public int TriangleCount => Faces.Count;
    public int VertexCount => Positions.Count;

    public Vector3 Min { get; private set; }
    public Vector3 Max { get; private set; }
    public Vector3 Size => Max - Min;
    public Vector3 Center => (Min + Max) * 0.5f;

    /// <summary>Recompute the axis-aligned bounding box from the current positions.</summary>
    public void ComputeBounds()
    {
        if (Positions.Count == 0)
        {
            Min = Max = Vector3.Zero;
            return;
        }

        var min = new Vector3(float.MaxValue);
        var max = new Vector3(float.MinValue);
        foreach (var p in Positions)
        {
            min = Vector3.Min(min, p);
            max = Vector3.Max(max, p);
        }
        Min = min;
        Max = max;
    }
}
