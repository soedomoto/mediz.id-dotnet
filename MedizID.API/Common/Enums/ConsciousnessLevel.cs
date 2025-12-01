namespace MedizID.API.Common.Enums;

/// <summary>
/// Consciousness level assessment (Kesadaran)
/// </summary>
public enum ConsciousnessLevel
{
    /// <summary>Compos Mentis - Alert and oriented</summary>
    ComposMentis = 0,
    
    /// <summary>Somnolen - Drowsy, easily aroused</summary>
    Somnolen = 1,
    
    /// <summary>Sopor - Deep sleep, difficult to arouse</summary>
    Sopor = 2,
    
    /// <summary>Coma - Unconscious, cannot be aroused</summary>
    Coma = 3
}
