using System.Globalization;
using System.Numerics;
using System.Text;
using RB_Skin_Forge.Core.Models;

namespace RB_Skin_Forge.Core.Services;

/// <summary>
/// Minimal, dependency-free <b>ASCII</b> FBX reader (Phase 3). It extracts geometry
/// (vertex positions + polygon indices) from one or more <c>Geometry</c> nodes and
/// triangulates them into the shared <see cref="MeshData"/> structure.
///
/// FBX has two on-disk encodings: a human-readable ASCII form and a proprietary
/// binary form. Every free, fully-managed parser only covers ASCII; the binary
/// form needs native libraries (e.g. the Autodesk FBX SDK / Assimp) that do not
/// run under Blazor WebAssembly. So binary FBX is detected up-front and rejected
/// with an actionable message rather than failing obscurely.
/// </summary>
public sealed class FbxParser
{
    /// <summary>The 21-byte magic that opens every binary FBX file.</summary>
    private static readonly byte[] BinaryMagic =
        Encoding.ASCII.GetBytes("Kaydara FBX Binary  ");

    public sealed class BinaryFbxException : Exception
    {
        public BinaryFbxException()
            : base("This is a binary FBX. RB Skin Forge reads ASCII FBX only "
                 + "(export from Blender/Maya with the ASCII option, or convert to OBJ). "
                 + "Binary FBX needs native libraries that cannot run in the browser build.") { }
    }

    public MeshData Parse(byte[] data)
    {
        if (LooksBinary(data))
            throw new BinaryFbxException();

        return Parse(Encoding.UTF8.GetString(data));
    }

    public static bool LooksBinary(byte[] data)
    {
        if (data.Length < BinaryMagic.Length) return false;
        for (int i = 0; i < BinaryMagic.Length; i++)
            if (data[i] != BinaryMagic[i]) return false;
        return true;
    }

    public MeshData Parse(string text)
    {
        var mesh = new MeshData();

        // Walk every "Vertices:" array; pair each with the next "PolygonVertexIndex:".
        int cursor = 0;
        while (true)
        {
            int vPos = text.IndexOf("Vertices:", cursor, StringComparison.Ordinal);
            if (vPos < 0) break;

            int iPos = text.IndexOf("PolygonVertexIndex:", vPos, StringComparison.Ordinal);
            if (iPos < 0) break;

            double[] coords = ReadNumberArray(text, vPos);
            double[] indices = ReadNumberArray(text, iPos);

            AddGeometry(mesh, coords, indices);
            cursor = iPos + "PolygonVertexIndex:".Length;
        }

        mesh.ComputeBounds();
        return mesh;
    }

    /// <summary>Append one Geometry node's vertices + triangulated polygons to the mesh.</summary>
    private static void AddGeometry(MeshData mesh, double[] coords, double[] indices)
    {
        int baseIndex = mesh.Positions.Count;
        for (int i = 0; i + 2 < coords.Length; i += 3)
            mesh.Positions.Add(new Vector3((float)coords[i], (float)coords[i + 1], (float)coords[i + 2]));

        // FBX encodes polygons as a flat index list; the last index of each polygon
        // is negative (bitwise-complemented) to mark the polygon boundary.
        var poly = new List<int>(4);
        foreach (double d in indices)
        {
            int raw = (int)d;
            bool end = raw < 0;
            int idx = end ? ~raw : raw;          // ~raw == -raw - 1
            poly.Add(baseIndex + idx);

            if (!end) continue;

            // Fan-triangulate the completed polygon.
            for (int k = 1; k < poly.Count - 1; k++)
            {
                mesh.Faces.Add(new Face(
                    new Corner(poly[0], -1, -1),
                    new Corner(poly[k], -1, -1),
                    new Corner(poly[k + 1], -1, -1),
                    null));
            }
            poly.Clear();
        }
    }

    /// <summary>
    /// Read the numeric array that follows a token like "Vertices:" — i.e. the
    /// comma-separated numbers inside the next <c>{ a: ... }</c> block.
    /// </summary>
    private static double[] ReadNumberArray(string text, int tokenPos)
    {
        int open = text.IndexOf('{', tokenPos);
        if (open < 0) return Array.Empty<double>();
        int close = text.IndexOf('}', open);
        if (close < 0) close = text.Length;

        string inner = text.Substring(open + 1, close - open - 1);

        var nums = new List<double>();
        foreach (var piece in inner.Split(','))
        {
            // Strip the leading "a:" key and any whitespace/newlines.
            var span = piece.AsSpan().Trim();
            int colon = span.IndexOf(':');
            if (colon >= 0) span = span[(colon + 1)..].Trim();
            if (span.Length == 0) continue;

            if (double.TryParse(span, NumberStyles.Float, CultureInfo.InvariantCulture, out double v))
                nums.Add(v);
        }
        return nums.ToArray();
    }
}
