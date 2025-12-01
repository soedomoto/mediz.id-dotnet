using MedizID.API.Common.Exceptions;
using MedizID.API.Data;
using MedizID.API.DTOs;
using MedizID.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedizID.API.Controllers.SpecializedServices;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
[Authorize]
public class AdolescentHealthController : ControllerBase
{
    private readonly MedizIDDbContext _context;
    private readonly ILogger<AdolescentHealthController> _logger;

    public AdolescentHealthController(MedizIDDbContext context, ILogger<AdolescentHealthController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Create adolescent health record for an appointment
    /// </summary>
    [HttpPost("appointments/{appointmentId}/adolescent-health")]
    [ProducesResponseType(typeof(AdolescentHealthResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAdolescentHealthRecord(Guid appointmentId, [FromBody] CreateAdolescentHealthRequest request)
    {
        try
        {
            var appointment = await _context.Appointments
                .Include(a => a.Anamnesis)
                .FirstOrDefaultAsync(a => a.Id == appointmentId);

            if (appointment == null)
            {
                throw new NotFoundException($"Appointment with ID {appointmentId} not found");
            }

            var adolescentHealth = new AdolescentHealth
            {
                Id = Guid.NewGuid(),
                AppointmentId = appointmentId,
                PubertanStage = request.HealthConcern,
                RiskyBehaviors = request.SexualActivity,
                MentalHealthStatus = request.Contraception,
                Notes = request.Counseling,
                CreatedAt = DateTime.UtcNow
            };

            _context.AdolescentHealths.Add(adolescentHealth);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Adolescent health record created: {adolescentHealth.Id}");

            var response = new AdolescentHealthResponse
            {
                Id = adolescentHealth.Id,
                AppointmentId = adolescentHealth.AppointmentId,
                HealthConcern = adolescentHealth.PubertanStage,
                SexualActivity = adolescentHealth.RiskyBehaviors,
                Contraception = adolescentHealth.MentalHealthStatus,
                Counseling = adolescentHealth.Notes,
                CreatedAt = adolescentHealth.CreatedAt
            };

            return CreatedAtAction(nameof(GetAdolescentHealthRecord), new { recordId = adolescentHealth.Id }, response);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ErrorResponse
            {
                ErrorCode = ex.ErrorCode,
                Message = ex.Message
            });
        }
        catch (ApiException ex)
        {
            return BadRequest(new ErrorResponse
            {
                ErrorCode = ex.ErrorCode,
                Message = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating adolescent health record");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while creating adolescent health record"
            });
        }
    }

    /// <summary>
    /// Get adolescent health record by appointment
    /// </summary>
    [HttpGet("appointments/{appointmentId}/adolescent-health")]
    [ProducesResponseType(typeof(AdolescentHealthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAdolescentHealthRecordByAppointment(Guid appointmentId)
    {
        try
        {
            var appointment = await _context.Appointments
                .Include(a => a.Anamnesis)
                .FirstOrDefaultAsync(a => a.Id == appointmentId);

            if (appointment == null)
            {
                throw new NotFoundException($"Appointment with ID {appointmentId} not found");
            }

            var medicalRecordIds = appointment.Anamnesis.Select(m => m.Id).ToList();
            var adolescentHealth = await _context.AdolescentHealths
                .Where(a => medicalRecordIds.Contains(a.AppointmentId))
                .FirstOrDefaultAsync();

            if (adolescentHealth == null)
            {
                throw new NotFoundException("No adolescent health record found for this appointment");
            }

            var response = new AdolescentHealthResponse
            {
                Id = adolescentHealth.Id,
                AppointmentId = adolescentHealth.AppointmentId,
                HealthConcern = adolescentHealth.PubertanStage,
                SexualActivity = adolescentHealth.RiskyBehaviors,
                Contraception = adolescentHealth.MentalHealthStatus,
                Counseling = adolescentHealth.Notes,
                CreatedAt = adolescentHealth.CreatedAt
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
            _logger.LogError(ex, $"Error fetching adolescent health record for appointment {appointmentId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching adolescent health record"
            });
        }
    }

    /// <summary>
    /// Get adolescent health record by ID
    /// </summary>
    [HttpGet("{recordId}")]
    [ProducesResponseType(typeof(AdolescentHealthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAdolescentHealthRecord(Guid recordId)
    {
        try
        {
            var adolescentHealth = await _context.AdolescentHealths.FirstOrDefaultAsync(a => a.Id == recordId);

            if (adolescentHealth == null)
            {
                throw new NotFoundException($"Adolescent health record with ID {recordId} not found");
            }

            var response = new AdolescentHealthResponse
            {
                Id = adolescentHealth.Id,
                AppointmentId = adolescentHealth.AppointmentId,
                HealthConcern = adolescentHealth.PubertanStage,
                SexualActivity = adolescentHealth.RiskyBehaviors,
                Contraception = adolescentHealth.MentalHealthStatus,
                Counseling = adolescentHealth.Notes,
                CreatedAt = adolescentHealth.CreatedAt
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
            _logger.LogError(ex, $"Error fetching adolescent health record {recordId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching adolescent health record"
            });
        }
    }

    /// <summary>
    /// Update adolescent health record for an appointment
    /// </summary>
    [HttpPut("appointments/{appointmentId}/adolescent-health")]
    [ProducesResponseType(typeof(AdolescentHealthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAdolescentHealthRecordByAppointment(Guid appointmentId, [FromBody] UpdateAdolescentHealthRequest request)
    {
        try
        {
            var appointment = await _context.Appointments
                .Include(a => a.Anamnesis)
                .FirstOrDefaultAsync(a => a.Id == appointmentId);

            if (appointment == null)
            {
                throw new NotFoundException($"Appointment with ID {appointmentId} not found");
            }

            var medicalRecordIds = appointment.Anamnesis.Select(m => m.Id).ToList();
            var adolescentHealth = await _context.AdolescentHealths
                .Where(a => medicalRecordIds.Contains(a.AppointmentId))
                .FirstOrDefaultAsync();

            if (adolescentHealth == null)
            {
                throw new NotFoundException("No adolescent health record found for this appointment");
            }

            if (!string.IsNullOrEmpty(request.HealthConcern))
                adolescentHealth.PubertanStage = request.HealthConcern;

            if (!string.IsNullOrEmpty(request.SexualActivity))
                adolescentHealth.RiskyBehaviors = request.SexualActivity;

            if (!string.IsNullOrEmpty(request.Contraception))
                adolescentHealth.MentalHealthStatus = request.Contraception;

            if (!string.IsNullOrEmpty(request.Counseling))
                adolescentHealth.Notes = request.Counseling;

            _context.AdolescentHealths.Update(adolescentHealth);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Adolescent health record updated: {adolescentHealth.Id}");

            var response = new AdolescentHealthResponse
            {
                Id = adolescentHealth.Id,
                AppointmentId = adolescentHealth.AppointmentId,
                HealthConcern = adolescentHealth.PubertanStage,
                SexualActivity = adolescentHealth.RiskyBehaviors,
                Contraception = adolescentHealth.MentalHealthStatus,
                Counseling = adolescentHealth.Notes,
                UpdatedAt = DateTime.UtcNow
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
            _logger.LogError(ex, $"Error updating adolescent health record for appointment {appointmentId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while updating adolescent health record"
            });
        }
    }

    /// <summary>
    /// Update adolescent health record by ID
    /// </summary>
    [HttpPut("{recordId}")]
    [ProducesResponseType(typeof(AdolescentHealthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAdolescentHealthRecord(Guid recordId, [FromBody] UpdateAdolescentHealthRequest request)
    {
        try
        {
            var adolescentHealth = await _context.AdolescentHealths.FirstOrDefaultAsync(a => a.Id == recordId);

            if (adolescentHealth == null)
            {
                throw new NotFoundException($"Adolescent health record with ID {recordId} not found");
            }

            if (!string.IsNullOrEmpty(request.HealthConcern))
                adolescentHealth.PubertanStage = request.HealthConcern;

            if (!string.IsNullOrEmpty(request.SexualActivity))
                adolescentHealth.RiskyBehaviors = request.SexualActivity;

            if (!string.IsNullOrEmpty(request.Contraception))
                adolescentHealth.MentalHealthStatus = request.Contraception;

            if (!string.IsNullOrEmpty(request.Counseling))
                adolescentHealth.Notes = request.Counseling;

            _context.AdolescentHealths.Update(adolescentHealth);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Adolescent health record updated: {adolescentHealth.Id}");

            var response = new AdolescentHealthResponse
            {
                Id = adolescentHealth.Id,
                AppointmentId = adolescentHealth.AppointmentId,
                HealthConcern = adolescentHealth.PubertanStage,
                SexualActivity = adolescentHealth.RiskyBehaviors,
                Contraception = adolescentHealth.MentalHealthStatus,
                Counseling = adolescentHealth.Notes,
                UpdatedAt = DateTime.UtcNow
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
            _logger.LogError(ex, $"Error updating adolescent health record {recordId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while updating adolescent health record"
            });
        }
    }

    /// <summary>
    /// Delete adolescent health record for an appointment
    /// </summary>
    [HttpDelete("appointments/{appointmentId}/adolescent-health")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAdolescentHealthRecordByAppointment(Guid appointmentId)
    {
        try
        {
            var appointment = await _context.Appointments
                .Include(a => a.Anamnesis)
                .FirstOrDefaultAsync(a => a.Id == appointmentId);

            if (appointment == null)
            {
                throw new NotFoundException($"Appointment with ID {appointmentId} not found");
            }

            var medicalRecordIds = appointment.Anamnesis.Select(m => m.Id).ToList();
            var adolescentHealth = await _context.AdolescentHealths
                .Where(a => medicalRecordIds.Contains(a.AppointmentId))
                .FirstOrDefaultAsync();

            if (adolescentHealth == null)
            {
                throw new NotFoundException("No adolescent health record found for this appointment");
            }

            _context.AdolescentHealths.Remove(adolescentHealth);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Adolescent health record deleted: {adolescentHealth.Id}");

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
            _logger.LogError(ex, $"Error deleting adolescent health record for appointment {appointmentId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while deleting adolescent health record"
            });
        }
    }

    /// <summary>
    /// Delete adolescent health record by ID
    /// </summary>
    [HttpDelete("{recordId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAdolescentHealthRecord(Guid recordId)
    {
        try
        {
            var adolescentHealth = await _context.AdolescentHealths.FirstOrDefaultAsync(a => a.Id == recordId);

            if (adolescentHealth == null)
            {
                throw new NotFoundException($"Adolescent health record with ID {recordId} not found");
            }

            _context.AdolescentHealths.Remove(adolescentHealth);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Adolescent health record deleted: {recordId}");

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
            _logger.LogError(ex, $"Error deleting adolescent health record {recordId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while deleting adolescent health record"
            });
        }
    }
}
