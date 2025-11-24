using MedizID.API.DTOs;
using MedizID.API.Repositories;

namespace MedizID.API.Services;

public class MedicalRecordService : IMedicalRecordService
{
    private readonly IMedicalRecordRepository _repository;
    private readonly ILogger<MedicalRecordService> _logger;

    public MedicalRecordService(IMedicalRecordRepository repository, ILogger<MedicalRecordService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<MedicalRecordDetail?> GetMedicalRecordAsync(Guid recordId, CancellationToken cancellationToken = default)
    {
        var record = await _repository.GetDetailedAsync(recordId, cancellationToken);
        return record == null ? null : MapToDetail(record);
    }

    public async Task<IEnumerable<MedicalRecordDetail>> GetPatientMedicalRecordsAsync(Guid patientId, CancellationToken cancellationToken = default)
    {
        var records = await _repository.GetByPatientAsync(patientId, cancellationToken);
        return records.Select(MapToDetail).ToList();
    }

    public async Task<IEnumerable<MedicalRecordDetail>> GetAppointmentMedicalRecordsAsync(Guid appointmentId, CancellationToken cancellationToken = default)
    {
        var records = await _repository.GetByAppointmentAsync(appointmentId, cancellationToken);
        return records.Select(MapToDetail).ToList();
    }

    public async Task<MedicalRecordDetail?> GetLatestMedicalRecordAsync(Guid patientId, CancellationToken cancellationToken = default)
    {
        var record = await _repository.GetLatestForPatientAsync(patientId, cancellationToken);
        return record == null ? null : MapToDetail(record);
    }

    public async Task<MedicalRecordDetail> CreateMedicalRecordAsync(CreateMedicalRecordRequest request, CancellationToken cancellationToken = default)
    {
        var record = new Models.MedicalRecord
        {
            Id = Guid.NewGuid(),
            PatientId = request.PatientId,
            AppointmentId = request.AppointmentId,
            VisitDate = request.VisitDate,
            ChiefComplaint = request.ChiefComplaint,
            Diagnosis = request.Diagnosis,
            Treatment = request.Treatment,
            Notes = request.Notes,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _repository.AddAsync(record, cancellationToken);
        return MapToDetail(created);
    }

    public async Task<MedicalRecordDetail> UpdateMedicalRecordAsync(Guid recordId, UpdateMedicalRecordRequest request, CancellationToken cancellationToken = default)
    {
        var record = await _repository.GetByIdAsync(recordId, cancellationToken);
        if (record == null) throw new InvalidOperationException($"Medical record not found: {recordId}");

        if (!string.IsNullOrEmpty(request.ChiefComplaint)) record.ChiefComplaint = request.ChiefComplaint;
        if (!string.IsNullOrEmpty(request.Diagnosis)) record.Diagnosis = request.Diagnosis;
        if (!string.IsNullOrEmpty(request.Treatment)) record.Treatment = request.Treatment;
        if (!string.IsNullOrEmpty(request.Notes)) record.Notes = request.Notes;
        record.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(record, cancellationToken);
        return MapToDetail(record);
    }

    public async Task<IEnumerable<MedicalRecordDetail>> GetMedicalRecordsByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        var records = await _repository.GetByDateRangeAsync(startDate, endDate, cancellationToken);
        return records.Select(MapToDetail).ToList();
    }

    private MedicalRecordDetail MapToDetail(Models.MedicalRecord record)
    {
        return new MedicalRecordDetail
        {
            Id = record.Id,
            PatientId = record.PatientId,
            PatientName = record.Patient != null ? $"{record.Patient.FirstName} {record.Patient.LastName}" : "",
            VisitDate = record.VisitDate,
            ChiefComplaint = record.ChiefComplaint,
            Diagnosis = record.Diagnosis,
            Treatment = record.Treatment,
            Diagnoses = record.Diagnoses.Select(d => new DiagnosisResponse
            {
                Id = d.Id,
                DiagnosisCode = d.DiagnosisCode,
                DiagnosisDescription = d.DiagnosisDescription,
                DiagnosisType = d.DiagnosisType,
                ConfidencePercentage = d.ConfidencePercentage,
                CreatedAt = d.CreatedAt
            }).ToList(),
            Prescriptions = record.Prescriptions.Select(p => new PrescriptionResponse
            {
                Id = p.Id,
                MedicationName = p.MedicationName,
                Dosage = p.Dosage,
                Frequency = p.Frequency,
                Duration = p.Duration,
                CreatedAt = p.CreatedAt
            }).ToList(),
            LaboratoriumTests = record.LaboratoriumTests.Select(l => new LaboratoriumResponse
            {
                Id = l.Id,
                TestName = l.TestName,
                Result = l.Result,
                Unit = l.Unit,
                Status = l.Status,
                TestDate = l.TestDate,
                CreatedAt = l.CreatedAt
            }).ToList(),
            CreatedAt = record.CreatedAt
        };
    }
}

public class DiagnosisService : IDiagnosisService
{
    private readonly IDiagnosisRepository _repository;
    private readonly ILogger<DiagnosisService> _logger;

