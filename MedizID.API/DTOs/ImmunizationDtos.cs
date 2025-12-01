namespace MedizID.API.DTOs;

public class CreateImmunizationRequest
{
    public Guid AppointmentId { get; set; }
    public string VaccineName { get; set; } = null!;
    public DateTime VaccineDate { get; set; }
    public int? DoseNumber { get; set; }
    public string? Lot { get; set; }
    public string? Route { get; set; }
    public string? Site { get; set; }
    public string? Reactions { get; set; }
}

public class ImmunizationResponse
{
    public Guid Id { get; set; }
    public string VaccineName { get; set; } = null!;
    public DateTime VaccineDate { get; set; }
    public int? DoseNumber { get; set; }
    public string? Lot { get; set; }
    public DateTime CreatedAt { get; set; }
}
