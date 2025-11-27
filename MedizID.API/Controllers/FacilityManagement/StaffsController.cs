using MedizID.API.Common.Exceptions;
using MedizID.API.Data;
using MedizID.API.DTOs;
using MedizID.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedizID.API.Controllers.FacilityManagement;

[ApiController]
[Route("api/v1/facilities/{facilityId}/staff")]
[Produces("application/json")]
[Authorize]
public class StaffsController : ControllerBase
{
    private readonly MedizIDDbContext _context;
    private readonly ILogger<StaffsController> _logger;

    public StaffsController(MedizIDDbContext context, ILogger<StaffsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Add staff to facility
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(FacilityStaffResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddStaffToFacility(Guid facilityId, [FromBody] AddStaffToFacilityRequest request)
    {
        try
        {
            var facility = await _context.Facilities.FirstOrDefaultAsync(f => f.Id == facilityId);
            if (facility == null)
                throw new NotFoundException($"Facility with ID {facilityId} not found");

            var staff = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.StaffId);
            if (staff == null)
                throw new NotFoundException($"Staff with ID {request.StaffId} not found");

            var facilityStaff = new FacilityStaff
            {
                Id = Guid.NewGuid(),
                FacilityId = facilityId,
                StaffId = request.StaffId,
                DepartmentId = request.DepartmentId,
                Position = request.Position,
                Specialization = request.Specialization,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.FacilityStaffs.Add(facilityStaff);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Staff added to facility: {facilityStaff.Id}");

            var response = new FacilityStaffResponse
            {
                Id = facilityStaff.Id,
                FacilityId = facilityStaff.FacilityId,
                StaffId = facilityStaff.StaffId,
                DepartmentId = facilityStaff.DepartmentId,
                Position = facilityStaff.Position,
                Specialization = facilityStaff.Specialization,
                StartDate = facilityStaff.StartDate,
                EndDate = facilityStaff.EndDate,
                IsActive = facilityStaff.IsActive,
                CreatedAt = facilityStaff.CreatedAt,
                UpdatedAt = facilityStaff.UpdatedAt
            };

            return CreatedAtAction(nameof(ListStaff), new { facilityId }, response);
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
            _logger.LogError(ex, "Error adding staff to facility");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while adding staff"
            });
        }
    }

    /// <summary>
    /// List staff for facility
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<FacilityStaffResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ListStaff(Guid facilityId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var facility = await _context.Facilities.FirstOrDefaultAsync(f => f.Id == facilityId);
            if (facility == null)
                throw new NotFoundException($"Facility with ID {facilityId} not found");

            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var query = _context.FacilityStaffs
                .Where(fs => fs.FacilityId == facilityId && fs.IsActive)
                .AsQueryable();

            var total = await query.CountAsync();

            var staff = await query
                .Include(fs => fs.Staff)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(fs => new FacilityStaffResponse
                {
                    Id = fs.Id,
                    FacilityId = fs.FacilityId,
                    StaffId = fs.StaffId,
                    DepartmentId = fs.DepartmentId,
                    Position = fs.Position,
                    Specialization = fs.Specialization,
                    StartDate = fs.StartDate,
                    EndDate = fs.EndDate,
                    IsActive = fs.IsActive,
                    CreatedAt = fs.CreatedAt,
                    UpdatedAt = fs.UpdatedAt
                })
                .ToListAsync();

            return Ok(new PagedResult<FacilityStaffResponse>
            {
                Items = staff,
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
            _logger.LogError(ex, $"Error fetching facility staff {facilityId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching staff"
            });
        }
    }

    /// <summary>
    /// Update facility staff
    /// </summary>
    [HttpPut("{staffId}")]
    [ProducesResponseType(typeof(FacilityStaffResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateFacilityStaff(Guid facilityId, Guid staffId, [FromBody] UpdateFacilityStaffRequest request)
    {
        try
        {
            var facility = await _context.Facilities.FirstOrDefaultAsync(f => f.Id == facilityId);
            if (facility == null)
                throw new NotFoundException($"Facility with ID {facilityId} not found");

            var facilityStaff = await _context.FacilityStaffs.FirstOrDefaultAsync(fs => fs.Id == staffId && fs.FacilityId == facilityId);
            if (facilityStaff == null)
                throw new NotFoundException($"Staff with ID {staffId} not found");

            if (request.DepartmentId.HasValue)
                facilityStaff.DepartmentId = request.DepartmentId;

            if (!string.IsNullOrEmpty(request.Position))
                facilityStaff.Position = request.Position;

            if (!string.IsNullOrEmpty(request.Specialization))
                facilityStaff.Specialization = request.Specialization;

            if (request.IsActive.HasValue)
                facilityStaff.IsActive = request.IsActive.Value;

            facilityStaff.UpdatedAt = DateTime.UtcNow;

            _context.FacilityStaffs.Update(facilityStaff);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Facility staff updated: {staffId}");

            var response = new FacilityStaffResponse
            {
                Id = facilityStaff.Id,
                FacilityId = facilityStaff.FacilityId,
                StaffId = facilityStaff.StaffId,
                DepartmentId = facilityStaff.DepartmentId,
                Position = facilityStaff.Position,
                Specialization = facilityStaff.Specialization,
                StartDate = facilityStaff.StartDate,
                EndDate = facilityStaff.EndDate,
                IsActive = facilityStaff.IsActive,
                CreatedAt = facilityStaff.CreatedAt,
                UpdatedAt = facilityStaff.UpdatedAt
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
            _logger.LogError(ex, $"Error updating facility staff {staffId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while updating staff"
            });
        }
    }

    /// <summary>
    /// Remove staff from facility
    /// </summary>
    [HttpDelete("{staffId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveStaffFromFacility(Guid facilityId, Guid staffId)
    {
        try
        {
            var facility = await _context.Facilities.FirstOrDefaultAsync(f => f.Id == facilityId);
            if (facility == null)
                throw new NotFoundException($"Facility with ID {facilityId} not found");

            var facilityStaff = await _context.FacilityStaffs.FirstOrDefaultAsync(fs => fs.Id == staffId && fs.FacilityId == facilityId);
            if (facilityStaff == null)
                throw new NotFoundException($"Staff with ID {staffId} not found");

            _context.FacilityStaffs.Remove(facilityStaff);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Staff removed from facility: {staffId}");

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
            _logger.LogError(ex, $"Error removing staff from facility {staffId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while removing staff"
            });
        }
    }
}
