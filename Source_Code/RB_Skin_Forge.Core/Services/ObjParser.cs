using System.Globalization;
using System.Numerics;
using RB_Skin_Forge.Core.Models;

namespace RB_Skin_Forge.Core.Services;

/// <summary>
/// Minimal, dependency-free Wavefront OBJ parser. Handles v / vt / vn / f,
/// material directives (usemtl/mtllib), negative (relative) indices, and
/// triangulates polygons with more than three sides via a simple fan.
/// </summary>
public sealed class ObjParser
{
    public MeshData Parse(byte[] data) =>
        Parse(System.Text.Encoding.UTF8.GetString(data));

    public MeshData Parse(string text)
    {
        var mesh = new MeshData();
        string? currentMaterial = null;

        foreach (var raw in text.Split('\n'))
        {
            var line = raw.Trim();
            if (line.Length == 0 || line[0] == '#') continue;

            int sp = line.IndexOf(' ');
            string tag = sp < 0 ? line : line[..sp];
            string rest = sp < 0 ? "" : line[(sp + 1)..].Trim();

            switch (tag)
            {
                case "v":
                    mesh.Positions.Add(ParseVec3(rest));
                    break;
                case "vt":
                    mesh.TexCoords.Add(ParseVec2(rest));
                    break;
                case "vn":
                    mesh.Normals.Add(ParseVec3(rest));
                    break;
                case "usemtl":
                    currentMaterial = rest;
                    if (rest.Length > 0) mesh.Materials.Add(rest);
                    break;
                case "f":
                    AddFace(mesh, rest, currentMaterial);
                    break;
                // mtllib / o / g / s and anything else: ignored for Phase 2.
            }
        }

        mesh.ComputeBounds();
        return mesh;
    }

    private static Vector3 ParseVec3(string s)
    {
        var t = SplitWhitespace(s);
        return new Vector3(F(t, 0), F(t, 1), F(t, 2));
    }

    private static Vector2 ParseVec2(string s)
    {
        var t = SplitWhitespace(s);
        return new Vector2(F(t, 0), F(t, 1));
    }

    private static void AddFace(MeshData mesh, string rest, string? material)
    {
        var tokens = SplitWhitespace(rest);
        if (tokens.Length < 3) return;

        var corners = new Corner[tokens.Length];
        for (int i = 0; i < tokens.Length; i++)
            corners[i] = ParseCorner(tokens[i], mesh);

        // Fan triangulation: (0, i, i+1).
        for (int i = 1; i < corners.Length - 1; i++)
            mesh.Faces.Add(new Face(corners[0], corners[i], corners[i + 1], material));
    }

    private static Corner ParseCorner(string token, MeshData mesh)
    {
        // token forms: v | v/vt | v//vn | v/vt/vn
        var parts = token.Split('/');
        int p = ResolveIndex(parts[0], mesh.Positions.Count);
        int t = parts.Length > 1 ? ResolveIndex(parts[1], mesh.TexCoords.Count) : -1;
        int n = parts.Length > 2 ? ResolveIndex(parts[2], mesh.Normals.Count) : -1;
        return new Corner(p, t, n);
    }

    /// <summary>OBJ indices are 1-based; negative values count back from the end.</summary>
    private static int ResolveIndex(string s, int count)
    {
        if (string.IsNullOrEmpty(s)) return -1;
        if (!int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out int idx)) return -1;
        return idx > 0 ? idx - 1 : count + idx;
    }

    private static string[] SplitWhitespace(string s) =>
        s.Split(new[] { ' ', '\t', '\r' }, StringSplitOptions.RemoveEmptyEntries);

    private static float F(string[] t, int i) =>
        i < t.Length && float.TryParse(t[i], NumberStyles.Float, CultureInfo.InvariantCulture, out float v)
            ? v : 0f;
}
