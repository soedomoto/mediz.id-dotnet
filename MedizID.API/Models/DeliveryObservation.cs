namespace MedizID.API.Models;

/// <summary>
/// DeliveryObservation represents clinical observations and outcomes related to childbirth/delivery.
/// This entity captures comprehensive delivery information including maternal/neonatal status,
/// delivery method, complications, and referral information.
/// </summary>
public class DeliveryObservation
{
    /// <summary>Gets or sets the unique identifier for the delivery observation.</summary>
    public Guid Id { get; set; }

    /// <summary>Gets or sets the appointment identifier (foreign key).</summary>
    public Guid AppointmentId { get; set; }

    // Pregnancy Timeline
    /// <summary>Gets or sets the gestational age at delivery (in weeks).</summary>
    public int? GestationalAge { get; set; }

    /// <summary>Gets or sets the gestational age from last menstrual period (in weeks).</summary>
    public int? GestationalAgeFromLmp { get; set; }

    // Maternal Status
    /// <summary>Gets or sets the maternal condition at delivery (e.g., "Alive", "Deceased").</summary>
    public string? MaternalCondition { get; set; }

    /// <summary>Gets or sets the maternal condition at discharge (e.g., "Good", "Fair", "Poor").</summary>
    public string? MaternalDischargingCondition { get; set; }

    // Neonatal Status
    /// <summary>Gets or sets the neonatal condition at birth (e.g., "Alive", "Stillborn", "Macerated").</summary>
    public string? NeonatalCondition { get; set; }

    /// <summary>Gets or sets the neonatal birth weight (in grams).</summary>
    public double? NeonatalWeight { get; set; }

    // Delivery Characteristics
    /// <summary>Gets or sets the fetal presentation at delivery (e.g., "Vertex", "Breech", "Transverse").</summary>
    public string? Presentation { get; set; }

    /// <summary>Gets or sets the delivery location (e.g., "Home", "Clinic", "Hospital").</summary>
    public string? DeliveryLocation { get; set; }

    /// <summary>Gets or sets the primary birth attendant (e.g., "Midwife", "Doctor", "Family").</summary>
    public string? BirthAttendant { get; set; }

    /// <summary>Gets or sets the mode of delivery (e.g., "Spontaneous Vaginal", "Assisted", "Cesarean Section").</summary>
    public string? DeliveryMode { get; set; }

    // Active Management Third Stage
    /// <summary>Gets or sets whether oxytocin was administered during third stage.</summary>
    public bool? OxytocinAdministered { get; set; }

    /// <summary>Gets or sets whether controlled cord traction was performed.</summary>
    public bool? ControlledCordTraction { get; set; }

    /// <summary>Gets or sets whether uterine massage was performed.</summary>
    public bool? UterineMassage { get; set; }

    // Maternal Services
    /// <summary>Gets or sets whether blood transfusion was provided.</summary>
    public bool? BloodTransfusion { get; set; }

    /// <summary>Gets or sets whether antibiotic therapy was provided.</summary>
    public bool? AntibioticTherapy { get; set; }

    /// <summary>Gets or sets whether neonatal resuscitation was performed.</summary>
    public bool? NeonatalResuscitation { get; set; }

    // Program Integration
    /// <summary>Gets or sets the program integration status (e.g., "Integrated", "Non-integrated", "Partial").</summary>
    public string? ProgramIntegration { get; set; }

    /// <summary>Gets or sets the antiretroviral prophylaxis medication given to newborn (if applicable).</summary>
    public string? AntiretroviralProphylaxis { get; set; }

    // Complications and Outcomes
    /// <summary>Gets or sets whether maternal complications occurred during delivery.</summary>
    public bool? HasComplications { get; set; }

    /// <summary>Gets or sets the type/description of maternal complications if any.</summary>
    public string? ComplicationsDescription { get; set; }

    /// <summary>Gets or sets whether referral was necessary after delivery.</summary>
    public bool? WasReferred { get; set; }

    /// <summary>Gets or sets the referral destination (e.g., "Hospital", "Clinic", "Higher Level Facility").</summary>
    public string? ReferralDestination { get; set; }

    /// <summary>Gets or sets the maternal condition at referral (e.g., "Good", "Fair", "Critical").</summary>
    public string? MaternalConditionAtReferral { get; set; }

    /// <summary>Gets or sets the delivery address/location details.</summary>
    public string? DeliveryAddress { get; set; }

    // Audit Fields
    /// <summary>Gets or sets the timestamp when the record was created.</summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>Gets or sets the timestamp when the record was last updated.</summary>
    public DateTimeOffset? UpdatedAt { get; set; }

    // Navigation Properties
    /// <summary>Gets or sets the navigation property to the associated appointment.</summary>
    public Appointment? Appointment { get; set; }
}
