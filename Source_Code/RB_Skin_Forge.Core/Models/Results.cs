namespace RB_Skin_Forge.Core.Models;

/// <summary>A single validation finding against Roblox specifications.</summary>
public sealed record ValidationFinding(Severity Severity, string Message);

/// <summary>Result of validating an asset against Roblox specs.</summary>
public sealed class ValidationResult
{
    public List<ValidationFinding> Findings { get; } = new();

    public bool HasErrors => Findings.Any(f => f.Severity == Severity.Error);

    public void Add(Severity severity, string message) =>
        Findings.Add(new ValidationFinding(severity, message));
}

/// <summary>
/// Quality metrics shown on the Console &amp; Stats panel
/// (triangle count, material/texture size, etc.).
/// </summary>
public sealed class QualityReport
{
    public int TriangleCount { get; set; }
    public long TextureSizeBytes { get; set; }
    public int OutputWidth { get; set; }
    public int OutputHeight { get; set; }
    public ValidationResult Validation { get; set; } = new();

    // --- Mesh metrics (Phase 2/3) ------------------------------------------
    public int VertexCount { get; set; }
    public int MaterialCount { get; set; }
    public int AttachmentCount { get; set; }

    /// <summary>Triangle count before decimation (equals <see cref="TriangleCount"/> if none ran).</summary>
    public int OriginalTriangleCount { get; set; }
    public bool WasDecimated { get; set; }

    /// <summary>Final bounding-box size in studs (X, Y, Z). Zero for 2D assets.</summary>
    public float BoundsX { get; set; }
    public float BoundsY { get; set; }
    public float BoundsZ { get; set; }
}

/// <summary>Outcome of running one asset through the pipeline.</summary>
public sealed class ProcessingResult
{
    public required string SourceFileName { get; init; }
    public TaskState State { get; set; } = TaskState.Queued;

    /// <summary>Downloadable output bytes (a template PNG for 2D, a cleaned OBJ for 3D).</summary>
    public byte[]? Output { get; set; }

    /// <summary>Suggested download file name for <see cref="Output"/>.</summary>
    public string? OutputFileName { get; set; }

    /// <summary>MIME type for <see cref="Output"/> (used by the web download helper).</summary>
    public string OutputContentType { get; set; } = "application/octet-stream";

    /// <summary>
    /// PNG to show in the Preview panel. For images this equals <see cref="Output"/>;
    /// for meshes it is a wireframe render (the deliverable is the OBJ in Output).
    /// </summary>
    public byte[]? PreviewImage { get; set; }

    public QualityReport Report { get; set; } = new();

    /// <summary>Human-readable log lines for the console panel.</summary>
    public List<string> Log { get; } = new();
}
