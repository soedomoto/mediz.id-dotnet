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
public class AnamnesisController : ControllerBase
{
    private readonly MedizIDDbContext _context;
    private readonly ILogger<AnamnesisController> _logger;

    public AnamnesisController(MedizIDDbContext context, ILogger<AnamnesisController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Get all anamnesis records with pagination
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<AnamnesisResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAnamnesis(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] Guid? medicalRecordId = null)
    {
        try
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var query = _context.Anamnesis.AsQueryable();

            if (medicalRecordId.HasValue)
                query = query.Where(a => a.MedicalRecordId == medicalRecordId.Value);

            var total = await query.CountAsync();

            var records = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new AnamnesisResponse
                {
                    Id = a.Id,
                    MedicalRecordId = a.MedicalRecordId,
                    ChiefComplaint = a.ChiefComplaint,
                    AllergiesHistory = a.Allergies,
                    MedicalHistory = a.PastMedicalHistory,
                    CreatedAt = a.CreatedAt
                })
                .ToListAsync();

            return Ok(new PagedResult<AnamnesisResponse>
            {
                Items = records,
                Total = total,
                Page = page,
                PageSize = pageSize
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching anamnesis records");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching anamnesis records"
            });
        }
    }

    /// <summary>
    /// Get anamnesis record by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(AnamnesisResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAnamnesisRecord(Guid id)
    {
        try
        {
            var record = await _context.Anamnesis
                .Include(a => a.MedicalRecord)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (record == null)
            {
                _logger.LogWarning($"Anamnesis record not found: {id}");
                throw new NotFoundException($"Anamnesis record with ID {id} not found");
            }

            var response = new AnamnesisResponse
            {
                Id = record.Id,
                MedicalRecordId = record.MedicalRecordId,
                ChiefComplaint = record.ChiefComplaint,
                AllergiesHistory = record.Allergies,
                MedicalHistory = record.PastMedicalHistory,
                CreatedAt = record.CreatedAt
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
            _logger.LogError(ex, $"Error fetching anamnesis record {id}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching the anamnesis record"
            });
        }
    }

    /// <summary>
    /// Create a new anamnesis record
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(AnamnesisResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAnamnesis([FromBody] CreateAnamnesisRequest request)
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

            var anamnesis = new Anamnesis
            {
                Id = Guid.NewGuid(),
                MedicalRecordId = request.MedicalRecordId,
                ChiefComplaint = request.ChiefComplaint,
                PresentingIllness = request.PresentIllness,
                PastMedicalHistory = request.MedicalHistory,
                Allergies = request.AllergiesHistory,
                CurrentMedications = request.MedicationHistory,
                SocialHistory = request.SocialHistory,
                CreatedAt = DateTime.UtcNow
            };

            _context.Anamnesis.Add(anamnesis);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Anamnesis record created: {anamnesis.Id}");

            var response = new AnamnesisResponse
            {
                Id = anamnesis.Id,
                MedicalRecordId = anamnesis.MedicalRecordId,
                ChiefComplaint = anamnesis.ChiefComplaint,
                AllergiesHistory = anamnesis.Allergies,
                MedicalHistory = anamnesis.PastMedicalHistory,
                CreatedAt = anamnesis.CreatedAt
            };

            return CreatedAtAction(nameof(GetAnamnesisRecord), new { id = anamnesis.Id }, response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating anamnesis record");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while creating the anamnesis record"
            });
        }
    }

    /// <summary>
    /// Update anamnesis record
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(AnamnesisResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAnamnesis(Guid id, [FromBody] CreateAnamnesisRequest request)
    {
        try
        {
            var anamnesis = await _context.Anamnesis.FirstOrDefaultAsync(a => a.Id == id);

            if (anamnesis == null)
            {
                throw new NotFoundException($"Anamnesis record with ID {id} not found");
            }

            if (!string.IsNullOrEmpty(request.ChiefComplaint))
                anamnesis.ChiefComplaint = request.ChiefComplaint;

            if (!string.IsNullOrEmpty(request.PresentIllness))
                anamnesis.PresentingIllness = request.PresentIllness;

            if (!string.IsNullOrEmpty(request.MedicalHistory))
                anamnesis.PastMedicalHistory = request.MedicalHistory;

            if (!string.IsNullOrEmpty(request.AllergiesHistory))
                anamnesis.Allergies = request.AllergiesHistory;

            if (!string.IsNullOrEmpty(request.MedicationHistory))
                anamnesis.CurrentMedications = request.MedicationHistory;

            if (!string.IsNullOrEmpty(request.SocialHistory))
                anamnesis.SocialHistory = request.SocialHistory;

            _context.Anamnesis.Update(anamnesis);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Anamnesis record updated: {anamnesis.Id}");

            var response = new AnamnesisResponse
            {
                Id = anamnesis.Id,
                MedicalRecordId = anamnesis.MedicalRecordId,
                ChiefComplaint = anamnesis.ChiefComplaint,
                AllergiesHistory = anamnesis.Allergies,
                MedicalHistory = anamnesis.PastMedicalHistory,
                CreatedAt = anamnesis.CreatedAt
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
            _logger.LogError(ex, $"Error updating anamnesis record {id}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while updating the anamnesis record"
            });
        }
    }

    /// <summary>
    /// Delete anamnesis record
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAnamnesis(Guid id)
    {
        try
        {
            var anamnesis = await _context.Anamnesis.FirstOrDefaultAsync(a => a.Id == id);

            if (anamnesis == null)
            {
                throw new NotFoundException($"Anamnesis record with ID {id} not found");
            }

            _context.Anamnesis.Remove(anamnesis);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Anamnesis record deleted: {id}");

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
            _logger.LogError(ex, $"Error deleting anamnesis record {id}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while deleting the anamnesis record"
            });
        }
    }
}
