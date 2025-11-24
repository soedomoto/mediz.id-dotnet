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
public class DepartmentsController : ControllerBase
{
    private readonly MedizIDDbContext _context;
    private readonly ILogger<DepartmentsController> _logger;

    public DepartmentsController(MedizIDDbContext context, ILogger<DepartmentsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Create a new department
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(DepartmentResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateDepartment([FromBody] CreateDepartmentRequest request, [FromQuery] Guid facilityId)
    {
        try
        {
            var facility = await _context.Facilities.FirstOrDefaultAsync(f => f.Id == facilityId);
            if (facility == null)
                throw new NotFoundException($"Facility with ID {facilityId} not found");

            var department = new Department
            {
                Id = Guid.NewGuid(),
                FacilityId = facilityId,
                Name = request.Name,
                Description = request.Description,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Departments.Add(department);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Department created: {department.Id}");

            var response = new DepartmentResponse
            {
                Id = department.Id,
                FacilityId = department.FacilityId,
                Name = department.Name,
                Description = department.Description,
                IsActive = department.IsActive,
                CreatedAt = department.CreatedAt
            };

            return CreatedAtAction(nameof(GetDepartments), new { facilityId }, response);
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
            _logger.LogError(ex, "Error creating department");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while creating the department"
            });
        }
    }

    /// <summary>
    /// Get all departments for a facility
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<DepartmentResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDepartments([FromQuery] Guid facilityId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var facility = await _context.Facilities.FirstOrDefaultAsync(f => f.Id == facilityId);
            if (facility == null)
                throw new NotFoundException($"Facility with ID {facilityId} not found");

            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var query = _context.Departments
                .Where(d => d.FacilityId == facilityId && d.IsActive)
                .AsQueryable();

            var total = await query.CountAsync();

            var departments = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(d => new DepartmentResponse
                {
                    Id = d.Id,
                    FacilityId = d.FacilityId,
                    Name = d.Name,
                    Description = d.Description,
                    IsActive = d.IsActive,
                    CreatedAt = d.CreatedAt
                })
                .ToListAsync();

            return Ok(new PagedResult<DepartmentResponse>
            {
                Items = departments,
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
            _logger.LogError(ex, $"Error fetching departments for facility {facilityId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching departments"
            });
        }
    }
}
