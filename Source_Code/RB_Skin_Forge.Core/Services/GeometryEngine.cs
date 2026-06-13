using System.Globalization;
using System.Numerics;
using System.Text;
using RB_Skin_Forge.Core.Abstractions;
using RB_Skin_Forge.Core.Models;

namespace RB_Skin_Forge.Core.Services;

/// <summary>
/// Phase 2 geometry engine for OBJ meshes: parse, measure, remove degenerate
/// triangles, center + scale to fit Roblox bounds, validate against the spec,
/// export a cleaned OBJ, and render a wireframe preview.
/// </summary>
public sealed class GeometryEngine : IGeometryEngine
{
    private readonly ObjParser _parser;
    private readonly ISpecValidator _validator;
    private readonly MeshRenderer _renderer;

    public GeometryEngine(ObjParser parser, ISpecValidator validator, MeshRenderer renderer)
    {
        _parser = parser;
        _validator = validator;
        _renderer = renderer;
    }

    public Task<ProcessingResult> ProcessAsync(AssetInput meshInput, CancellationToken ct = default)
    {
        return Task.Run(() =>
        {
            var result = new ProcessingResult { SourceFileName = meshInput.FileName, State = TaskState.Processing };
            result.Log.Add($"Ingesting mesh '{meshInput.FileName}' ({meshInput.Data.Length:N0} bytes).");

            if (meshInput.Extension != "obj")
            {
                result.State = TaskState.Failed;
                result.Log.Add($"[Error] Phase 2 supports OBJ only; '.{meshInput.Extension}' arrives in Phase 3.");
                return result;
            }

            try
            {
                ct.ThrowIfCancellationRequested();

                var mesh = _parser.Parse(meshInput.Data);
                result.Log.Add($"Parsed {mesh.VertexCount:N0} vertices, {mesh.TriangleCount:N0} triangles, " +
                               $"{mesh.Materials.Count} material(s).");

                if (mesh.TriangleCount == 0)
                {
                    result.State = TaskState.Failed;
                    result.Log.Add("[Error] No triangles found. Is this a valid OBJ?");
                    return result;
                }

                int removed = RemoveDegenerateFaces(mesh);
                if (removed > 0) result.Log.Add($"Removed {removed} degenerate (zero-area) triangle(s).");

                mesh.ComputeBounds();
                result.Log.Add($"Bounds: {Fmt(mesh.Size)} (studs), center {Fmt(mesh.Center)}.");

                NormalizeScale(mesh, result);

                if (mesh.Normals.Count == 0)
                {
                    GenerateFlatNormals(mesh);
                    result.Log.Add("Source had no normals; generated flat per-face normals.");
                }

                // Quality report (no 2D texture, so width/height stay 0).
                result.Report.TriangleCount = mesh.TriangleCount;

                byte[] obj = ExportObj(mesh);
                result.Output = obj;
                result.OutputFileName = Path.GetFileNameWithoutExtension(meshInput.FileName) + "_roblox.obj";
                result.OutputContentType = "text/plain";
                result.Report.TextureSizeBytes = obj.Length;
                result.Log.Add($"Exported cleaned OBJ: {obj.Length:N0} bytes.");

                // Spec validation (triangle limit etc.).
                var specCheck = _validator.Validate(result.Report);
                result.Report.Validation = specCheck;
                foreach (var f in specCheck.Findings)
                    result.Log.Add($"[{f.Severity}] {f.Message}");

                // Wireframe preview.
                result.PreviewImage = _renderer.RenderWireframe(mesh, 512);

                result.State = specCheck.HasErrors ? TaskState.Failed : TaskState.Synchronized;
            }
            catch (OperationCanceledException)
            {
                result.State = TaskState.Failed;
                result.Log.Add("Processing cancelled.");
            }
            catch (Exception ex)
            {
                result.State = TaskState.Failed;
                result.Log.Add($"[Error] {ex.Message}");
            }

            return result;
        }, ct);
    }

