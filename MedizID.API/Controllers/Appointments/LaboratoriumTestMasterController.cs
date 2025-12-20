using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedizID.API.Common;
using MedizID.API.Common.Enums;
using MedizID.API.Common.Exceptions;
using MedizID.API.Data;
using MedizID.API.DTOs;
using MedizID.API.Models;

namespace MedizID.API.Controllers.Appointments;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
[Authorize]
public class LaboratoriumTestMasterController : ControllerBase
{
    private readonly MedizIDDbContext _context;
    private readonly ILogger<LaboratoriumTestMasterController> _logger;

    public LaboratoriumTestMasterController(MedizIDDbContext context, ILogger<LaboratoriumTestMasterController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Get all laboratory test masters with pagination
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<LaboratoriumTestMasterDetailResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLaboratoriumTestMasters(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] bool? isActive = null)
    {
        try
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var query = _context.LaboratoriumTestMasters.AsQueryable();

            if (isActive.HasValue)
                query = query.Where(l => l.IsActive == isActive.Value);

            var total = await query.CountAsync();

            var tests = await query
                .OrderBy(l => l.Category)
                .ThenBy(l => l.SortOrder)
                .ThenBy(l => l.TestName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(l => MapToDetailResponseStatic(l))
                .ToListAsync();

            return Ok(new PagedResult<LaboratoriumTestMasterDetailResponse>
            {
                Items = tests,
                Total = total,
                Page = page,
                PageSize = pageSize
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching laboratory test masters");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching laboratory test masters"
            });
        }
    }

