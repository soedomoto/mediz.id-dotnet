using MedizID.API.Common.Exceptions;
using MedizID.API.Data;
using MedizID.API.DTOs;
using MedizID.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedizID.API.Controllers.Appointments;

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
    /// Save or update tooth states for an appointment
    /// </summary>
    [HttpPost("save-tooth-states")]
    [ProducesResponseType(typeof(OdontogramResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(OdontogramResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SaveToothStates([FromBody] SaveOdontogramRequest request)
    {
        try
        {
            if (request == null || request.AppointmentId == Guid.Empty)
            {
                return BadRequest(new ErrorResponse
                {
                    ErrorCode = "INVALID_REQUEST",
                    Message = "Invalid appointment ID"
                });
            }

            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.Id == request.AppointmentId);

            if (appointment == null)
            {
                throw new NotFoundException($"Appointment with ID {request.AppointmentId} not found");
            }

            // Check if odontogram already exists for this appointment
            var existingOdontogram = await _context.Odontograms
                .Include(o => o.Surfaces)
                .FirstOrDefaultAsync(o => o.AppointmentId == request.AppointmentId);

            Odontogram odontogram;
            bool isNew = false;

            if (existingOdontogram == null)
            {
                // Create new odontogram
                odontogram = new Odontogram
                {
                    Id = Guid.NewGuid(),
                    AppointmentId = request.AppointmentId,
                    CreatedAt = DateTime.UtcNow,
                    Surfaces = new List<OdontogramSurface>()
                };
                _context.Odontograms.Add(odontogram);
                isNew = true;
            }
            else
            {
                odontogram = existingOdontogram;
                odontogram.UpdatedAt = DateTime.UtcNow;
            }

            // Remove existing surfaces that are not in the new request
            var newSurfaceKeys = request.ToothStates
                .Select(ts => (ts.Number, ts.Surface))
                .ToHashSet();

            var surfacesToRemove = odontogram.Surfaces
                .Where(s => !newSurfaceKeys.Contains((s.ToothNumber, s.Surface)))
                .ToList();

            foreach (var surface in surfacesToRemove)
            {
                odontogram.Surfaces.Remove(surface);
            }

            // Update or add surfaces
            foreach (var state in request.ToothStates)
            {
                var existingSurface = odontogram.Surfaces
                    .FirstOrDefault(s => s.ToothNumber == state.Number && s.Surface == state.Surface);

                if (existingSurface != null)
                {
                    existingSurface.ConditionCode = state.ConditionCode;
                    existingSurface.UpdatedAt = DateTime.UtcNow;
                }
                else
                {
                    var newSurface = new OdontogramSurface
                    {
                        Id = Guid.NewGuid(),
                        OdontogramId = odontogram.Id,
                        ToothNumber = state.Number,
                        Surface = state.Surface,
                        ConditionCode = state.ConditionCode,
                        CreatedAt = DateTime.UtcNow
                    };
                    odontogram.Surfaces.Add(newSurface);
                }
            }

            await _context.SaveChangesAsync();

            _logger.LogInformation($"Odontogram {(isNew ? "created" : "updated")}: {odontogram.Id}");

            var response = new OdontogramResponse
            {
                Id = odontogram.Id,
                AppointmentId = odontogram.AppointmentId,
                Surfaces = odontogram.Surfaces.Select(s => new OdontogramSurfaceResponse
                {
                    Id = s.Id,
                    ToothNumber = s.ToothNumber,
                    Surface = s.Surface,
                    ConditionCode = s.ConditionCode,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt
                }).ToList(),
                CreatedAt = odontogram.CreatedAt,
                UpdatedAt = odontogram.UpdatedAt
            };

            return isNew ? CreatedAtAction(nameof(GetOdontogramByAppointment), 
                new { appointmentId = odontogram.AppointmentId }, response) : Ok(response);
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
            _logger.LogError(ex, "Error saving tooth states");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while saving tooth states"
            });
        }
    }

    /// <summary>
    /// Get odontogram by appointment
    /// </summary>
    [HttpGet("appointment/{appointmentId}")]
    [ProducesResponseType(typeof(OdontogramResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOdontogramByAppointment(Guid appointmentId)
    {
        try
        {
            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.Id == appointmentId);

            if (appointment == null)
            {
                throw new NotFoundException($"Appointment with ID {appointmentId} not found");
            }

            var odontogram = await _context.Odontograms
                .Include(o => o.Surfaces)
                .FirstOrDefaultAsync(o => o.AppointmentId == appointmentId);

            if (odontogram == null)
            {
                // Return empty odontogram structure
                return Ok(new OdontogramResponse
                {
                    Id = Guid.Empty,
                    AppointmentId = appointmentId,
                    Surfaces = new List<OdontogramSurfaceResponse>(),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = null
                });
            }

            var response = new OdontogramResponse
            {
                Id = odontogram.Id,
                AppointmentId = odontogram.AppointmentId,
                Surfaces = odontogram.Surfaces.Select(s => new OdontogramSurfaceResponse
                {
                    Id = s.Id,
                    ToothNumber = s.ToothNumber,
                    Surface = s.Surface,
                    ConditionCode = s.ConditionCode,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt
                }).ToList(),
                CreatedAt = odontogram.CreatedAt,
                UpdatedAt = odontogram.UpdatedAt
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
            _logger.LogError(ex, $"Error fetching odontogram for appointment {appointmentId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching odontogram"
            });
        }
    }

    /// <summary>
    /// Create odontogram (legacy endpoint)
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(OdontogramResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateOdontogram([FromBody] CreateOdontogramRequest request)
    {
        try
        {
            var medicalRecord = await _context.Appointments
                .FirstOrDefaultAsync(m => m.Id == request.AppointmentId);

            if (medicalRecord == null)
            {
                throw new NotFoundException($"Appointment with ID {request.AppointmentId} not found");
            }

            var odontogram = new Odontogram
            {
                Id = Guid.NewGuid(),
                AppointmentId = request.AppointmentId,
                CreatedAt = DateTime.UtcNow,
                Surfaces = new List<OdontogramSurface>()
            };

            _context.Odontograms.Add(odontogram);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Odontogram created: {odontogram.Id}");

            var response = new OdontogramResponse
            {
                Id = odontogram.Id,
                AppointmentId = odontogram.AppointmentId,
                Surfaces = new List<OdontogramSurfaceResponse>(),
                CreatedAt = odontogram.CreatedAt,
                UpdatedAt = odontogram.UpdatedAt
            };

            return CreatedAtAction(nameof(GetOdontogramByAppointment), 
                new { appointmentId = odontogram.AppointmentId }, response);
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
    /// Get odontogram by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(OdontogramResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOdontogram(Guid id)
    {
        try
        {
            var odontogram = await _context.Odontograms
                .Include(o => o.Surfaces)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (odontogram == null)
            {
                throw new NotFoundException($"Odontogram with ID {id} not found");
            }

            var response = new OdontogramResponse
            {
                Id = odontogram.Id,
                AppointmentId = odontogram.AppointmentId,
                Surfaces = odontogram.Surfaces.Select(s => new OdontogramSurfaceResponse
                {
                    Id = s.Id,
                    ToothNumber = s.ToothNumber,
                    Surface = s.Surface,
                    ConditionCode = s.ConditionCode,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt
                }).ToList(),
                CreatedAt = odontogram.CreatedAt,
                UpdatedAt = odontogram.UpdatedAt
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
            var odontogram = await _context.Odontograms
                .Include(o => o.Surfaces)
                .FirstOrDefaultAsync(o => o.Id == id);

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

            var query = _context.Odontograms
                .Include(o => o.Surfaces)
                .AsQueryable();
            var total = await query.CountAsync();

            var odontograms = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(o => new OdontogramResponse
                {
                    Id = o.Id,
                    AppointmentId = o.AppointmentId,
                    Surfaces = o.Surfaces.Select(s => new OdontogramSurfaceResponse
                    {
                        Id = s.Id,
                        ToothNumber = s.ToothNumber,
                        Surface = s.Surface,
                        ConditionCode = s.ConditionCode,
                        CreatedAt = s.CreatedAt,
                        UpdatedAt = s.UpdatedAt
                    }).ToList(),
                    CreatedAt = o.CreatedAt,
                    UpdatedAt = o.UpdatedAt
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