    private static int RemoveDegenerateFaces(MeshData mesh)
    {
        int before = mesh.Faces.Count;
        mesh.Faces.RemoveAll(f =>
        {
            var a = mesh.Positions[f.A.P];
            var b = mesh.Positions[f.B.P];
            var c = mesh.Positions[f.C.P];
            // Zero area => cross product length ~ 0.
            return Vector3.Cross(b - a, c - a).LengthSquared() < 1e-12f;
        });
        return before - mesh.Faces.Count;
    }

    /// <summary>Center the mesh at the origin; if it exceeds Roblox bounds, scale to fit.</summary>
    private static void NormalizeScale(MeshData mesh, ProcessingResult result)
    {
        var center = mesh.Center;
        float maxDim = Math.Max(mesh.Size.X, Math.Max(mesh.Size.Y, mesh.Size.Z));

        float scale = 1f;
        if (maxDim > RobloxSpec.MaxMeshBoundsStuds)
            scale = RobloxSpec.MaxMeshBoundsStuds / maxDim;

        for (int i = 0; i < mesh.Positions.Count; i++)
            mesh.Positions[i] = (mesh.Positions[i] - center) * scale;

        mesh.ComputeBounds();
        result.Log.Add(scale < 1f
            ? $"Centered at origin and scaled ×{scale:0.000} to fit the {RobloxSpec.MaxMeshBoundsStuds}-stud limit."
            : "Centered at origin (within bounds; no scaling needed).");
    }

    private static void GenerateFlatNormals(MeshData mesh)
    {
        for (int i = 0; i < mesh.Faces.Count; i++)
        {
            var f = mesh.Faces[i];
            var a = mesh.Positions[f.A.P];
            var b = mesh.Positions[f.B.P];
            var c = mesh.Positions[f.C.P];
            var n = Vector3.Normalize(Vector3.Cross(b - a, c - a));
            if (!float.IsFinite(n.X)) n = Vector3.UnitY;

            int ni = mesh.Normals.Count;
            mesh.Normals.Add(n);
            f.A.N = ni; f.B.N = ni; f.C.N = ni;
            mesh.Faces[i] = f;
        }
    }

    private static byte[] ExportObj(MeshData mesh)
    {
        var sb = new StringBuilder();
        sb.AppendLine("# Generated by RB Skin Forge (Phase 2)");
        var ci = CultureInfo.InvariantCulture;

        foreach (var p in mesh.Positions)
            sb.AppendLine($"v {p.X.ToString("0.######", ci)} {p.Y.ToString("0.######", ci)} {p.Z.ToString("0.######", ci)}");
        foreach (var t in mesh.TexCoords)
            sb.AppendLine($"vt {t.X.ToString("0.######", ci)} {t.Y.ToString("0.######", ci)}");
        foreach (var n in mesh.Normals)
            sb.AppendLine($"vn {n.X.ToString("0.######", ci)} {n.Y.ToString("0.######", ci)} {n.Z.ToString("0.######", ci)}");

        foreach (var f in mesh.Faces)
            sb.AppendLine($"f {Corner(f.A)} {Corner(f.B)} {Corner(f.C)}");

        return Encoding.UTF8.GetBytes(sb.ToString());
    }

    /// <summary>Format a corner as OBJ 1-based v/vt/vn, omitting absent indices.</summary>
    private static string Corner(Corner c)
    {
        string v = (c.P + 1).ToString(CultureInfo.InvariantCulture);
        string t = c.T >= 0 ? (c.T + 1).ToString(CultureInfo.InvariantCulture) : "";
        string n = c.N >= 0 ? (c.N + 1).ToString(CultureInfo.InvariantCulture) : "";
        if (c.N >= 0) return $"{v}/{t}/{n}";
        if (c.T >= 0) return $"{v}/{t}";
        return v;
    }

    private static string Fmt(Vector3 v) =>
        $"{v.X:0.##} × {v.Y:0.##} × {v.Z:0.##}";
}
