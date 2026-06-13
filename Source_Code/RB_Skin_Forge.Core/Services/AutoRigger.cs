using System.Numerics;
using RB_Skin_Forge.Core.Abstractions;
using RB_Skin_Forge.Core.Models;

namespace RB_Skin_Forge.Core.Services;

/// <summary>
/// Phase 3 auto-rigger. Implants Roblox <b>Attachment</b> nodes onto a mesh so it
/// is ready to accept rig joints and accessories.
///
/// A single uploaded mesh isn't segmented into limbs, so this uses a heuristic:
/// it reads the axis-aligned bounding box, decides which axis is "up" (the tallest
/// one), and places the standard humanoid attachment points at well-known fractions
/// of that box — neck/head near the top, root/waist at the middle, shoulders high
/// and hips low on the widest horizontal axis. The names match Roblox's R15 rig so
/// the points are meaningful when imported.
/// </summary>
public sealed class AutoRigger : IAutoRigger
{
    /// <summary>name, up-fraction (0=bottom,1=top), side-fraction of half-width (-1=left,0=center,1=right).</summary>
    private static readonly (string Name, float Up, float Side)[] Layout =
    {
        ("HeadRootAttachment",        0.92f,  0f),
        ("NeckRigAttachment",         0.80f,  0f),
        ("LeftShoulderRigAttachment", 0.74f, -0.85f),
        ("RightShoulderRigAttachment",0.74f,  0.85f),
        ("RootRigAttachment",         0.50f,  0f),
        ("WaistRigAttachment",        0.42f,  0f),
        ("LeftHipRigAttachment",      0.30f, -0.45f),
        ("RightHipRigAttachment",     0.30f,  0.45f),
    };

    /// <summary>
    /// Compute and attach the rig points to <paramref name="mesh"/> (in place).
    /// Returns the attachments that were added.
    /// </summary>
    public IReadOnlyList<Attachment> Rig(MeshData mesh)
    {
        mesh.Attachments.Clear();
        if (mesh.Positions.Count == 0) return mesh.Attachments;

        mesh.ComputeBounds();
        Vector3 min = mesh.Min, size = mesh.Size;

        // "Up" = tallest axis; "side" = widest of the remaining two.
        int up = LongestAxis(size);
        int side = WidestOtherAxis(size, up);

        foreach (var (name, upFrac, sideFrac) in Layout)
        {
            Vector3 p = mesh.Center;
            SetAxis(ref p, up, min, size, upFrac);
            // sideFrac is in units of half-width; map to 0.5 ± frac*0.5 of the span.
            SetAxis(ref p, side, min, size, 0.5f + sideFrac * 0.5f);
            mesh.Attachments.Add(new Attachment(name, p));
        }

        return mesh.Attachments;
    }

    public Task<ProcessingResult> RigAsync(AssetInput meshInput, CancellationToken ct = default)
    {
        // Standalone entry point (not used by the main pipeline, which rigs inline
        // in the GeometryEngine). Kept so the engine is usable on its own.
        var result = new ProcessingResult { SourceFileName = meshInput.FileName, State = TaskState.Failed };
        result.Log.Add("Use the geometry pipeline; standalone rigging needs a parsed mesh.");
        return Task.FromResult(result);
    }

    private static void SetAxis(ref Vector3 p, int axis, Vector3 min, Vector3 size, float frac)
    {
        float value = Axis(min, axis) + Axis(size, axis) * frac;
        switch (axis)
        {
            case 0: p.X = value; break;
            case 1: p.Y = value; break;
            default: p.Z = value; break;
        }
    }

    private static float Axis(Vector3 v, int i) => i == 0 ? v.X : (i == 1 ? v.Y : v.Z);

    private static int LongestAxis(Vector3 s)
    {
        if (s.Y >= s.X && s.Y >= s.Z) return 1;
        if (s.X >= s.Y && s.X >= s.Z) return 0;
        return 2;
    }

    private static int WidestOtherAxis(Vector3 s, int up)
    {
        int best = -1; float bestLen = -1f;
        for (int a = 0; a < 3; a++)
        {
            if (a == up) continue;
            if (Axis(s, a) > bestLen) { bestLen = Axis(s, a); best = a; }
        }
        return best;
    }
}
