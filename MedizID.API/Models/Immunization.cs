namespace MedizID.API.Models;

using MedizID.API.Common.Enums;

public class Immunization
{
    public Guid Id { get; set; }
    public Guid AppointmentId { get; set; }
    
    // Patient Demographics (for child immunization)
    public DateTime? DateOfBirth { get; set; }
    public string? BirthPlace { get; set; }
    public decimal? BirthWeight { get; set; }
    public decimal? BirthLength { get; set; }
    public string? FatherName { get; set; }
    public string? FatherOccupation { get; set; }
    public string? MotherName { get; set; }
    public string? MotherOccupation { get; set; }
    
    // Delivery Information
    public DeliveryType? DeliveryType { get; set; }
    public DeliveryPersonnel? DeliveryPersonnel { get; set; }
    public DeliveryPlace? DeliveryPlace { get; set; }
    public int? ChildNumber { get; set; }
    
    // Neonatal Data
    public string? NeonatalVisit1 { get; set; } // Lahir - 5 Jam
    public string? NeonatalVisit2 { get; set; } // 6 - 48 Jam
    public string? NeonatalVisit3 { get; set; } // Hari ke 3-7
    public string? NeonatalVisit4 { get; set; } // Hari ke 8-28
    
    // Feeding and Supplementation
    public BreastfeedingStatus? BreastfeedingStatus { get; set; }
    public VitaminAStatus? VitaminAStatusSixMonths { get; set; }
    
    // Vaccine Information
    public VaccineType? VaccineType { get; set; }
    public string? VaccineName { get; set; } = null!;
    public DateTime VaccineDate { get; set; }
    public int? DoseNumber { get; set; }
    public string? Lot { get; set; }
    public VaccineRoute? Route { get; set; }
    public VaccineSite? Site { get; set; }
    public string? Reactions { get; set; }
    public VaccineReactionSeverity? ReactionSeverity { get; set; }
    
    // Age Category
    public ImmunizationAgeCategory? AgeCategory { get; set; }
    
    // Medical Personnel
    public Guid? ProviderId { get; set; }
    public Guid? NurseId { get; set; }
    
    // Timestamps
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Relationships
    public Appointment Appointment { get; set; } = null!;
}