    public DiagnosisService(IDiagnosisRepository repository, ILogger<DiagnosisService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<DiagnosisResponse?> GetDiagnosisAsync(Guid diagnosisId, CancellationToken cancellationToken = default)
    {
        var diagnosis = await _repository.GetByIdAsync(diagnosisId, cancellationToken);
        return diagnosis == null ? null : MapToResponse(diagnosis);
    }

    public async Task<IEnumerable<DiagnosisResponse>> GetMedicalRecordDiagnosesAsync(Guid medicalRecordId, CancellationToken cancellationToken = default)
    {
        var diagnoses = await _repository.GetByMedicalRecordAsync(medicalRecordId, cancellationToken);
        return diagnoses.Select(MapToResponse).ToList();
    }

    public async Task<IEnumerable<DiagnosisResponse>> GetPatientDiagnosisHistoryAsync(Guid patientId, int limit = 10, CancellationToken cancellationToken = default)
    {
        var diagnoses = await _repository.GetPatientDiagnosisHistoryAsync(patientId, limit, cancellationToken);
        return diagnoses.Select(MapToResponse).ToList();
    }

    public async Task<DiagnosisResponse> CreateDiagnosisAsync(CreateDiagnosisRequest request, CancellationToken cancellationToken = default)
    {
        var diagnosis = new Models.Diagnosis
        {
            Id = Guid.NewGuid(),
            MedicalRecordId = request.MedicalRecordId,
            DiagnosisCode = request.DiagnosisCode,
            DiagnosisDescription = request.DiagnosisDescription,
            DiagnosisType = request.DiagnosisType,
            ConfidencePercentage = request.ConfidencePercentage,
            Reason = request.Reason,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _repository.AddAsync(diagnosis, cancellationToken);
        return MapToResponse(created);
    }

    public async Task<DiagnosisResponse> UpdateDiagnosisAsync(Guid diagnosisId, CreateDiagnosisRequest request, CancellationToken cancellationToken = default)
    {
        var diagnosis = await _repository.GetByIdAsync(diagnosisId, cancellationToken);
        if (diagnosis == null) throw new InvalidOperationException($"Diagnosis not found: {diagnosisId}");

        diagnosis.DiagnosisDescription = request.DiagnosisDescription;
        diagnosis.DiagnosisType = request.DiagnosisType;
        diagnosis.ConfidencePercentage = request.ConfidencePercentage;

        await _repository.UpdateAsync(diagnosis, cancellationToken);
        return MapToResponse(diagnosis);
    }

    public async Task<bool> DeleteDiagnosisAsync(Guid diagnosisId, CancellationToken cancellationToken = default)
    {
        return await _repository.DeleteAsync(diagnosisId, cancellationToken);
    }

    public async Task<DiagnosisSearchResponse> SearchDiagnosesAsync(DiagnosisSearchRequest request, CancellationToken cancellationToken = default)
    {
        var diagnoses = await _repository.GetAllAsync(cancellationToken);
        var filtered = diagnoses.Where(d => d.DiagnosisCode.Contains(request.Query ?? "")).ToList();

        return new DiagnosisSearchResponse
        {
            Results = filtered.Select(d => new ICD10CodeResponse
            {
                Id = d.Id,
                Code = d.DiagnosisCode,
                Description = d.DiagnosisDescription,
                CreatedAt = d.CreatedAt
            }).ToList(),
            Total = filtered.Count(),
            Page = request.Page,
            PageSize = request.PageSize
        };
    }

    private DiagnosisResponse MapToResponse(Models.Diagnosis diagnosis)
    {
        return new DiagnosisResponse
        {
            Id = diagnosis.Id,
            DiagnosisCode = diagnosis.DiagnosisCode,
            DiagnosisDescription = diagnosis.DiagnosisDescription,
            DiagnosisType = diagnosis.DiagnosisType,
            ConfidencePercentage = diagnosis.ConfidencePercentage,
            CreatedAt = diagnosis.CreatedAt
        };
    }
}

public class PrescriptionService : IPrescriptionService
{
    private readonly IPrescriptionRepository _repository;
    private readonly ILogger<PrescriptionService> _logger;

    public PrescriptionService(IPrescriptionRepository repository, ILogger<PrescriptionService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<PrescriptionResponse?> GetPrescriptionAsync(Guid prescriptionId, CancellationToken cancellationToken = default)
    {
        var prescription = await _repository.GetDetailedAsync(prescriptionId, cancellationToken);
        return prescription == null ? null : MapToResponse(prescription);
    }

    public async Task<IEnumerable<PrescriptionResponse>> GetMedicalRecordPrescriptionsAsync(Guid medicalRecordId, CancellationToken cancellationToken = default)
    {
        var prescriptions = await _repository.GetByMedicalRecordAsync(medicalRecordId, cancellationToken);
        return prescriptions.Select(MapToResponse).ToList();
    }

    public async Task<IEnumerable<PrescriptionResponse>> GetPatientActivePrescriptionsAsync(Guid patientId, CancellationToken cancellationToken = default)
    {
        var prescriptions = await _repository.GetActivePrescriptionsAsync(patientId, cancellationToken);
        return prescriptions.Select(MapToResponse).ToList();
    }

    public async Task<PatientPrescriptionHistoryResponse> GetPatientPrescriptionHistoryAsync(Guid patientId, int limit = 20, CancellationToken cancellationToken = default)
    {
        var prescriptions = await _repository.GetPatientPrescriptionHistoryAsync(patientId, limit, cancellationToken);

        return new PatientPrescriptionHistoryResponse
        {
            PatientId = patientId,
            Prescriptions = prescriptions.Select(p => new PrescriptionHistoryItem
            {
                Id = p.Id,
                MedicationName = p.MedicationName,
                Dosage = p.Dosage,
                Frequency = p.Frequency,
                PrescribedDate = p.CreatedAt,
                DurationDays = p.Duration
            }).ToList()
        };
    }

    public async Task<PrescriptionResponse> CreatePrescriptionAsync(CreatePrescriptionRequest request, CancellationToken cancellationToken = default)
    {
        var prescription = new Models.Prescription
        {
            Id = Guid.NewGuid(),
            MedicalRecordId = request.MedicalRecordId,
            MedicationName = request.MedicationName,
            Dosage = request.Dosage,
            Frequency = request.Frequency,
            Duration = request.Duration,
            Instructions = request.Instructions,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _repository.AddAsync(prescription, cancellationToken);
        return MapToResponse(created);
    }

    public async Task<PrescriptionResponse> UpdatePrescriptionAsync(Guid prescriptionId, CreatePrescriptionRequest request, CancellationToken cancellationToken = default)
    {
        var prescription = await _repository.GetByIdAsync(prescriptionId, cancellationToken);
        if (prescription == null) throw new InvalidOperationException($"Prescription not found: {prescriptionId}");

        prescription.MedicationName = request.MedicationName;
        prescription.Dosage = request.Dosage;
        prescription.Frequency = request.Frequency;
        prescription.Duration = request.Duration;
        prescription.Instructions = request.Instructions;

        await _repository.UpdateAsync(prescription, cancellationToken);
        return MapToResponse(prescription);
    }

    public async Task<bool> DispensePrescriptionAsync(Guid prescriptionId, DispensePrescriptionRequest request, CancellationToken cancellationToken = default)
    {
        var prescription = await _repository.GetByIdAsync(prescriptionId, cancellationToken);
        if (prescription == null) return false;

        prescription.ExpiryDate = request.DispenseDate.AddDays(prescription.Duration);
        await _repository.UpdateAsync(prescription, cancellationToken);
        return true;
    }

    public async Task<DrugInteractionCheckResponse> CheckDrugInteractionsAsync(CheckDrugInteractionRequest request, CancellationToken cancellationToken = default)
    {
        return new DrugInteractionCheckResponse
        {
            Interactions = new List<DrugInteractionDetail>(),
            HasInteractions = false
        };
    }

    private PrescriptionResponse MapToResponse(Models.Prescription prescription)
    {
        return new PrescriptionResponse
        {
            Id = prescription.Id,
            MedicationName = prescription.MedicationName,
            Dosage = prescription.Dosage,
            Frequency = prescription.Frequency,
            Duration = prescription.Duration,
            CreatedAt = prescription.CreatedAt
        };
    }
}
