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
    /// Add patient to facility
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(FacilityPatientResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddPatientToFacility(Guid facilityId, [FromBody] CreateFacilityPatientRequest request)
    {
        try
        {
            var facility = await _context.Facilities.FirstOrDefaultAsync(f => f.Id == facilityId);
            if (facility == null)
                throw new NotFoundException($"Facility with ID {facilityId} not found");

            var patient = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.PatientId);
            if (patient == null)
                throw new NotFoundException($"Patient with ID {request.PatientId} not found");

            // Generate MRN
            var mrn = $"MRN{DateTime.UtcNow.Ticks}";

            var facilityPatient = new FacilityPatient
            {
                Id = Guid.NewGuid(),
                FacilityId = facilityId,
                PatientId = request.PatientId,
                MedicalRecordNumber = mrn,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.FacilityPatients.Add(facilityPatient);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Patient added to facility: {facilityPatient.Id}");

            var response = new FacilityPatientResponse
            {
                Id = facilityPatient.Id,
                FacilityId = facilityPatient.FacilityId,
                PatientId = facilityPatient.PatientId,
                PatientName = $"{patient.FirstName} {patient.LastName}",
                MedicalRecordNumber = facilityPatient.MedicalRecordNumber,
                IsActive = facilityPatient.IsActive,
                CreatedAt = facilityPatient.CreatedAt,
                UpdatedAt = facilityPatient.UpdatedAt
            };

            return CreatedAtAction(nameof(ListPatients), new { facilityId }, response);
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
            _logger.LogError(ex, "Error adding patient to facility");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while adding patient"
            });
        }
    }

    /// <summary>
    /// List patients for facility
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<FacilityPatientResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ListPatients(Guid facilityId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var facility = await _context.Facilities.FirstOrDefaultAsync(f => f.Id == facilityId);
            if (facility == null)
                throw new NotFoundException($"Facility with ID {facilityId} not found");

            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var query = _context.FacilityPatients
                .Where(fp => fp.FacilityId == facilityId && fp.IsActive)
                .AsQueryable();

            var total = await query.CountAsync();

            var patients = await query
                .Include(fp => fp.Patient)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(fp => new FacilityPatientResponse
                {
                    Id = fp.Id,
                    FacilityId = fp.FacilityId,
                    PatientId = fp.PatientId,
                    PatientName = fp.Patient.FirstName + " " + fp.Patient.LastName,
                    MedicalRecordNumber = fp.MedicalRecordNumber,
                    IsActive = fp.IsActive,
                    CreatedAt = fp.CreatedAt,
                    UpdatedAt = fp.UpdatedAt
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
    /// Update facility patient
    /// </summary>
    [HttpPut("{patientId}")]
    [ProducesResponseType(typeof(FacilityPatientResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdatePatient(Guid facilityId, Guid patientId, [FromBody] UpdateFacilityPatientRequest request)
    {
        try
        {
            var facility = await _context.Facilities.FirstOrDefaultAsync(f => f.Id == facilityId);
            if (facility == null)
                throw new NotFoundException($"Facility with ID {facilityId} not found");

            var facilityPatient = await _context.FacilityPatients
                .Include(fp => fp.Patient)
                .FirstOrDefaultAsync(fp => fp.Id == patientId && fp.FacilityId == facilityId);

            if (facilityPatient == null)
                throw new NotFoundException($"Patient with ID {patientId} not found");

            if (request.IsActive.HasValue)
                facilityPatient.IsActive = request.IsActive.Value;

            facilityPatient.UpdatedAt = DateTime.UtcNow;

            _context.FacilityPatients.Update(facilityPatient);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Facility patient updated: {patientId}");

            var response = new FacilityPatientResponse
            {
                Id = facilityPatient.Id,
                FacilityId = facilityPatient.FacilityId,
                PatientId = facilityPatient.PatientId,
                PatientName = $"{facilityPatient.Patient.FirstName} {facilityPatient.Patient.LastName}",
                MedicalRecordNumber = facilityPatient.MedicalRecordNumber,
                IsActive = facilityPatient.IsActive,
                CreatedAt = facilityPatient.CreatedAt,
                UpdatedAt = facilityPatient.UpdatedAt
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
            _logger.LogError(ex, $"Error updating patient {patientId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while updating patient"
            });
        }
    }

    /// <summary>
    /// Delete facility patient (soft delete)
    /// </summary>
    [HttpDelete("{patientId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeletePatient(Guid facilityId, Guid patientId)
    {
        try
        {
            var facility = await _context.Facilities.FirstOrDefaultAsync(f => f.Id == facilityId);
            if (facility == null)
                throw new NotFoundException($"Facility with ID {facilityId} not found");

            var facilityPatient = await _context.FacilityPatients
                .FirstOrDefaultAsync(fp => fp.Id == patientId && fp.FacilityId == facilityId);

            if (facilityPatient == null)
                throw new NotFoundException($"Patient with ID {patientId} not found");

            facilityPatient.IsActive = false;
            facilityPatient.UpdatedAt = DateTime.UtcNow;

            _context.FacilityPatients.Update(facilityPatient);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Facility patient deleted: {patientId}");

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
            _logger.LogError(ex, $"Error deleting patient {patientId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while deleting patient"
            });
        }
    }

    /// <summary>
    /// Search patients by keyword (matches email, ID, first name, last name)
    /// </summary>
    [HttpGet("search")]
    [ProducesResponseType(typeof(PagedResult<FacilityPatientResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SearchPatients(
        Guid facilityId,
        [FromQuery] string keyword,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10
    )
    {
        try
        {
            if (string.IsNullOrWhiteSpace(keyword))
                throw new ArgumentException("Keyword parameter is required");

            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var facility = await _context.Facilities.FirstOrDefaultAsync(f => f.Id == facilityId);
            if (facility == null)
                throw new NotFoundException($"Facility with ID {facilityId} not found");

            // Normalize keyword for case-insensitive search
            var normalizedKeyword = keyword.ToLower().Trim();

            var query = _context.FacilityPatients
                .Where(fp => fp.FacilityId == facilityId && fp.IsActive &&
                    (fp.Id.ToString().ToLower().Contains(normalizedKeyword) ||
                     (fp.Patient.Email != null && fp.Patient.Email.ToLower().Contains(normalizedKeyword)) ||
                     fp.Patient.FirstName.ToLower().Contains(normalizedKeyword) ||
                     fp.Patient.LastName.ToLower().Contains(normalizedKeyword)))
                .AsQueryable();

            var total = await query.CountAsync();

            var patients = await query
                .Include(fp => fp.Patient)
                .OrderBy(fp => fp.Patient.FirstName)
                .ThenBy(fp => fp.Patient.LastName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(fp => new FacilityPatientResponse
                {
                    Id = fp.Id,
                    FacilityId = fp.FacilityId,
                    PatientId = fp.PatientId,
                    PatientName = $"{fp.Patient.FirstName} {fp.Patient.LastName}",
                    MedicalRecordNumber = fp.MedicalRecordNumber,
                    IsActive = fp.IsActive,
                    CreatedAt = fp.CreatedAt,
                    UpdatedAt = fp.UpdatedAt
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
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid search request");
            return BadRequest(new ErrorResponse
            {
                ErrorCode = "INVALID_REQUEST",
                Message = ex.Message
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
            _logger.LogError(ex, "Error searching patients");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while searching for patients"
            });
        }
    }

    /// <summary>
    /// Search for available patients to add to facility
    /// </summary>
    [HttpGet("available")]
    [ProducesResponseType(typeof(List<ApplicationUserResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchAvailablePatients(Guid facilityId, [FromQuery] string? keyword = null)
    {
        try
        {
            var facility = await _context.Facilities.FirstOrDefaultAsync(f => f.Id == facilityId);
            if (facility == null)
                throw new NotFoundException($"Facility with ID {facilityId} not found");

            var query = _context.Users
                .Where(u => u.Role.ToString() == "Patient" || u.Role.ToString() == "2")
                .AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(u =>
                    u.FirstName.Contains(keyword) ||
                    u.LastName.Contains(keyword) ||
                    u.Email.Contains(keyword));
            }

            var patients = await query
                .Select(u => new ApplicationUserResponse
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email
                })
                .Take(10)
                .ToListAsync();

            return Ok(patients);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching available patients");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while searching patients"
            });
        }
    }
}
