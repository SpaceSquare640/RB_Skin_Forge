using System.Numerics;
using RB_Skin_Forge.Core.Models;

namespace RB_Skin_Forge.Core.Services;

/// <summary>
/// Reduces a mesh's triangle count to fit Roblox's per-mesh limit (Phase 3).
///
/// Uses <b>vertex clustering</b>: overlay a uniform 3D grid on the bounding box,
/// snap every vertex to its cell's representative, then rebuild the faces against
/// those representatives and drop any triangle whose corners collapsed into the
/// same cell. It is fast, never crashes on messy input, and degrades gracefully —
/// the grid resolution is searched so the result lands at or under the target.
/// (Quadric-error decimation gives nicer results but is far heavier; clustering is
/// the right trade-off for a browser/WASM build.)
/// </summary>
public sealed class MeshDecimator
{
    /// <summary>
    /// Decimate <paramref name="mesh"/> in place so it has at most
    /// <paramref name="targetTriangles"/> triangles. No-op if already under.
    /// Returns the triangle count after decimation.
    /// </summary>
    public int Decimate(MeshData mesh, int targetTriangles)
    {
        if (mesh.TriangleCount <= targetTriangles || mesh.Positions.Count == 0)
            return mesh.TriangleCount;

        mesh.ComputeBounds();

        // Triangle count grows with grid resolution, so walk coarse → fine and keep
        // the FINEST grid that still fits the budget (maximum detail under the cap).
        // Stop as soon as a resolution overshoots — finer ones only overshoot more.
        List<Face>? bestFaces = null;
        List<Vector3>? bestPositions = null;

        for (int res = 4; res <= 256; res *= 2)
        {
            var (positions, faces) = Cluster(mesh, res);
            if (faces.Count <= targetTriangles)
            {
                bestFaces = faces;
                bestPositions = positions;
            }
            else
            {
                break; // overshot — keep the previous (finest fitting) result
            }
        }

        // If even the coarsest grid overshoots (pathological), fall back to it.
        if (bestFaces is null || bestPositions is null)
            (bestPositions, bestFaces) = Cluster(mesh, 4);

        ReplaceGeometry(mesh, bestPositions, bestFaces);
        return mesh.TriangleCount;
    }

    private static (List<Vector3> positions, List<Face> faces) Cluster(MeshData mesh, int res)
    {
        Vector3 min = mesh.Min;
        Vector3 size = mesh.Size;
        // Avoid divide-by-zero on flat axes.
        Vector3 inv = new(
            size.X > 1e-6f ? res / size.X : 0f,
            size.Y > 1e-6f ? res / size.Y : 0f,
            size.Z > 1e-6f ? res / size.Z : 0f);

        // Map each cell to a new vertex index + accumulate centroids.
        var cellToIndex = new Dictionary<long, int>();
        var accum = new List<Vector3>();
        var counts = new List<int>();
        var vertexCell = new int[mesh.Positions.Count];

        for (int i = 0; i < mesh.Positions.Count; i++)
        {
            Vector3 p = mesh.Positions[i];
            int cx = Clamp((int)((p.X - min.X) * inv.X), res);
            int cy = Clamp((int)((p.Y - min.Y) * inv.Y), res);
            int cz = Clamp((int)((p.Z - min.Z) * inv.Z), res);
            long key = ((long)cx * (res + 1) + cy) * (res + 1) + cz;

            if (!cellToIndex.TryGetValue(key, out int ni))
            {
                ni = accum.Count;
                cellToIndex[key] = ni;
                accum.Add(Vector3.Zero);
                counts.Add(0);
            }
            accum[ni] += p;
            counts[ni]++;
            vertexCell[i] = ni;
        }

        var positions = new List<Vector3>(accum.Count);
        for (int i = 0; i < accum.Count; i++)
            positions.Add(accum[i] / Math.Max(1, counts[i]));

        // Rebuild faces; drop any that collapsed (two corners in the same cell)
        // and any exact duplicate triangle.
        var faces = new List<Face>(mesh.Faces.Count);
        var seen = new HashSet<long>();
        foreach (var f in mesh.Faces)
        {
            int a = vertexCell[f.A.P];
            int b = vertexCell[f.B.P];
            int c = vertexCell[f.C.P];
            if (a == b || b == c || a == c) continue;

            long sig = TriSignature(a, b, c);
            if (!seen.Add(sig)) continue;

            faces.Add(new Face(new Corner(a, -1, -1), new Corner(b, -1, -1), new Corner(c, -1, -1), f.Material));
        }

        return (positions, faces);
    }

    private static void ReplaceGeometry(MeshData mesh, List<Vector3> positions, List<Face> faces)
    {
        mesh.Positions.Clear();
        mesh.Positions.AddRange(positions);
        mesh.TexCoords.Clear();   // clustering invalidates per-vertex UVs/normals
        mesh.Normals.Clear();
        mesh.Faces.Clear();
        mesh.Faces.AddRange(faces);
        mesh.ComputeBounds();
    }

    private static int Clamp(int v, int res) => v < 0 ? 0 : (v > res ? res : v);

    /// <summary>Order-independent signature so duplicate triangles collapse.</summary>
    private static long TriSignature(int a, int b, int c)
    {
        // Sort the three ids, then pack. Caps ~2M verts per id which is plenty.
        if (a > b) (a, b) = (b, a);
        if (b > c) (b, c) = (c, b);
        if (a > b) (a, b) = (b, a);
        return ((long)a * 2_097_152L + b) * 2_097_152L + c;
    }
}