    /// <summary>
    /// Get laboratory test master by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(LaboratoriumTestMasterDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetLaboratoriumTestMaster(Guid id)
    {
        try
        {
            var test = await _context.LaboratoriumTestMasters
                .FirstOrDefaultAsync(l => l.Id == id);

            if (test == null)
            {
                _logger.LogWarning($"Laboratory test master not found: {id}");
                throw new NotFoundException($"Laboratory test master with ID {id} not found");
            }

            return Ok(MapToDetailResponseStatic(test));
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
            _logger.LogError(ex, $"Error fetching laboratory test master {id}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching the laboratory test master"
            });
        }
    }

    /// <summary>
    /// Get laboratory tests by category
    /// </summary>
    [HttpGet("category/{category}")]
    [ProducesResponseType(typeof(List<LaboratoriumTestMasterDetailResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTestsByCategory(LaboratoriumCategoryEnum category)
    {
        try
        {
            var tests = await _context.LaboratoriumTestMasters
                .Where(l => l.Category == category && l.IsActive)
                .OrderBy(l => l.SortOrder)
                .ThenBy(l => l.TestName)
                .Select(l => MapToDetailResponseStatic(l))
                .ToListAsync();

            return Ok(tests);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching tests for category {category}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching tests"
            });
        }
    }

    /// <summary>
    /// Get all tests grouped by category
    /// </summary>
    [HttpGet("grouped/by-category")]
    [ProducesResponseType(typeof(List<LaboratoriumTestsByCategory>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTestsGroupedByCategory()
    {
        try
        {
            var allTests = await _context.LaboratoriumTestMasters
                .Where(l => l.IsActive)
                .OrderBy(l => l.Category)
                .ThenBy(l => l.SortOrder)
                .ThenBy(l => l.TestName)
                .ToListAsync();

            var grouped = allTests
                .GroupBy(l => l.Category)
                .Select(g => new LaboratoriumTestsByCategory
                {
                    Category = g.Key.ToString(),
                    Tests = g.Select(l => new LaboratoriumTestMasterResponse
                    {
                        Id = l.Id,
                        TestName = l.TestName,
                        TestCode = l.TestCode,
                        Category = l.Category.ToString(),
                        Unit = l.Unit,
                        ReferenceRange = l.ReferenceRange,
                        Description = l.Description,
                        SampleType = l.SampleType?.ToString(),
                        SamplePreparation = l.SamplePreparation,
                        IsActive = l.IsActive,
                        SortOrder = l.SortOrder,
                        CreatedAt = l.CreatedAt,
                        UpdatedAt = l.UpdatedAt
                    }).ToList(),
                    Count = g.Count()
                })
                .ToList();

            return Ok(grouped);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching tests grouped by category");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching tests"
            });
        }
    }

    /// <summary>
    /// Create a new laboratory test master
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(LaboratoriumTestMasterDetailResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateLaboratoriumTestMaster([FromBody] CreateLaboratoriumTestMasterRequest request)
    {
        try
        {
            // Validate required fields
            if (string.IsNullOrWhiteSpace(request.TestName))
            {
                return BadRequest(new ErrorResponse
                {
                    ErrorCode = "VALIDATION_ERROR",
                    Message = "Test name is required"
                });
            }

            // Check for duplicates
            var exists = await _context.LaboratoriumTestMasters
                .AnyAsync(l => l.TestName == request.TestName && l.Category == request.Category);

            if (exists)
            {
                return BadRequest(new ErrorResponse
                {
                    ErrorCode = "DUPLICATE_TEST",
                    Message = "This test already exists in the category"
                });
            }

            var test = new LaboratoriumTestMaster
            {
                Id = Guid.NewGuid(),
                TestName = request.TestName.Trim(),
                TestCode = request.TestCode?.Trim(),
                Category = request.Category,
                Unit = request.Unit?.Trim(),
                ReferenceRange = request.ReferenceRange?.Trim(),
                Description = request.Description?.Trim(),
                SampleType = request.SampleType,
                SamplePreparation = request.SamplePreparation?.Trim(),
                SortOrder = request.SortOrder,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.LaboratoriumTestMasters.Add(test);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Laboratory test master created: {test.Id}");

            return CreatedAtAction(nameof(GetLaboratoriumTestMaster), new { id = test.Id }, MapToDetailResponseStatic(test));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating laboratory test master");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while creating the laboratory test master"
            });
        }
    }

    /// <summary>
    /// Update laboratory test master
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(LaboratoriumTestMasterDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateLaboratoriumTestMaster(Guid id, [FromBody] UpdateLaboratoriumTestMasterRequest request)
    {
        try
        {
            var test = await _context.LaboratoriumTestMasters.FirstOrDefaultAsync(l => l.Id == id);
            if (test == null)
            {
                _logger.LogWarning($"Laboratory test master not found: {id}");
                throw new NotFoundException($"Laboratory test master with ID {id} not found");
            }

            if (!string.IsNullOrWhiteSpace(request.TestName))
                test.TestName = request.TestName.Trim();
            if (request.TestCode != null)
                test.TestCode = request.TestCode.Trim();
            if (request.Category.HasValue)
                test.Category = request.Category.Value;
            if (request.Unit != null)
                test.Unit = request.Unit.Trim();
            if (request.ReferenceRange != null)
                test.ReferenceRange = request.ReferenceRange.Trim();
            if (request.Description != null)
                test.Description = request.Description.Trim();
            if (request.SampleType.HasValue)
                test.SampleType = request.SampleType.Value;
            if (request.SamplePreparation != null)
                test.SamplePreparation = request.SamplePreparation.Trim();
            if (request.SortOrder.HasValue)
                test.SortOrder = request.SortOrder.Value;
            if (request.IsActive.HasValue)
                test.IsActive = request.IsActive.Value;

            test.UpdatedAt = DateTime.UtcNow;

            _context.LaboratoriumTestMasters.Update(test);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Laboratory test master updated: {test.Id}");

            return Ok(MapToDetailResponseStatic(test));
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
            _logger.LogError(ex, $"Error updating laboratory test master {id}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while updating the laboratory test master"
            });
        }
    }

    /// <summary>
    /// Delete laboratory test master (soft delete - set IsActive to false)
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteLaboratoriumTestMaster(Guid id)
    {
        try
        {
            var test = await _context.LaboratoriumTestMasters.FirstOrDefaultAsync(l => l.Id == id);
            if (test == null)
            {
                _logger.LogWarning($"Laboratory test master not found: {id}");
                throw new NotFoundException($"Laboratory test master with ID {id} not found");
            }

            // Soft delete
            test.IsActive = false;
            test.UpdatedAt = DateTime.UtcNow;
            _context.LaboratoriumTestMasters.Update(test);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Laboratory test master deleted: {id}");

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
            _logger.LogError(ex, $"Error deleting laboratory test master {id}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while deleting the laboratory test master"
            });
        }
    }

    /// <summary>
    /// Search laboratory test masters
    /// </summary>
    [HttpGet("search")]
    [ProducesResponseType(typeof(PagedResult<LaboratoriumTestMasterDetailResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchLaboratoriumTestMasters(
        [FromQuery] string? query,
        [FromQuery] string? category = null,
        [FromQuery] bool? isActive = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var dbQuery = _context.LaboratoriumTestMasters.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
            {
                var searchQuery = query.Trim().ToLower();
                dbQuery = dbQuery.Where(l =>
                    l.TestName.ToLower().Contains(searchQuery) ||
                    (l.TestCode != null && l.TestCode.ToLower().Contains(searchQuery)) ||
                    (l.Description != null && l.Description.ToLower().Contains(searchQuery))
                );
            }

            if (!string.IsNullOrWhiteSpace(category))
            {
                if (Enum.TryParse<LaboratoriumCategoryEnum>(category, true, out var categoryEnum))
                {
                    dbQuery = dbQuery.Where(l => l.Category == categoryEnum);
                }
            }

            if (isActive.HasValue)
                dbQuery = dbQuery.Where(l => l.IsActive == isActive.Value);

            var total = await dbQuery.CountAsync();

            var tests = await dbQuery
                .OrderBy(l => l.Category)
                .ThenBy(l => l.SortOrder)
                .ThenBy(l => l.TestName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(l => MapToDetailResponseStatic(l))
                .ToListAsync();

            return Ok(new PagedResult<LaboratoriumTestMasterDetailResponse>
            {
                Items = tests,
                Total = total,
                Page = page,
                PageSize = pageSize
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching laboratory test masters");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while searching laboratory test masters"
            });
        }
    }

    // Helper method to map LaboratoriumTestMaster to response
    private static LaboratoriumTestMasterDetailResponse MapToDetailResponseStatic(LaboratoriumTestMaster test)
    {
        return new LaboratoriumTestMasterDetailResponse
        {
            Id = test.Id,
            TestName = test.TestName,
            TestCode = test.TestCode,
            Category = test.Category,
            Unit = test.Unit,
            ReferenceRange = test.ReferenceRange,
            Description = test.Description,
            SampleType = test.SampleType,
            SamplePreparation = test.SamplePreparation,
            IsActive = test.IsActive,
            SortOrder = test.SortOrder,
            CreatedAt = test.CreatedAt,
            UpdatedAt = test.UpdatedAt
        };
    }
}
