using System.Globalization;
using System.Numerics;
using System.Text;
using RB_Skin_Forge.Core.Abstractions;
using RB_Skin_Forge.Core.Models;

namespace RB_Skin_Forge.Core.Services;

/// <summary>
/// Geometry engine for 3D meshes.
/// <para>Phase 2: OBJ parse, measure, remove degenerate triangles, center +
/// scale to fit Roblox bounds, validate, export cleaned OBJ, wireframe preview.</para>
/// <para>Phase 3: ASCII FBX input, automatic decimation when a mesh exceeds the
/// triangle limit, and auto-rigging (Roblox attachment-node implantation).</para>
/// </summary>
public sealed class GeometryEngine : IGeometryEngine
{
    private readonly ObjParser _objParser;
    private readonly FbxParser _fbxParser;
    private readonly MeshDecimator _decimator;
    private readonly AutoRigger _rigger;
    private readonly ISpecValidator _validator;
    private readonly MeshRenderer _renderer;

    public GeometryEngine(
        ObjParser objParser,
        FbxParser fbxParser,
        MeshDecimator decimator,
        AutoRigger rigger,
        ISpecValidator validator,
        MeshRenderer renderer)
    {
        _objParser = objParser;
        _fbxParser = fbxParser;
        _decimator = decimator;
        _rigger = rigger;
        _validator = validator;
        _renderer = renderer;
    }

    public Task<ProcessingResult> ProcessAsync(AssetInput meshInput, CancellationToken ct = default)
    {
        return Task.Run(() =>
        {
            var result = new ProcessingResult { SourceFileName = meshInput.FileName, State = TaskState.Processing };
            result.Log.Add($"Ingesting mesh '{meshInput.FileName}' ({meshInput.Data.Length:N0} bytes).");

            try
            {
                ct.ThrowIfCancellationRequested();

                MeshData mesh = ParseByExtension(meshInput, result);
                if (mesh.TriangleCount == 0)
                {
                    result.State = TaskState.Failed;
                    result.Log.Add("[Error] No triangles found. Is this a valid OBJ/FBX mesh?");
                    return result;
                }

                result.Log.Add($"Parsed {mesh.VertexCount:N0} vertices, {mesh.TriangleCount:N0} triangles, " +
                               $"{mesh.Materials.Count} material(s).");
                int original = mesh.TriangleCount;
                result.Report.OriginalTriangleCount = original;

                int removed = RemoveDegenerateFaces(mesh);
                if (removed > 0) result.Log.Add($"Removed {removed} degenerate (zero-area) triangle(s).");

                mesh.ComputeBounds();
                result.Log.Add($"Bounds: {Fmt(mesh.Size)} (studs), center {Fmt(mesh.Center)}.");

                NormalizeScale(mesh, result);

                // Phase 3: decimate if we are over the Roblox triangle budget.
                if (mesh.TriangleCount > RobloxSpec.MaxTrianglesPerMesh)
                {
                    result.Log.Add($"Mesh exceeds the {RobloxSpec.MaxTrianglesPerMesh:N0}-triangle limit; decimating...");
                    int after = _decimator.Decimate(mesh, RobloxSpec.MaxTrianglesPerMesh);
                    result.Report.WasDecimated = true;
                    result.Log.Add($"Decimated {original:N0} → {after:N0} triangles (vertex clustering).");
                }

                if (mesh.Normals.Count == 0)
                {
                    GenerateFlatNormals(mesh);
                    result.Log.Add("Generated flat per-face normals.");
                }

                // Phase 3: auto-rigging — implant Roblox attachment nodes.
                var attachments = _rigger.Rig(mesh);
                result.Log.Add($"Auto-rigger placed {attachments.Count} Roblox attachment node(s):");
                foreach (var a in attachments)
                    result.Log.Add($"    • {a.Name} @ {Fmt(a.Position)}");

                // Quality report.
                result.Report.TriangleCount = mesh.TriangleCount;
                result.Report.VertexCount = mesh.VertexCount;
                result.Report.MaterialCount = mesh.Materials.Count;
                result.Report.AttachmentCount = attachments.Count;
                result.Report.BoundsX = mesh.Size.X;
                result.Report.BoundsY = mesh.Size.Y;
                result.Report.BoundsZ = mesh.Size.Z;

                byte[] obj = ExportObj(mesh);
                result.Output = obj;
                result.OutputFileName = Path.GetFileNameWithoutExtension(meshInput.FileName) + "_roblox.obj";
                result.OutputContentType = "text/plain";
                result.Report.TextureSizeBytes = obj.Length;
                result.Log.Add($"Exported cleaned OBJ (with attachment metadata): {obj.Length:N0} bytes.");

                var specCheck = _validator.Validate(result.Report);
                result.Report.Validation = specCheck;
                foreach (var f in specCheck.Findings)
                    result.Log.Add($"[{f.Severity}] {f.Message}");

                result.PreviewImage = _renderer.RenderWireframe(mesh, 512);

                result.State = specCheck.HasErrors ? TaskState.Failed : TaskState.Synchronized;
            }
            catch (FbxParser.BinaryFbxException ex)
            {
                result.State = TaskState.Failed;
                result.Log.Add($"[Error] {ex.Message}");
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

    private MeshData ParseByExtension(AssetInput meshInput, ProcessingResult result)
    {
        switch (meshInput.Extension)
        {
            case "obj":
                return _objParser.Parse(meshInput.Data);
            case "fbx":
                result.Log.Add("Reading ASCII FBX...");
                return _fbxParser.Parse(meshInput.Data);
            default:
                throw new NotSupportedException(
                    $"Unsupported mesh type '.{meshInput.Extension}'. Supported: " +
                    string.Join(", ", RobloxSpec.MeshExtensions) + ".");
        }
    }

    private static int RemoveDegenerateFaces(MeshData mesh)
    {
        int before = mesh.Faces.Count;
        mesh.Faces.RemoveAll(f =>
        {
            var a = mesh.Positions[f.A.P];
            var b = mesh.Positions[f.B.P];
            var c = mesh.Positions[f.C.P];
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
        sb.AppendLine("# Generated by RB Skin Forge (Phase 3)");
        var ci = CultureInfo.InvariantCulture;

        // Attachment nodes recorded as comments so they survive a round-trip and
        // can be read back / scripted into Roblox.
        foreach (var at in mesh.Attachments)
            sb.AppendLine($"# attachment {at.Name} " +
                $"{at.Position.X.ToString("0.######", ci)} " +
                $"{at.Position.Y.ToString("0.######", ci)} " +
                $"{at.Position.Z.ToString("0.######", ci)}");

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
