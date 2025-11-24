using MedizID.API.Common.Exceptions;
using MedizID.API.Common.Enums;
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
public class STIController : ControllerBase
{
    private readonly MedizIDDbContext _context;
    private readonly ILogger<STIController> _logger;

    public STIController(MedizIDDbContext context, ILogger<STIController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Create STI record
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(STIResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateSTIRecord([FromBody] CreateSTIRequest request)
    {
        try
        {
            var medicalRecord = await _context.MedicalRecords
                .FirstOrDefaultAsync(m => m.Id == request.MedicalRecordId);

            if (medicalRecord == null)
            {
                throw new NotFoundException($"Medical record with ID {request.MedicalRecordId} not found");
            }

            var sti = new STI
            {
                Id = Guid.NewGuid(),
                MedicalRecordId = request.MedicalRecordId,
                VisitStatus = request.VisitStatus != null ? Enum.Parse<STIVisitStatusEnum>(request.VisitStatus) : STIVisitStatusEnum.DatangSendiri,
                RiskGroup = request.RiskGroup != null ? Enum.Parse<STIRiskGroupEnum>(request.RiskGroup) : null,
                Symptoms = request.Screening,
                DiagnosisSTI = request.Diagnosis,
                Treatment = request.Treatment,
                CreatedAt = DateTime.UtcNow
            };

            _context.STIs.Add(sti);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"STI record created: {sti.Id}");

            var response = new STIResponse
            {
                Id = sti.Id,
                MedicalRecordId = sti.MedicalRecordId,
                VisitStatus = sti.VisitStatus.ToString(),
                RiskGroup = sti.RiskGroup?.ToString() ?? "",
                Screening = sti.Symptoms,
                Diagnosis = sti.DiagnosisSTI,
                Treatment = sti.Treatment,
                CreatedAt = sti.CreatedAt
            };

            return CreatedAtAction(nameof(GetSTIRecord), new { stiId = sti.Id }, response);
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
            _logger.LogError(ex, "Error creating STI record");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while creating STI record"
            });
        }
    }

    /// <summary>
    /// Get all STI records with pagination
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<STIResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSTIRecords([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var query = _context.STIs.AsQueryable();
            var total = await query.CountAsync();

            var stiRecords = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(s => new STIResponse
                {
                    Id = s.Id,
                    MedicalRecordId = s.MedicalRecordId,
                    VisitStatus = s.VisitStatus.ToString(),
                    RiskGroup = s.RiskGroup != null ? s.RiskGroup.ToString() : "",
                    Screening = s.Symptoms,
                    Diagnosis = s.DiagnosisSTI,
                    Treatment = s.Treatment,
                    CreatedAt = s.CreatedAt
                })
                .ToListAsync();

            return Ok(new PagedResult<STIResponse>
            {
                Items = stiRecords,
                Total = total,
                Page = page,
                PageSize = pageSize
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching STI records");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching STI records"
            });
        }
    }

    /// <summary>
    /// Get STI record by ID
    /// </summary>
    [HttpGet("{stiId}")]
    [ProducesResponseType(typeof(STIResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSTIRecord(Guid stiId)
    {
        try
        {
            var sti = await _context.STIs.FirstOrDefaultAsync(s => s.Id == stiId);

            if (sti == null)
            {
                throw new NotFoundException($"STI record with ID {stiId} not found");
            }

            var response = new STIResponse
            {
                Id = sti.Id,
                MedicalRecordId = sti.MedicalRecordId,
                VisitStatus = sti.VisitStatus.ToString(),
                RiskGroup = sti.RiskGroup?.ToString() ?? "",
                Screening = sti.Symptoms,
                Diagnosis = sti.DiagnosisSTI,
                Treatment = sti.Treatment,
                CreatedAt = sti.CreatedAt
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
            _logger.LogError(ex, $"Error fetching STI record {stiId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching STI record"
            });
        }
    }

    /// <summary>
    /// Get STI record by appointment
    /// </summary>
    [HttpGet("appointment/{appointmentId}")]
    [ProducesResponseType(typeof(STIResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSTIByAppointment(Guid appointmentId)
    {
        try
        {
            var appointment = await _context.Appointments
                .Include(a => a.MedicalRecords)
                .FirstOrDefaultAsync(a => a.Id == appointmentId);

            if (appointment == null)
            {
                throw new NotFoundException($"Appointment with ID {appointmentId} not found");
            }

            var medicalRecordIds = appointment.MedicalRecords.Select(m => m.Id).ToList();
            var sti = await _context.STIs
                .Where(s => medicalRecordIds.Contains(s.MedicalRecordId))
                .FirstOrDefaultAsync();

            if (sti == null)
            {
                throw new NotFoundException("No STI record found for this appointment");
            }

            var response = new STIResponse
            {
                Id = sti.Id,
                MedicalRecordId = sti.MedicalRecordId,
                AppointmentId = appointmentId,
                VisitStatus = sti.VisitStatus.ToString(),
                RiskGroup = sti.RiskGroup?.ToString() ?? "",
                Screening = sti.Symptoms,
                Diagnosis = sti.DiagnosisSTI,
                Treatment = sti.Treatment,
                CreatedAt = sti.CreatedAt
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
            _logger.LogError(ex, $"Error fetching STI record for appointment {appointmentId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching STI record"
            });
        }
    }

    /// <summary>
    /// Update STI record
    /// </summary>
    [HttpPut("{stiId}")]
    [ProducesResponseType(typeof(STIResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateSTIRecord(Guid stiId, [FromBody] UpdateSTIRequest request)
    {
        try
        {
            var sti = await _context.STIs.FirstOrDefaultAsync(s => s.Id == stiId);

            if (sti == null)
            {
                throw new NotFoundException($"STI record with ID {stiId} not found");
            }

            if (!string.IsNullOrEmpty(request.VisitStatus))
                sti.VisitStatus = Enum.Parse<STIVisitStatusEnum>(request.VisitStatus);

            if (!string.IsNullOrEmpty(request.RiskGroup))
                sti.RiskGroup = Enum.Parse<STIRiskGroupEnum>(request.RiskGroup);

            if (!string.IsNullOrEmpty(request.Screening))
                sti.Symptoms = request.Screening;

            if (!string.IsNullOrEmpty(request.Diagnosis))
                sti.DiagnosisSTI = request.Diagnosis;

            if (!string.IsNullOrEmpty(request.Treatment))
                sti.Treatment = request.Treatment;

            _context.STIs.Update(sti);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"STI record updated: {sti.Id}");

            var response = new STIResponse
            {
                Id = sti.Id,
                MedicalRecordId = sti.MedicalRecordId,
                VisitStatus = sti.VisitStatus.ToString(),
                RiskGroup = sti.RiskGroup?.ToString() ?? "",
                Screening = sti.Symptoms,
                Diagnosis = sti.DiagnosisSTI,
                Treatment = sti.Treatment,
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
            _logger.LogError(ex, $"Error updating STI record {stiId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while updating STI record"
            });
        }
    }

    /// <summary>
    /// Delete STI record
    /// </summary>
    [HttpDelete("{stiId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSTIRecord(Guid stiId)
    {
        try
        {
            var sti = await _context.STIs.FirstOrDefaultAsync(s => s.Id == stiId);

            if (sti == null)
            {
                throw new NotFoundException($"STI record with ID {stiId} not found");
            }

            _context.STIs.Remove(sti);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"STI record deleted: {stiId}");

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
            _logger.LogError(ex, $"Error deleting STI record {stiId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while deleting STI record"
            });
        }
    }
}
