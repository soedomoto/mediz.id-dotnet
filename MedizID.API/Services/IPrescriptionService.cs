using MedizID.API.DTOs;

namespace MedizID.API.Services;

/// <summary>
/// Service interface for prescription operations
/// </summary>
public interface IPrescriptionService
{
    /// <summary>
    /// Get prescription by ID
    /// </summary>
    Task<PrescriptionResponse?> GetPrescriptionAsync(Guid prescriptionId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get prescriptions for a medical record
    /// </summary>
    Task<IEnumerable<PrescriptionResponse>> GetMedicalRecordPrescriptionsAsync(Guid medicalRecordId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get active prescriptions for a patient
    /// </summary>
    Task<IEnumerable<PrescriptionResponse>> GetPatientActivePrescriptionsAsync(Guid patientId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get prescription history for a patient
    /// </summary>
    Task<PatientPrescriptionHistoryResponse> GetPatientPrescriptionHistoryAsync(Guid patientId, int limit = 20, CancellationToken cancellationToken = default);

    /// <summary>
    /// Create new prescription
    /// </summary>
    Task<PrescriptionResponse> CreatePrescriptionAsync(CreatePrescriptionRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update prescription
    /// </summary>
    Task<PrescriptionResponse> UpdatePrescriptionAsync(Guid prescriptionId, CreatePrescriptionRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Dispense prescription
    /// </summary>
    Task<bool> DispensePrescriptionAsync(Guid prescriptionId, DispensePrescriptionRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Check drug interactions
    /// </summary>
    Task<DrugInteractionCheckResponse> CheckDrugInteractionsAsync(CheckDrugInteractionRequest request, CancellationToken cancellationToken = default);
}
