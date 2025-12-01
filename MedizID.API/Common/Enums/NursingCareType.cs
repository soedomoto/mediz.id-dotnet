namespace MedizID.API.Common.Enums;

/// <summary>
/// Nursing care type (Tipe Asuhan Keperawatan)
/// </summary>
public enum NursingCareType
{
    /// <summary>Text-based documentation</summary>
    Text = 0,
    
    /// <summary>SOAP format (Subjective, Objective, Assessment, Plan)</summary>
    SOAP = 1
}
