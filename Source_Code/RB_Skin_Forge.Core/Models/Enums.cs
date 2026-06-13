namespace RB_Skin_Forge.Core.Models;

/// <summary>Supported input asset categories.</summary>
public enum AssetType
{
    Unknown = 0,
    Image,   // PNG, JPG, ...
    Mesh     // OBJ, FBX, ...
}

/// <summary>Roblox character body parts the app can target.</summary>
public enum BodyPart
{
    Unknown = 0,
    Torso,
    LeftArm,
    RightArm,
    LeftLeg,
    RightLeg,
    Head,
    Accessory
}

/// <summary>Where an asset sits in the processing pipeline.</summary>
public enum TaskState
{
    Queued,      // "Standing in the queue"
    Processing,
    Synchronized,
    Failed
}

/// <summary>Severity for a validation finding.</summary>
public enum Severity
{
    Info,
    Warning,
    Error
}
