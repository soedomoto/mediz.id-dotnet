namespace MedizID.API.Common.Enums;

/// <summary>
/// Triage level classification (Tingkat Triase)
/// </summary>
public enum TriageLevel
{
    /// <summary>Emergency - Life-threatening condition requiring immediate intervention</summary>
    LifeThreatening = 0,
    
    /// <summary>Urgent - Serious condition requiring prompt care</summary>
    Urgent = 1,
    
    /// <summary>Non-Emergency - Stable condition that can wait for care</summary>
    NonEmergency = 2,
    
    /// <summary>Deceased - Patient has passed away</summary>
    Deceased = 3
}
