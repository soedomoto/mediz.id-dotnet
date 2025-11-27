using MedizID.API.Common.Exceptions;
using MedizID.API.Data;
using MedizID.API.DTOs;
using MedizID.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedizID.API.Controllers.FacilityManagement;

[ApiController]
[Route("api/v1/facilities/{facilityId}/patients")]
[Produces("application/json")]
[Authorize]
public class PatientsController : ControllerBase
{
    private readonly MedizIDDbContext _context;
    private readonly ILogger<PatientsController> _logger;

    public PatientsController(MedizIDDbContext context, ILogger<PatientsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Create facility patient
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(FacilityPatientResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateFacilityPatient(Guid facilityId, [FromBody] CreateFacilityPatientRequest request)
    {
        try
        {
            var facility = await _context.Facilities.FirstOrDefaultAsync(f => f.Id == facilityId);
            if (facility == null)
                throw new NotFoundException($"Facility with ID {facilityId} not found");

            var patient = new Patient
            {
                Id = Guid.NewGuid(),
                FacilityId = facilityId,
                MedicalRecordNumber = request.MedicalRecordNumber,
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

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Facility patient created: {patient.Id}");

            var response = new FacilityPatientResponse
            {
                Id = patient.Id,
                FacilityId = patient.FacilityId,
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
                CreatedAt = patient.CreatedAt,
                UpdatedAt = patient.UpdatedAt
            };

            return CreatedAtAction(nameof(GetFacilityPatients), new { facilityId }, response);
        }
        catch (NotFoundException ex)
        {
            return BadRequest(new ErrorResponse
            {
                ErrorCode = ex.ErrorCode,
                Message = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating facility patient");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while creating the patient"
            });
        }
    }

    /// <summary>
    /// Get facility patients
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<FacilityPatientResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetFacilityPatients(Guid facilityId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var facility = await _context.Facilities.FirstOrDefaultAsync(f => f.Id == facilityId);
            if (facility == null)
                throw new NotFoundException($"Facility with ID {facilityId} not found");

            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var query = _context.Patients
                .Where(p => p.FacilityId == facilityId)
                .AsQueryable();

            var total = await query.CountAsync();

            var patients = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new FacilityPatientResponse
                {
                    Id = p.Id,
                    FacilityId = p.FacilityId,
                    MedicalRecordNumber = p.MedicalRecordNumber,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    PhoneNumber = p.PhoneNumber,
                    Email = p.Email,
                    DateOfBirth = p.DateOfBirth,
                    Gender = p.Gender,
                    BloodType = p.BloodType,
                    Address = p.Address,
                    City = p.City,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                })
                .ToListAsync();

            return Ok(new PagedResult<FacilityPatientResponse>
            {
                Items = patients,
                Total = total,
                Page = page,
                PageSize = pageSize
            });
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ErrorResponse
            {
                ErrorCode = ex.ErrorCode,
                Message = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching facility patients {facilityId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching patients"
            });
        }
    }

    /// <summary>
    /// Get facility patient details
    /// </summary>
    [HttpGet("{patientId}/details")]
    [ProducesResponseType(typeof(FacilityPatientDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetFacilityPatientDetails(Guid facilityId, Guid patientId)
    {
        try
        {
            var facility = await _context.Facilities.FirstOrDefaultAsync(f => f.Id == facilityId);
            if (facility == null)
                throw new NotFoundException($"Facility with ID {facilityId} not found");

            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Id == patientId && p.FacilityId == facilityId);
            if (patient == null)
                throw new NotFoundException($"Patient with ID {patientId} not found");

            var totalAppointments = await _context.Appointments
                .CountAsync(a => a.PatientId == patientId);

            var totalMedicalRecords = await _context.MedicalRecords
                .CountAsync(m => m.PatientId == patientId);

            var lastVisitDate = await _context.MedicalRecords
                .Where(m => m.PatientId == patientId)
                .OrderByDescending(m => m.VisitDate)
                .Select(m => m.VisitDate)
                .FirstOrDefaultAsync();

            var response = new FacilityPatientDetailResponse
            {
                Id = patient.Id,
                FacilityId = patient.FacilityId,
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
                TotalAppointments = totalAppointments,
                TotalMedicalRecords = totalMedicalRecords,
                LastVisitDate = lastVisitDate != default ? lastVisitDate : null,
                CreatedAt = patient.CreatedAt,
                UpdatedAt = patient.UpdatedAt
            };

            return Ok(response);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ErrorResponse
            {
                ErrorCode = ex.ErrorCode,
                Message = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching facility patient details {patientId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching patient details"
            });
        }
    }
}
