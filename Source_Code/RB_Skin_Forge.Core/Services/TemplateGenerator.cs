using RB_Skin_Forge.Core.Abstractions;
using RB_Skin_Forge.Core.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace RB_Skin_Forge.Core.Services;

/// <summary>
/// Phase 1 template generator. Fits the source image onto the standard Roblox
/// clothing-template canvas (585x559) preserving aspect ratio, then fills the
/// surrounding margin by extending (clamping) the edge pixels outward so the
/// finished template has no transparent gaps.
/// </summary>
public sealed class TemplateGenerator : ITemplateGenerator
{
    public Task<byte[]> GenerateAsync(AssetInput input, BodyPart part, CancellationToken ct = default)
    {
        // ImageSharp work is CPU-bound; run it off the calling thread.
        return Task.Run(() =>
        {
            ct.ThrowIfCancellationRequested();

            const int canvasW = RobloxSpec.ClothingTemplateWidth;
            const int canvasH = RobloxSpec.ClothingTemplateHeight;

            using var source = Image.Load<Rgba32>(input.Data);

            // Scale to fit inside the canvas (contain), preserving aspect ratio.
            double scale = Math.Min((double)canvasW / source.Width, (double)canvasH / source.Height);
            int fitW = Math.Max(1, (int)Math.Round(source.Width * scale));
            int fitH = Math.Max(1, (int)Math.Round(source.Height * scale));

            source.Mutate(c => c.Resize(fitW, fitH));

            int offsetX = (canvasW - fitW) / 2;
            int offsetY = (canvasH - fitH) / 2;

            using var canvas = new Image<Rgba32>(canvasW, canvasH, new Rgba32(0, 0, 0, 0));
            canvas.Mutate(c => c.DrawImage(source, new Point(offsetX, offsetY), 1f));

            EdgeFill(canvas, offsetX, offsetY, fitW, fitH);

            using var ms = new MemoryStream();
            canvas.SaveAsPng(ms);
            return ms.ToArray();
        }, ct);
    }

    /// <summary>
    /// Fills every pixel outside the placed-image rectangle with the nearest
    /// edge pixel of that rectangle (edge clamp), eliminating transparent gaps.
    /// </summary>
    private static void EdgeFill(Image<Rgba32> canvas, int offsetX, int offsetY, int fitW, int fitH)
    {
        int right = offsetX + fitW - 1;
        int bottom = offsetY + fitH - 1;

        canvas.ProcessPixelRows(accessor =>
        {
            for (int y = 0; y < accessor.Height; y++)
            {
                int sy = Math.Clamp(y, offsetY, bottom);
                Span<Rgba32> row = accessor.GetRowSpan(y);
                Span<Rgba32> srcRow = accessor.GetRowSpan(sy);

                for (int x = 0; x < row.Length; x++)
                {
                    bool inside = x >= offsetX && x <= right && y >= offsetY && y <= bottom;
                    if (inside) continue;

                    int sx = Math.Clamp(x, offsetX, right);
                    row[x] = srcRow[sx];
                }
            }
        });
    }
}
