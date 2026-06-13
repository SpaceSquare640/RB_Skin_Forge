using System.Numerics;
using RB_Skin_Forge.Core.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace RB_Skin_Forge.Core.Services;

/// <summary>
/// Renders a mesh to a flat wireframe PNG for the Preview panel — no 3D viewport
/// or extra packages needed. Uses an isometric-style view and draws triangle
/// edges with a Bresenham line into the pixel buffer.
/// </summary>
public sealed class MeshRenderer
{
    private static readonly Rgba32 Background = new(15, 20, 25, 255);   // #0f1419
    private static readonly Rgba32 LineColor = new(47, 129, 247, 255);  // accent blue

    public byte[] RenderWireframe(MeshData mesh, int size = 512)
    {
        using var img = new Image<Rgba32>(size, size, Background);

        if (mesh.Positions.Count == 0 || mesh.Faces.Count == 0)
            return Encode(img);

        // Isometric-ish view: rotate around Y then X.
        var view = Matrix4x4.CreateRotationY(0.6f) * Matrix4x4.CreateRotationX(-0.45f);

        // Project all positions, tracking 2D bounds for fit-to-canvas scaling.
        var pts = new Vector2[mesh.Positions.Count];
        float minX = float.MaxValue, minY = float.MaxValue, maxX = float.MinValue, maxY = float.MinValue;
        for (int i = 0; i < mesh.Positions.Count; i++)
        {
            var v = Vector3.Transform(mesh.Positions[i] - mesh.Center, view);
            pts[i] = new Vector2(v.X, v.Y);
            minX = Math.Min(minX, v.X); maxX = Math.Max(maxX, v.X);
            minY = Math.Min(minY, v.Y); maxY = Math.Max(maxY, v.Y);
        }

        float spanX = Math.Max(maxX - minX, 1e-4f);
        float spanY = Math.Max(maxY - minY, 1e-4f);
        float margin = size * 0.08f;
        float scale = Math.Min((size - 2 * margin) / spanX, (size - 2 * margin) / spanY);
        float cx = size / 2f, cy = size / 2f;
        float midX = (minX + maxX) / 2f, midY = (minY + maxY) / 2f;

        Point Screen(int idx)
        {
            var p = pts[idx];
            int sx = (int)(cx + (p.X - midX) * scale);
            int sy = (int)(cy - (p.Y - midY) * scale); // flip Y for screen space
            return new Point(sx, sy);
        }

        img.ProcessPixelRows(accessor =>
        {
            foreach (var f in mesh.Faces)
            {
                var a = Screen(f.A.P);
                var b = Screen(f.B.P);
                var c = Screen(f.C.P);
                DrawLine(accessor, a, b);
                DrawLine(accessor, b, c);
                DrawLine(accessor, c, a);
            }
        });

        return Encode(img);
    }

    private static byte[] Encode(Image<Rgba32> img)
    {
        using var ms = new MemoryStream();
        img.SaveAsPng(ms);
        return ms.ToArray();
    }

    private static void DrawLine(PixelAccessor<Rgba32> acc, Point p0, Point p1)
    {
        int x0 = p0.X, y0 = p0.Y, x1 = p1.X, y1 = p1.Y;
        int dx = Math.Abs(x1 - x0), dy = -Math.Abs(y1 - y0);
        int sx = x0 < x1 ? 1 : -1, sy = y0 < y1 ? 1 : -1;
        int err = dx + dy;

        while (true)
        {
            if ((uint)y0 < (uint)acc.Height && (uint)x0 < (uint)acc.Width)
                acc.GetRowSpan(y0)[x0] = LineColor;

            if (x0 == x1 && y0 == y1) break;
            int e2 = 2 * err;
            if (e2 >= dy) { err += dy; x0 += sx; }
            if (e2 <= dx) { err += dx; y0 += sy; }
        }
    }
}
