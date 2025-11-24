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
public class DiagnosisController : ControllerBase
{
    private readonly MedizIDDbContext _context;
    private readonly ILogger<DiagnosisController> _logger;

    public DiagnosisController(MedizIDDbContext context, ILogger<DiagnosisController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Get all diagnoses with pagination
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<DiagnosisResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDiagnoses(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] Guid? medicalRecordId = null)
    {
        try
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var query = _context.Diagnoses.AsQueryable();

            if (medicalRecordId.HasValue)
                query = query.Where(d => d.MedicalRecordId == medicalRecordId.Value);

            var total = await query.CountAsync();

            var diagnoses = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(d => new DiagnosisResponse
                {
                    Id = d.Id,
                    DiagnosisCode = d.DiagnosisCode,
                    DiagnosisDescription = d.DiagnosisDescription,
                    DiagnosisType = d.DiagnosisType,
                    ConfidencePercentage = d.ConfidencePercentage,
                    CreatedAt = d.CreatedAt
                })
                .ToListAsync();

            return Ok(new PagedResult<DiagnosisResponse>
            {
                Items = diagnoses,
                Total = total,
                Page = page,
                PageSize = pageSize
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching diagnoses");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching diagnoses"
            });
        }
    }

    /// <summary>
    /// Get diagnosis by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(DiagnosisResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDiagnosis(Guid id)
    {
        try
        {
            var diagnosis = await _context.Diagnoses.FirstOrDefaultAsync(d => d.Id == id);

            if (diagnosis == null)
            {
                _logger.LogWarning($"Diagnosis not found: {id}");
                throw new NotFoundException($"Diagnosis with ID {id} not found");
            }

            var response = new DiagnosisResponse
            {
                Id = diagnosis.Id,
                DiagnosisCode = diagnosis.DiagnosisCode,
                DiagnosisDescription = diagnosis.DiagnosisDescription,
                DiagnosisType = diagnosis.DiagnosisType,
                ConfidencePercentage = diagnosis.ConfidencePercentage,
                CreatedAt = diagnosis.CreatedAt
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
            _logger.LogError(ex, $"Error fetching diagnosis {id}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching the diagnosis"
            });
        }
    }

    /// <summary>
    /// Create a new diagnosis
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(DiagnosisResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateDiagnosis([FromBody] CreateDiagnosisRequest request)
    {
        try
        {
            // Validate medical record exists
            var medicalRecord = await _context.MedicalRecords.FirstOrDefaultAsync(mr => mr.Id == request.MedicalRecordId);
            if (medicalRecord == null)
            {
                return BadRequest(new ErrorResponse
                {
                    ErrorCode = "MEDICAL_RECORD_NOT_FOUND",
                    Message = "Medical record not found"
                });
            }

            var diagnosis = new Diagnosis
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

            _context.Diagnoses.Add(diagnosis);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Diagnosis created: {diagnosis.Id}");

            var response = new DiagnosisResponse
            {
                Id = diagnosis.Id,
                DiagnosisCode = diagnosis.DiagnosisCode,
                DiagnosisDescription = diagnosis.DiagnosisDescription,
                DiagnosisType = diagnosis.DiagnosisType,
                ConfidencePercentage = diagnosis.ConfidencePercentage,
                CreatedAt = diagnosis.CreatedAt
            };

            return CreatedAtAction(nameof(GetDiagnosis), new { id = diagnosis.Id }, response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating diagnosis");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while creating the diagnosis"
            });
        }
    }

    /// <summary>
    /// Update diagnosis
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(DiagnosisResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateDiagnosis(Guid id, [FromBody] CreateDiagnosisRequest request)
    {
        try
        {
            var diagnosis = await _context.Diagnoses.FirstOrDefaultAsync(d => d.Id == id);

            if (diagnosis == null)
            {
                throw new NotFoundException($"Diagnosis with ID {id} not found");
            }

            diagnosis.DiagnosisCode = request.DiagnosisCode;
            diagnosis.DiagnosisDescription = request.DiagnosisDescription;
            diagnosis.DiagnosisType = request.DiagnosisType;
            diagnosis.ConfidencePercentage = request.ConfidencePercentage;
            diagnosis.Reason = request.Reason;

            _context.Diagnoses.Update(diagnosis);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Diagnosis updated: {diagnosis.Id}");

            var response = new DiagnosisResponse
            {
                Id = diagnosis.Id,
                DiagnosisCode = diagnosis.DiagnosisCode,
                DiagnosisDescription = diagnosis.DiagnosisDescription,
                DiagnosisType = diagnosis.DiagnosisType,
                ConfidencePercentage = diagnosis.ConfidencePercentage,
                CreatedAt = diagnosis.CreatedAt
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
            _logger.LogError(ex, $"Error updating diagnosis {id}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while updating the diagnosis"
            });
        }
    }

    /// <summary>
    /// Delete diagnosis
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteDiagnosis(Guid id)
    {
        try
        {
            var diagnosis = await _context.Diagnoses.FirstOrDefaultAsync(d => d.Id == id);

            if (diagnosis == null)
            {
                throw new NotFoundException($"Diagnosis with ID {id} not found");
            }

            _context.Diagnoses.Remove(diagnosis);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Diagnosis deleted: {id}");

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
            _logger.LogError(ex, $"Error deleting diagnosis {id}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while deleting the diagnosis"
            });
        }
    }
}
