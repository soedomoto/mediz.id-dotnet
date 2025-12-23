namespace MedizID.API.DTOs;

using MedizID.API.Common.Enums;

public class CreateImmunizationRequest
{
    public Guid AppointmentId { get; set; }
    
    // Patient Demographics (for child immunization)
    public DateTime? DateOfBirth { get; set; }
    public string? BirthPlace { get; set; }
    public int? Gender { get; set; }
    public string? Address { get; set; }
    public decimal? BirthWeight { get; set; }
    public decimal? BirthLength { get; set; }
    public string? FatherName { get; set; }
    public string? FatherOccupation { get; set; }
    public string? MotherName { get; set; }
    public string? MotherOccupation { get; set; }
    
    // Delivery Information
    public int? DeliveryType { get; set; }
    public int? DeliveryPersonnel { get; set; }
    public int? DeliveryPlace { get; set; }
    public int? ChildNumber { get; set; }
    
    // Neonatal Data
    public string? NeonatalVisit1 { get; set; }
    public string? NeonatalVisit2 { get; set; }
    public string? NeonatalVisit3 { get; set; }
    public string? NeonatalVisit4 { get; set; }
    
    // Feeding and Supplementation
    public int? BreastfeedingStatus { get; set; }
    public int? VitaminAStatusSixMonths { get; set; }
    
    // Vaccine Information
    public int? VaccineType { get; set; }
    public string? VaccineName { get; set; }
    public DateTime? VaccineDate { get; set; }
    public int? DoseNumber { get; set; }
    public string? Lot { get; set; }
    public int? Route { get; set; }
    public int? Site { get; set; }
    public string? Reactions { get; set; }
    public int? ReactionSeverity { get; set; }
    
    // Age Category
    public int? AgeCategory { get; set; }
    
    // Medical Personnel (for adult immunization)
    public DateTime? ServiceDate { get; set; }
    public int? AgeYears { get; set; }
    public string? DoctorName { get; set; }
    public string? NurseName { get; set; }
    
    // For linking to actual staff
    public Guid? ProviderId { get; set; }
    public Guid? NurseId { get; set; }
}

public class UpdateImmunizationRequest
{
    // Patient Demographics (for child immunization)
    public DateTime? DateOfBirth { get; set; }
    public string? BirthPlace { get; set; }
    public int? Gender { get; set; }
    public string? Address { get; set; }
    public decimal? BirthWeight { get; set; }
    public decimal? BirthLength { get; set; }
    public string? FatherName { get; set; }
    public string? FatherOccupation { get; set; }
    public string? MotherName { get; set; }
    public string? MotherOccupation { get; set; }
    
    // Delivery Information
    public int? DeliveryType { get; set; }
    public int? DeliveryPersonnel { get; set; }
    public int? DeliveryPlace { get; set; }
    public int? ChildNumber { get; set; }
    
    // Neonatal Data
    public string? NeonatalVisit1 { get; set; }
    public string? NeonatalVisit2 { get; set; }
    public string? NeonatalVisit3 { get; set; }
    public string? NeonatalVisit4 { get; set; }
    
    // Feeding and Supplementation
    public int? BreastfeedingStatus { get; set; }
    public int? VitaminAStatusSixMonths { get; set; }
    
    // Vaccine Information
    public int? VaccineType { get; set; }
    public string? VaccineName { get; set; }
    public DateTime? VaccineDate { get; set; }
    public int? DoseNumber { get; set; }
    public string? Lot { get; set; }
    public int? Route { get; set; }
    public int? Site { get; set; }
    public string? Reactions { get; set; }
    public int? ReactionSeverity { get; set; }
    
    // Age Category
    public int? AgeCategory { get; set; }
    
    // Medical Personnel (for adult immunization)
    public DateTime? ServiceDate { get; set; }
    public int? AgeYears { get; set; }
    public string? DoctorName { get; set; }
    public string? NurseName { get; set; }
    
    // For linking to actual staff
    public Guid? ProviderId { get; set; }
    public Guid? NurseId { get; set; }
}

public class ImmunizationResponse
{
    public Guid Id { get; set; }
    public Guid AppointmentId { get; set; }
    
    // Patient Demographics
    public DateTime? DateOfBirth { get; set; }
    public string? BirthPlace { get; set; }
    public int? Gender { get; set; }
    public string? Address { get; set; }
    public decimal? BirthWeight { get; set; }
    public decimal? BirthLength { get; set; }
    public string? FatherName { get; set; }
    public string? FatherOccupation { get; set; }
    public string? MotherName { get; set; }
    public string? MotherOccupation { get; set; }
    
    // Delivery Information
    public int? DeliveryType { get; set; }
    public string? DeliveryTypeLabel { get; set; }
    public int? DeliveryPersonnel { get; set; }
    public string? DeliveryPersonnelLabel { get; set; }
    public int? DeliveryPlace { get; set; }
    public string? DeliveryPlaceLabel { get; set; }
    public int? ChildNumber { get; set; }
    
    // Neonatal Data
    public string? NeonatalVisit1 { get; set; }
    public string? NeonatalVisit2 { get; set; }
    public string? NeonatalVisit3 { get; set; }
    public string? NeonatalVisit4 { get; set; }
    
    // Feeding and Supplementation
    public int? BreastfeedingStatus { get; set; }
    public string? BreastfeedingStatusLabel { get; set; }
    public int? VitaminAStatusSixMonths { get; set; }
    public string? VitaminAStatusLabel { get; set; }
    
    // Vaccine Information
    public int? VaccineType { get; set; }
    public string? VaccineTypeLabel { get; set; }
    public string? VaccineName { get; set; }
    public DateTime? VaccineDate { get; set; }
    public int? DoseNumber { get; set; }
    public string? Lot { get; set; }
    public int? Route { get; set; }
    public string? RouteLabel { get; set; }
    public int? Site { get; set; }
    public string? SiteLabel { get; set; }
    public string? Reactions { get; set; }
    public int? ReactionSeverity { get; set; }
    public string? ReactionSeverityLabel { get; set; }
    
    // Age Category
    public int? AgeCategory { get; set; }
    public string? AgeCategoryLabel { get; set; }
    
    // Medical Personnel (for adult immunization)
    public DateTime? ServiceDate { get; set; }
    public int? AgeYears { get; set; }
    public string? DoctorName { get; set; }
    public string? NurseName { get; set; }
    public Guid? ProviderId { get; set; }
    public Guid? NurseId { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
