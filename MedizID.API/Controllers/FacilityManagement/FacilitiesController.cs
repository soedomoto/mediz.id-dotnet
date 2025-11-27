using MedizID.API.Common.Exceptions;
using MedizID.API.Common.Enums;
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
public class FacilitiesController : ControllerBase
{
    private readonly MedizIDDbContext _context;
    private readonly ILogger<FacilitiesController> _logger;

    public FacilitiesController(MedizIDDbContext context, ILogger<FacilitiesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    #region Base Facility CRUD

    /// <summary>
    /// Create new facility
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(FacilityResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateFacility([FromBody] CreateFacilityRequest request)
    {
        try
        {
            // Parse the Type to FacilityTypeEnum
            if (!Enum.TryParse<FacilityTypeEnum>(request.Type.ToString(), out var facilityType))
            {
                return BadRequest(new ErrorResponse
                {
                    ErrorCode = "INVALID_TYPE",
                    Message = "Invalid facility type. Valid types are: " + string.Join(", ", Enum.GetNames(typeof(FacilityTypeEnum)))
                });
            }

            var facility = new Facility
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Address = request.Address,
                City = request.City,
                Province = request.Province,
                PostalCode = request.PostalCode,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                Type = facilityType,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Facilities.Add(facility);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Facility created: {facility.Id}");

            var response = new FacilityResponse
            {
                Id = facility.Id,
                Name = facility.Name,
                Address = facility.Address,
                City = facility.City,
                Province = facility.Province,
                PostalCode = facility.PostalCode,
                PhoneNumber = facility.PhoneNumber,
                Email = facility.Email,
                Type = facility.Type,
                IsActive = facility.IsActive,
                CreatedAt = facility.CreatedAt
            };

            return CreatedAtAction(nameof(GetFacility), new { facilityId = facility.Id }, response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating facility");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while creating the facility"
            });
        }
    }

    /// <summary>
    /// Get all facilities
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<FacilityResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFacilities([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var query = _context.Facilities.AsQueryable();

            var total = await query.CountAsync();

            var facilities = await query
                .OrderByDescending(f => f.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(f => new FacilityResponse
                {
                    Id = f.Id,
                    Name = f.Name,
                    Address = f.Address,
                    City = f.City,
                    Province = f.Province,
                    PostalCode = f.PostalCode,
                    PhoneNumber = f.PhoneNumber,
                    Email = f.Email,
                    Type = f.Type,
                    IsActive = f.IsActive,
                    CreatedAt = f.CreatedAt
                })
                .ToListAsync();

            return Ok(new PagedResult<FacilityResponse>
            {
                Items = facilities,
                Total = total,
                Page = page,
                PageSize = pageSize
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching facilities");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching facilities"
            });
        }
    }

    /// <summary>
    /// Get facility by ID
    /// </summary>
    [HttpGet("{facilityId}")]
    [ProducesResponseType(typeof(FacilityResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetFacility(Guid facilityId)
    {
        try
        {
            var facility = await _context.Facilities.FirstOrDefaultAsync(f => f.Id == facilityId);
            if (facility == null)
                throw new NotFoundException($"Facility with ID {facilityId} not found");

            var response = new FacilityResponse
            {
                Id = facility.Id,
                Name = facility.Name,
                Address = facility.Address,
                City = facility.City,
                Province = facility.Province,
                PostalCode = facility.PostalCode,
                PhoneNumber = facility.PhoneNumber,
                Email = facility.Email,
                Type = facility.Type,
                IsActive = facility.IsActive,
                CreatedAt = facility.CreatedAt
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
            _logger.LogError(ex, $"Error fetching facility {facilityId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching the facility"
            });
        }
    }

    /// <summary>
    /// Update facility
    /// </summary>
    [HttpPut("{facilityId}")]
    [ProducesResponseType(typeof(FacilityResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateFacility(Guid facilityId, [FromBody] UpdateFacilityRequest request)
    {
        try
        {
            var facility = await _context.Facilities.FirstOrDefaultAsync(f => f.Id == facilityId);
            if (facility == null)
                throw new NotFoundException($"Facility with ID {facilityId} not found");

            if (!string.IsNullOrEmpty(request.Name))
                facility.Name = request.Name;

            if (!string.IsNullOrEmpty(request.Address))
                facility.Address = request.Address;

            if (!string.IsNullOrEmpty(request.City))
                facility.City = request.City;

            if (!string.IsNullOrEmpty(request.Province))
                facility.Province = request.Province;

            if (!string.IsNullOrEmpty(request.PostalCode))
                facility.PostalCode = request.PostalCode;

            if (!string.IsNullOrEmpty(request.PhoneNumber))
                facility.PhoneNumber = request.PhoneNumber;

            if (!string.IsNullOrEmpty(request.Email))
                facility.Email = request.Email;

            if (request.IsActive.HasValue)
                facility.IsActive = request.IsActive.Value;

            facility.UpdatedAt = DateTime.UtcNow;

            _context.Facilities.Update(facility);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Facility updated: {facilityId}");

            var response = new FacilityResponse
            {
                Id = facility.Id,
                Name = facility.Name,
                Address = facility.Address,
                City = facility.City,
                Province = facility.Province,
                PostalCode = facility.PostalCode,
                PhoneNumber = facility.PhoneNumber,
                Email = facility.Email,
                Type = facility.Type,
                IsActive = facility.IsActive,
                CreatedAt = facility.CreatedAt
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
            _logger.LogError(ex, $"Error updating facility {facilityId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while updating the facility"
            });
        }
    }

    /// <summary>
    /// Delete facility
    /// </summary>
    [HttpDelete("{facilityId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteFacility(Guid facilityId)
    {
        try
        {
            var facility = await _context.Facilities.FirstOrDefaultAsync(f => f.Id == facilityId);
            if (facility == null)
                throw new NotFoundException($"Facility with ID {facilityId} not found");

            _context.Facilities.Remove(facility);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Facility deleted: {facilityId}");

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
            _logger.LogError(ex, $"Error deleting facility {facilityId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while deleting the facility"
            });
        }
    }

    #endregion

    #region Departments

    /// <summary>
    /// Create department for facility
    /// </summary>
    [HttpPost("{facilityId}/departments")]
    [ProducesResponseType(typeof(DepartmentResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateDepartmentForFacility(Guid facilityId, [FromBody] CreateDepartmentRequest request)
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

            _logger.LogInformation($"Department created for facility: {department.Id}");

            var response = new DepartmentResponse
            {
                Id = department.Id,
                FacilityId = department.FacilityId,
                Name = department.Name,
                Description = department.Description,
                IsActive = department.IsActive,
                CreatedAt = department.CreatedAt
            };

            return CreatedAtAction(nameof(ListDepartments), new { facilityId }, response);
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
    /// List departments for facility
    /// </summary>
    [HttpGet("{facilityId}/departments")]
    [ProducesResponseType(typeof(PagedResult<DepartmentResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ListDepartments(Guid facilityId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var facility = await _context.Facilities.FirstOrDefaultAsync(f => f.Id == facilityId);
            if (facility == null)
                throw new NotFoundException($"Facility with ID {facilityId} not found");

            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var query = _context.Departments
                .Where(d => d.FacilityId == facilityId)
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
            _logger.LogError(ex, $"Error fetching departments {facilityId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching departments"
            });
        }
    }

    /// <summary>
    /// Update department
    /// </summary>
    [HttpPut("{facilityId}/departments/{departmentId}")]
    [ProducesResponseType(typeof(DepartmentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateDepartment(Guid facilityId, Guid departmentId, [FromBody] UpdateDepartmentRequest request)
    {
        try
        {
            var facility = await _context.Facilities.FirstOrDefaultAsync(f => f.Id == facilityId);
            if (facility == null)
                throw new NotFoundException($"Facility with ID {facilityId} not found");

            var department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == departmentId && d.FacilityId == facilityId);
            if (department == null)
                throw new NotFoundException($"Department with ID {departmentId} not found");

            if (!string.IsNullOrEmpty(request.Name))
                department.Name = request.Name;

            if (!string.IsNullOrEmpty(request.Description))
                department.Description = request.Description;

            if (request.IsActive.HasValue)
                department.IsActive = request.IsActive.Value;

            _context.Departments.Update(department);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Department updated: {departmentId}");

            var response = new DepartmentResponse
            {
                Id = department.Id,
                FacilityId = department.FacilityId,
                Name = department.Name,
                Description = department.Description,
                IsActive = department.IsActive,
                CreatedAt = department.CreatedAt
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
            _logger.LogError(ex, $"Error updating department {departmentId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while updating the department"
            });
        }
    }

    #endregion
}
