using MedizID.API.Common.Exceptions;
using MedizID.API.Data;
using MedizID.API.DTOs;
using MedizID.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedizID.API.Controllers.FacilityManagement;

[ApiController]
[Route("api/v1/facilities")]
[Produces("application/json")]
[Authorize]
public class PolisController : ControllerBase
{
    private readonly MedizIDDbContext _context;
    private readonly ILogger<PolisController> _logger;

    public PolisController(MedizIDDbContext context, ILogger<PolisController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Create a new poli (polyclinic)
    /// </summary>
    [HttpPost("{facilityId}/polis")]
    [ProducesResponseType(typeof(PoliResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreatePoli(Guid facilityId, [FromBody] CreatePoliRequest request)
    {
        try
        {
            var facility = await _context.Facilities.FirstOrDefaultAsync(f => f.Id == facilityId);
            if (facility == null)
                throw new NotFoundException($"Facility with ID {facilityId} not found");

            var installation = await _context.Installations.FirstOrDefaultAsync(i => i.Id == request.InstallationId && i.FacilityId == facilityId);
            if (installation == null)
                throw new NotFoundException($"Installation with ID {request.InstallationId} not found in this facility");

            var poli = new Poli
            {
                Id = Guid.NewGuid(),
                FacilityId = facilityId,
                InstallationId = request.InstallationId,
                Name = request.Name,
                Description = request.Description,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Polis.Add(poli);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Poli created: {poli.Id}");

            var response = new PoliResponse
            {
                Id = poli.Id,
                FacilityId = poli.FacilityId,
                InstallationId = poli.InstallationId,
                Name = poli.Name,
                Description = poli.Description,
                IsActive = poli.IsActive,
                CreatedAt = poli.CreatedAt
            };

            return CreatedAtAction(nameof(GetPolis), new { facilityId }, response);
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
            _logger.LogError(ex, "Error creating poli");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while creating the poli"
            });
        }
    }

    /// <summary>
    /// Get all polis for a facility
    /// </summary>
    [HttpGet("{facilityId}/polis")]
    [ProducesResponseType(typeof(PagedResult<PoliResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPolis(Guid facilityId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var facility = await _context.Facilities.FirstOrDefaultAsync(f => f.Id == facilityId);
            if (facility == null)
                throw new NotFoundException($"Facility with ID {facilityId} not found");

            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var query = _context.Polis
                .Where(p => p.FacilityId == facilityId && p.IsActive)
                .AsQueryable();

            var total = await query.CountAsync();

            var polis = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new PoliResponse
                {
                    Id = p.Id,
                    FacilityId = p.FacilityId,
                    InstallationId = p.InstallationId,
                    Name = p.Name,
                    Description = p.Description,
                    IsActive = p.IsActive,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                })
                .ToListAsync();

            return Ok(new PagedResult<PoliResponse>
            {
                Items = polis,
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
            _logger.LogError(ex, $"Error fetching polis for facility {facilityId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching polis"
            });
        }
    }

    /// <summary>
    /// Get a specific poli by ID
    /// </summary>
    [HttpGet("{facilityId}/polis/{poliId}")]
    [ProducesResponseType(typeof(PoliResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPoliById(Guid facilityId, Guid poliId)
    {
        try
        {
            var poli = await _context.Polis.FirstOrDefaultAsync(p => p.Id == poliId && p.FacilityId == facilityId);
            if (poli == null)
                throw new NotFoundException($"Poli with ID {poliId} not found");

            var response = new PoliResponse
            {
                Id = poli.Id,
                FacilityId = poli.FacilityId,
                InstallationId = poli.InstallationId,
                Name = poli.Name,
                Description = poli.Description,
                IsActive = poli.IsActive,
                CreatedAt = poli.CreatedAt,
                UpdatedAt = poli.UpdatedAt
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
            _logger.LogError(ex, $"Error fetching poli {poliId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching the poli"
            });
        }
    }

    /// <summary>
    /// Update a poli
    /// </summary>
    [HttpPut("{facilityId}/polis/{poliId}")]
    [ProducesResponseType(typeof(PoliResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdatePoli(Guid facilityId, Guid poliId, [FromBody] UpdatePoliRequest request)
    {
        try
        {
            var poli = await _context.Polis.FirstOrDefaultAsync(p => p.Id == poliId && p.FacilityId == facilityId);
            if (poli == null)
                throw new NotFoundException($"Poli with ID {poliId} not found");

            if (!string.IsNullOrEmpty(request.Name))
                poli.Name = request.Name;

            if (!string.IsNullOrEmpty(request.Description))
                poli.Description = request.Description;

            if (request.InstallationId.HasValue)
                poli.InstallationId = request.InstallationId.Value;

            if (request.IsActive.HasValue)
                poli.IsActive = request.IsActive.Value;

            poli.UpdatedAt = DateTime.UtcNow;

            _context.Polis.Update(poli);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Poli updated: {poli.Id}");

            var response = new PoliResponse
            {
                Id = poli.Id,
                FacilityId = poli.FacilityId,
                InstallationId = poli.InstallationId,
                Name = poli.Name,
                Description = poli.Description,
                IsActive = poli.IsActive,
                CreatedAt = poli.CreatedAt,
                UpdatedAt = poli.UpdatedAt
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
            _logger.LogError(ex, $"Error updating poli {poliId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while updating the poli"
            });
        }
    }

    /// <summary>
    /// Delete a poli
    /// </summary>
    [HttpDelete("{facilityId}/polis/{poliId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeletePoli(Guid facilityId, Guid poliId)
    {
        try
        {
            var poli = await _context.Polis.FirstOrDefaultAsync(p => p.Id == poliId && p.FacilityId == facilityId);
            if (poli == null)
                throw new NotFoundException($"Poli with ID {poliId} not found");

            _context.Polis.Remove(poli);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Poli deleted: {poli.Id}");

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
            _logger.LogError(ex, $"Error deleting poli {poliId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while deleting the poli"
            });
        }
    }
}
