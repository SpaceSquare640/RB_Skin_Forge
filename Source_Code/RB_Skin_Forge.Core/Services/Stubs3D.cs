using RB_Skin_Forge.Core.Abstractions;
using RB_Skin_Forge.Core.Models;

namespace RB_Skin_Forge.Core.Services;

// Geometry (OBJ) is implemented in GeometryEngine as of Phase 2.
// Auto-rigging remains a Phase 3 feature and is stubbed here.

/// <summary>Placeholder auto-rigger. Implemented in Phase 3.</summary>
public sealed class AutoRiggerStub : IAutoRigger
{
    public Task<ProcessingResult> RigAsync(AssetInput meshInput, CancellationToken ct = default)
    {
        var result = new ProcessingResult { SourceFileName = meshInput.FileName, State = TaskState.Failed };
        result.Log.Add("Auto-rigging is not implemented yet (planned for Phase 3).");
        return Task.FromResult(result);
    }
}
