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
public class OdontogramController : ControllerBase
{
    private readonly MedizIDDbContext _context;
    private readonly ILogger<OdontogramController> _logger;

    public OdontogramController(MedizIDDbContext context, ILogger<OdontogramController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Get odontogram options (tooth statuses, surfaces, treatments)
    /// </summary>
    [HttpGet("options")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public IActionResult GetOdontogramOptions()
    {
        try
        {
            var options = new
            {
                statuses = new[] { "Caries", "Filling", "Missing", "Crown", "Root Canal", "Sealed", "Healthy" },
                surfaces = new[] { "Occlusal", "Buccal", "Lingual", "Mesial", "Distal", "Incisal" },
                treatments = new[] { "Filling", "Crown", "Extraction", "Root Canal", "Scaling", "Bleaching" }
            };

            return Ok(options);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching odontogram options");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching odontogram options"
            });
        }
    }

    /// <summary>
    /// Get odontogram by appointment
    /// </summary>
    [HttpGet("appointment/{appointmentId}")]
    [ProducesResponseType(typeof(List<OdontogramResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOdontogramByAppointment(Guid appointmentId)
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
            var odontograms = await _context.Odontograms
                .Where(o => medicalRecordIds.Contains(o.MedicalRecordId))
                .ToListAsync();

            var responses = odontograms.Select(o => new OdontogramResponse
            {
                Id = o.Id,
                ToothNumber = o.ToothNumber,
                Status = o.Status,
                Treatment = o.Treatment,
                CreatedAt = o.CreatedAt
            }).ToList();

            return Ok(responses);
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
            _logger.LogError(ex, $"Error fetching odontogram for appointment {appointmentId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching odontograms"
            });
        }
    }

    /// <summary>
    /// Create odontogram
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(OdontogramResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateOdontogram([FromBody] CreateOdontogramRequest request)
    {
        try
        {
            var medicalRecord = await _context.MedicalRecords
                .FirstOrDefaultAsync(m => m.Id == request.MedicalRecordId);

            if (medicalRecord == null)
            {
                throw new NotFoundException($"Medical record with ID {request.MedicalRecordId} not found");
            }

            var odontogram = new Odontogram
            {
                Id = Guid.NewGuid(),
                MedicalRecordId = request.MedicalRecordId,
                ToothNumber = request.ToothNumber,
                Surface = request.Surface,
                Status = request.Status,
                Treatment = request.Treatment,
                CreatedAt = DateTime.UtcNow
            };

            _context.Odontograms.Add(odontogram);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Odontogram created: {odontogram.Id}");

            var response = new OdontogramResponse
            {
                Id = odontogram.Id,
                ToothNumber = odontogram.ToothNumber,
                Status = odontogram.Status,
                Treatment = odontogram.Treatment,
                CreatedAt = odontogram.CreatedAt
            };

            return CreatedAtAction(nameof(GetOdontogram), new { id = odontogram.Id }, response);
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
            _logger.LogError(ex, "Error creating odontogram");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while creating odontogram"
            });
        }
    }

    /// <summary>
    /// Update odontogram
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(OdontogramResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateOdontogram(Guid id, [FromBody] CreateOdontogramRequest request)
    {
        try
        {
            var odontogram = await _context.Odontograms.FirstOrDefaultAsync(o => o.Id == id);

            if (odontogram == null)
            {
                throw new NotFoundException($"Odontogram with ID {id} not found");
            }

            if (!string.IsNullOrEmpty(request.ToothNumber))
                odontogram.ToothNumber = request.ToothNumber;

            if (!string.IsNullOrEmpty(request.Surface))
                odontogram.Surface = request.Surface;

            if (!string.IsNullOrEmpty(request.Status))
                odontogram.Status = request.Status;

            if (!string.IsNullOrEmpty(request.Treatment))
                odontogram.Treatment = request.Treatment;

            _context.Odontograms.Update(odontogram);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Odontogram updated: {odontogram.Id}");

            var response = new OdontogramResponse
            {
                Id = odontogram.Id,
                ToothNumber = odontogram.ToothNumber,
                Status = odontogram.Status,
                Treatment = odontogram.Treatment,
                CreatedAt = odontogram.CreatedAt
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
            _logger.LogError(ex, $"Error updating odontogram {id}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while updating odontogram"
            });
        }
    }

    /// <summary>
    /// Get odontogram by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(OdontogramResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOdontogram(Guid id)
    {
        try
        {
            var odontogram = await _context.Odontograms.FirstOrDefaultAsync(o => o.Id == id);

            if (odontogram == null)
            {
                throw new NotFoundException($"Odontogram with ID {id} not found");
            }

            var response = new OdontogramResponse
            {
                Id = odontogram.Id,
                ToothNumber = odontogram.ToothNumber,
                Status = odontogram.Status,
                Treatment = odontogram.Treatment,
                CreatedAt = odontogram.CreatedAt
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
            _logger.LogError(ex, $"Error fetching odontogram {id}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching odontogram"
            });
        }
    }

    /// <summary>
    /// Delete odontogram
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteOdontogram(Guid id)
    {
        try
        {
            var odontogram = await _context.Odontograms.FirstOrDefaultAsync(o => o.Id == id);

            if (odontogram == null)
            {
                throw new NotFoundException($"Odontogram with ID {id} not found");
            }

            _context.Odontograms.Remove(odontogram);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Odontogram deleted: {id}");

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
            _logger.LogError(ex, $"Error deleting odontogram {id}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while deleting odontogram"
            });
        }
    }

    /// <summary>
    /// List odontograms with pagination
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<OdontogramResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListOdontograms([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var query = _context.Odontograms.AsQueryable();
            var total = await query.CountAsync();

            var odontograms = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(o => new OdontogramResponse
                {
                    Id = o.Id,
                    ToothNumber = o.ToothNumber,
                    Status = o.Status,
                    Treatment = o.Treatment,
                    CreatedAt = o.CreatedAt
                })
                .ToListAsync();

            return Ok(new PagedResult<OdontogramResponse>
            {
                Items = odontograms,
                Total = total,
                Page = page,
                PageSize = pageSize
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching odontograms");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching odontograms"
            });
        }
    }
}
