using MedizID.API.DTOs;
using MedizID.API.Models;
using MedizID.API.Repositories;

namespace MedizID.API.Services;

/// <summary>
/// Service for patient operations with business logic
/// </summary>
public class PatientService : IPatientService
{
    private readonly IPatientRepository _patientRepository;
    private readonly IFacilityRepository _facilityRepository;
    private readonly ILogger<PatientService> _logger;

    public PatientService(
        IPatientRepository patientRepository,
        IFacilityRepository facilityRepository,
        ILogger<PatientService> logger)
    {
        _patientRepository = patientRepository;
        _facilityRepository = facilityRepository;
        _logger = logger;
    }

    public async Task<PatientDetail?> GetPatientAsync(Guid patientId, CancellationToken cancellationToken = default)
    {
        try
        {
            var patient = await _patientRepository.GetByIdAsync(patientId, cancellationToken);
            if (patient == null)
            {
                _logger.LogWarning("Patient not found: {PatientId}", patientId);
                return null;
            }

            return MapToPatientDetail(patient);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving patient: {PatientId}", patientId);
            throw;
        }
    }

    public async Task<IEnumerable<PatientDetail>> GetPatientsAsync(Guid facilityId, CancellationToken cancellationToken = default)
    {
        try
        {
            var patients = await _patientRepository.GetByFacilityAsync(facilityId, cancellationToken);
            return patients.Select(MapToPatientDetail).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving patients for facility: {FacilityId}", facilityId);
            throw;
        }
    }

    public async Task<PatientDetail?> GetPatientWithHistoryAsync(Guid patientId, CancellationToken cancellationToken = default)
    {
        try
        {
            var patient = await _patientRepository.GetWithMedicalRecordsAsync(patientId, cancellationToken);
            if (patient == null)
                return null;

            return MapToPatientDetail(patient);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving patient with history: {PatientId}", patientId);
            throw;
        }
    }

    public async Task<IEnumerable<PatientDetail>> SearchPatientsByNameAsync(string firstName, string lastName, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
                return Enumerable.Empty<PatientDetail>();

            var patients = await _patientRepository.SearchByNameAsync(firstName, lastName, cancellationToken);
            return patients.Select(MapToPatientDetail).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching patients by name");
            throw;
        }
    }

    public async Task<PatientDetail?> GetPatientByMRNAsync(string mrn, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(mrn))
                return null;

            var patient = await _patientRepository.GetByMedicalRecordNumberAsync(mrn, cancellationToken);
            if (patient == null)
            {
                _logger.LogWarning("Patient not found by MRN: {MRN}", mrn);
                return null;
            }

            return MapToPatientDetail(patient);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving patient by MRN: {MRN}", mrn);
            throw;
        }
    }

    public async Task<PatientDetail> CreatePatientAsync(CreatePatientRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            // Generate unique MRN
            var mrn = $"MRN{DateTime.UtcNow.Ticks}";

            // Check if MRN already exists (unlikely but safe)
            var existingPatient = await _patientRepository.GetByMedicalRecordNumberAsync(mrn, cancellationToken);
            if (existingPatient != null)
                throw new InvalidOperationException($"Medical record number already exists: {mrn}");

            var patient = new Patient
            {
                Id = Guid.NewGuid(),
                FacilityId = Guid.Empty, // Will be set from context or controller
                MedicalRecordNumber = mrn,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                DateOfBirth = request.DateOfBirth,
                Gender = request.Gender,
                BloodType = request.BloodType,
                Address = request.Address,
                City = request.City,
                CreatedAt = DateTime.UtcNow
            };

            var createdPatient = await _patientRepository.AddAsync(patient, cancellationToken);
            _logger.LogInformation("Patient created: {PatientId}", createdPatient.Id);

            return MapToPatientDetail(createdPatient);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating patient");
            throw;
        }
    }

    public async Task<PatientDetail> UpdatePatientAsync(Guid patientId, UpdatePatientRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var patient = await _patientRepository.GetByIdAsync(patientId, cancellationToken);
            if (patient == null)
                throw new InvalidOperationException($"Patient not found: {patientId}");

            patient.FirstName = request.FirstName ?? patient.FirstName;
            patient.LastName = request.LastName ?? patient.LastName;
            patient.PhoneNumber = request.PhoneNumber ?? patient.PhoneNumber;
            patient.Email = request.Email ?? patient.Email;
            patient.Address = request.Address ?? patient.Address;
            patient.City = request.City ?? patient.City;
            patient.UpdatedAt = DateTime.UtcNow;

            await _patientRepository.UpdateAsync(patient, cancellationToken);
            _logger.LogInformation("Patient updated: {PatientId}", patientId);

            return MapToPatientDetail(patient);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating patient: {PatientId}", patientId);
            throw;
        }
    }

    public async Task<PatientAllergiesResponse?> GetPatientAllergiesAsync(Guid patientId, CancellationToken cancellationToken = default)
    {
        try
        {
            // TODO: Update with FacilityPatient relationships
            return new PatientAllergiesResponse
            {
                PatientId = patientId,
                Allergies = new List<string>()
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving patient allergies: {PatientId}", patientId);
            throw;
        }
    }

    public async Task<IEnumerable<AppointmentResponse>> GetPatientAppointmentsAsync(Guid patientId, CancellationToken cancellationToken = default)
    {
        try
        {
            // TODO: Update with FacilityPatient relationships
            return Enumerable.Empty<AppointmentResponse>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving patient appointments: {PatientId}", patientId);
            throw;
        }
    }

    private PatientDetail MapToPatientDetail(Patient patient)
    {
        return new PatientDetail
        {
            Id = patient.Id,
            MedicalRecordNumber = patient.MedicalRecordNumber,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            PhoneNumber = patient.PhoneNumber,
            Email = patient.Email,
            DateOfBirth = patient.DateOfBirth,
            Gender = patient.Gender,
            BloodType = patient.BloodType,
            Address = patient.Address,
            City = patient.City,
            TotalAppointments = 0,
            TotalMedicalRecords = 0,
            CreatedAt = patient.CreatedAt,
            UpdatedAt = patient.UpdatedAt
        };
    }
}
