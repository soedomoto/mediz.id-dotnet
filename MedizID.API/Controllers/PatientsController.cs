using MedizID.API.Common.Exceptions;
using MedizID.API.Data;
using MedizID.API.DTOs;
using MedizID.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedizID.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
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
    /// Get all patients with pagination
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<PatientResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPatients([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var query = _context.Patients.AsQueryable();
            var total = await query.CountAsync();

            var patients = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new PatientResponse
                {
                    Id = p.Id,
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

            return Ok(new PagedResult<PatientResponse>
            {
                Items = patients,
                Total = total,
                Page = page,
                PageSize = pageSize
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching patients");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching patients"
            });
        }
    }

    /// <summary>
    /// Get patient by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PatientResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPatient(Guid id)
    {
        try
        {
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Id == id);

            if (patient == null)
            {
                _logger.LogWarning($"Patient not found: {id}");
                throw new NotFoundException($"Patient with ID {id} not found");
            }

            var response = new PatientResponse
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
            _logger.LogError(ex, $"Error fetching patient {id}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching the patient"
            });
        }
    }

    /// <summary>
    /// Create a new patient
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(PatientResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreatePatient([FromBody] CreatePatientRequest request)
    {
        try
        {
            // Get facility from context (in production, from user claims or parameter)
            var facilityId = Guid.NewGuid(); // Placeholder - should come from user context

            var patient = new Patient
            {
                Id = Guid.NewGuid(),
                FacilityId = facilityId,
                MedicalRecordNumber = GenerateMRN(),
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

            _logger.LogInformation($"Patient created: {patient.Id}");

            var response = new PatientResponse
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
                CreatedAt = patient.CreatedAt
            };

            return CreatedAtAction(nameof(GetPatient), new { id = patient.Id }, response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating patient");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while creating the patient"
            });
        }
    }

    /// <summary>
    /// Update patient
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(PatientResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdatePatient(Guid id, [FromBody] UpdatePatientRequest request)
    {
        try
        {
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Id == id);

            if (patient == null)
            {
                throw new NotFoundException($"Patient with ID {id} not found");
            }

            if (!string.IsNullOrEmpty(request.FirstName))
                patient.FirstName = request.FirstName;

            if (!string.IsNullOrEmpty(request.LastName))
                patient.LastName = request.LastName;

            if (!string.IsNullOrEmpty(request.PhoneNumber))
                patient.PhoneNumber = request.PhoneNumber;

            if (!string.IsNullOrEmpty(request.Email))
                patient.Email = request.Email;

            if (!string.IsNullOrEmpty(request.Gender))
                patient.Gender = request.Gender;

            if (!string.IsNullOrEmpty(request.BloodType))
                patient.BloodType = request.BloodType;

            if (!string.IsNullOrEmpty(request.Address))
                patient.Address = request.Address;

            if (!string.IsNullOrEmpty(request.City))
                patient.City = request.City;

            patient.UpdatedAt = DateTime.UtcNow;

            _context.Patients.Update(patient);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Patient updated: {patient.Id}");

            var response = new PatientResponse
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
            _logger.LogError(ex, $"Error updating patient {id}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while updating the patient"
            });
        }
    }

    /// <summary>
    /// Delete patient
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeletePatient(Guid id)
    {
        try
        {
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Id == id);

            if (patient == null)
            {
                throw new NotFoundException($"Patient with ID {id} not found");
            }

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Patient deleted: {id}");

            return NoContent();
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
            _logger.LogError(ex, $"Error deleting patient {id}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while deleting the patient"
            });
        }
    }

    private string GenerateMRN()
    {
        // Generate Medical Record Number: MRN-YYYYMMDD-XXXXX
        var timestamp = DateTime.UtcNow.ToString("yyyyMMdd");
        var random = new Random().Next(10000, 99999);
        return $"MRN-{timestamp}-{random}";
    }
}
