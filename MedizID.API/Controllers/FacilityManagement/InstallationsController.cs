using MedizID.API.Common.Exceptions;
using MedizID.API.Data;
using MedizID.API.DTOs;
using MedizID.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedizID.API.Controllers.FacilityManagement;

[ApiController]
[Route("api/v1/facilities/{facilityId}/installations")]
[Produces("application/json")]
[Authorize]
public class InstallationsController : ControllerBase
{
    private readonly MedizIDDbContext _context;
    private readonly ILogger<InstallationsController> _logger;

    public InstallationsController(MedizIDDbContext context, ILogger<InstallationsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Create installation
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(InstallationResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateInstallation(Guid facilityId, [FromBody] CreateInstallationRequest request)
    {
        try
        {
            var facility = await _context.Facilities.FirstOrDefaultAsync(f => f.Id == facilityId);
            if (facility == null)
                throw new NotFoundException($"Facility with ID {facilityId} not found");

            var installation = new Installation
            {
                Id = Guid.NewGuid(),
                FacilityId = facilityId,
                Name = request.Name,
                Type = request.Type,
                Location = request.Location,
                Condition = request.Condition,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Installations.Add(installation);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Installation created: {installation.Id}");

            var response = new InstallationResponse
            {
                Id = installation.Id,
                FacilityId = installation.FacilityId,
                Name = installation.Name,
                Type = installation.Type,
                Location = installation.Location,
                Condition = installation.Condition,
                LastMaintenanceDate = installation.LastMaintenanceDate,
                IsActive = installation.IsActive,
                CreatedAt = installation.CreatedAt,
                UpdatedAt = installation.UpdatedAt
            };

            return CreatedAtAction(nameof(GetInstallation), new { facilityId, installationId = installation.Id }, response);
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
            _logger.LogError(ex, "Error creating installation");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while creating the installation"
            });
        }
    }

    /// <summary>
    /// Get facility installations
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<InstallationResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetFacilityInstallations(Guid facilityId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var facility = await _context.Facilities.FirstOrDefaultAsync(f => f.Id == facilityId);
            if (facility == null)
                throw new NotFoundException($"Facility with ID {facilityId} not found");

            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var query = _context.Installations
                .Where(i => i.FacilityId == facilityId)
                .AsQueryable();

            var total = await query.CountAsync();

            var installations = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(i => new InstallationResponse
                {
                    Id = i.Id,
                    FacilityId = i.FacilityId,
                    Name = i.Name,
                    Type = i.Type,
                    Location = i.Location,
                    Condition = i.Condition,
                    LastMaintenanceDate = i.LastMaintenanceDate,
                    IsActive = i.IsActive,
                    CreatedAt = i.CreatedAt,
                    UpdatedAt = i.UpdatedAt
                })
                .ToListAsync();

            return Ok(new PagedResult<InstallationResponse>
            {
                Items = installations,
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
            _logger.LogError(ex, $"Error fetching facility installations {facilityId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching installations"
            });
        }
    }

    /// <summary>
    /// Get installation by ID
    /// </summary>
    [HttpGet("{installationId}")]
    [ProducesResponseType(typeof(InstallationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetInstallation(Guid facilityId, Guid installationId)
    {
        try
        {
            var facility = await _context.Facilities.FirstOrDefaultAsync(f => f.Id == facilityId);
            if (facility == null)
                throw new NotFoundException($"Facility with ID {facilityId} not found");

            var installation = await _context.Installations.FirstOrDefaultAsync(i => i.Id == installationId && i.FacilityId == facilityId);
            if (installation == null)
                throw new NotFoundException($"Installation with ID {installationId} not found");

            var response = new InstallationResponse
            {
                Id = installation.Id,
                FacilityId = installation.FacilityId,
                Name = installation.Name,
                Type = installation.Type,
                Location = installation.Location,
                Condition = installation.Condition,
                LastMaintenanceDate = installation.LastMaintenanceDate,
                IsActive = installation.IsActive,
                CreatedAt = installation.CreatedAt,
                UpdatedAt = installation.UpdatedAt
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
            _logger.LogError(ex, $"Error fetching installation {installationId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching the installation"
            });
        }
    }

    /// <summary>
    /// Update installation
    /// </summary>
    [HttpPut("{installationId}")]
    [ProducesResponseType(typeof(InstallationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateInstallation(Guid facilityId, Guid installationId, [FromBody] UpdateInstallationRequest request)
    {
        try
        {
            var facility = await _context.Facilities.FirstOrDefaultAsync(f => f.Id == facilityId);
            if (facility == null)
                throw new NotFoundException($"Facility with ID {facilityId} not found");

            var installation = await _context.Installations.FirstOrDefaultAsync(i => i.Id == installationId && i.FacilityId == facilityId);
            if (installation == null)
                throw new NotFoundException($"Installation with ID {installationId} not found");

            if (!string.IsNullOrEmpty(request.Name))
                installation.Name = request.Name;

            if (request.Type.HasValue)
                installation.Type = request.Type;

            if (!string.IsNullOrEmpty(request.Location))
                installation.Location = request.Location;

            if (!string.IsNullOrEmpty(request.Condition))
                installation.Condition = request.Condition;

            if (request.LastMaintenanceDate.HasValue)
                installation.LastMaintenanceDate = request.LastMaintenanceDate;

            if (request.IsActive.HasValue)
                installation.IsActive = request.IsActive.Value;

            installation.UpdatedAt = DateTime.UtcNow;

            _context.Installations.Update(installation);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Installation updated: {installationId}");

            var response = new InstallationResponse
            {
                Id = installation.Id,
                FacilityId = installation.FacilityId,
                Name = installation.Name,
                Type = installation.Type,
                Location = installation.Location,
                Condition = installation.Condition,
                LastMaintenanceDate = installation.LastMaintenanceDate,
                IsActive = installation.IsActive,
                CreatedAt = installation.CreatedAt,
                UpdatedAt = installation.UpdatedAt
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
            _logger.LogError(ex, $"Error updating installation {installationId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while updating the installation"
            });
        }
    }

    /// <summary>
    /// Delete installation
    /// </summary>
    [HttpDelete("{installationId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteInstallation(Guid facilityId, Guid installationId)
    {
        try
        {
            var facility = await _context.Facilities.FirstOrDefaultAsync(f => f.Id == facilityId);
            if (facility == null)
                throw new NotFoundException($"Facility with ID {facilityId} not found");

            var installation = await _context.Installations.FirstOrDefaultAsync(i => i.Id == installationId && i.FacilityId == facilityId);
            if (installation == null)
                throw new NotFoundException($"Installation with ID {installationId} not found");

            _context.Installations.Remove(installation);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Installation deleted: {installationId}");

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
            _logger.LogError(ex, $"Error deleting installation {installationId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while deleting the installation"
            });
        }
    }
}
