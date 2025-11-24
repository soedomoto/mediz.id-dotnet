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
public class LaboratoriumController : ControllerBase
{
    private readonly MedizIDDbContext _context;
    private readonly ILogger<LaboratoriumController> _logger;

    public LaboratoriumController(MedizIDDbContext context, ILogger<LaboratoriumController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Get all laboratorium tests with pagination
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<LaboratoriumResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTests(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] Guid? medicalRecordId = null)
    {
        try
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var query = _context.LaboratoriumTests.AsQueryable();

            if (medicalRecordId.HasValue)
                query = query.Where(l => l.MedicalRecordId == medicalRecordId.Value);

            var total = await query.CountAsync();

            var tests = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(l => new LaboratoriumResponse
                {
                    Id = l.Id,
                    TestName = l.TestName,
                    Result = l.Result,
                    Unit = l.Unit,
                    Status = l.Status,
                    TestDate = l.TestDate,
                    CreatedAt = l.CreatedAt
                })
                .ToListAsync();

            return Ok(new PagedResult<LaboratoriumResponse>
            {
                Items = tests,
                Total = total,
                Page = page,
                PageSize = pageSize
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching laboratorium tests");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching laboratorium tests"
            });
        }
    }

    /// <summary>
    /// Get laboratorium test by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(LaboratoriumResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTest(Guid id)
    {
        try
        {
            var test = await _context.LaboratoriumTests.FirstOrDefaultAsync(l => l.Id == id);

            if (test == null)
            {
                _logger.LogWarning($"Laboratorium test not found: {id}");
                throw new NotFoundException($"Laboratorium test with ID {id} not found");
            }

            var response = new LaboratoriumResponse
            {
                Id = test.Id,
                TestName = test.TestName,
                Result = test.Result,
                Unit = test.Unit,
                Status = test.Status,
                TestDate = test.TestDate,
                CreatedAt = test.CreatedAt
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
            _logger.LogError(ex, $"Error fetching laboratorium test {id}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching the laboratorium test"
            });
        }
    }

    /// <summary>
    /// Create a new laboratorium test
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(LaboratoriumResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateTest([FromBody] CreateLaboratoriumRequest request)
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

            var test = new Laboratorium
            {
                Id = Guid.NewGuid(),
                MedicalRecordId = request.MedicalRecordId,
                TestName = request.TestName,
                TestCode = request.TestCode,
                Result = request.Result,
                Unit = request.Unit,
                ReferenceRange = request.ReferenceRange,
                Status = request.Status,
                TestDate = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };

            _context.LaboratoriumTests.Add(test);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Laboratorium test created: {test.Id}");

            var response = new LaboratoriumResponse
            {
                Id = test.Id,
                TestName = test.TestName,
                Result = test.Result,
                Unit = test.Unit,
                Status = test.Status,
                TestDate = test.TestDate,
                CreatedAt = test.CreatedAt
            };

            return CreatedAtAction(nameof(GetTest), new { id = test.Id }, response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating laboratorium test");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while creating the laboratorium test"
            });
        }
    }

    /// <summary>
    /// Update laboratorium test
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(LaboratoriumResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateTest(Guid id, [FromBody] CreateLaboratoriumRequest request)
    {
        try
        {
            var test = await _context.LaboratoriumTests.FirstOrDefaultAsync(l => l.Id == id);

            if (test == null)
            {
                throw new NotFoundException($"Laboratorium test with ID {id} not found");
            }

            test.TestName = request.TestName;
            test.TestCode = request.TestCode;
            test.Result = request.Result;
            test.Unit = request.Unit;
            test.ReferenceRange = request.ReferenceRange;
            test.Status = request.Status;

            _context.LaboratoriumTests.Update(test);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Laboratorium test updated: {test.Id}");

            var response = new LaboratoriumResponse
            {
                Id = test.Id,
                TestName = test.TestName,
                Result = test.Result,
                Unit = test.Unit,
                Status = test.Status,
                TestDate = test.TestDate,
                CreatedAt = test.CreatedAt
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
            _logger.LogError(ex, $"Error updating laboratorium test {id}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while updating the laboratorium test"
            });
        }
    }

    /// <summary>
    /// Delete laboratorium test
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTest(Guid id)
    {
        try
        {
            var test = await _context.LaboratoriumTests.FirstOrDefaultAsync(l => l.Id == id);

            if (test == null)
            {
                throw new NotFoundException($"Laboratorium test with ID {id} not found");
            }

            _context.LaboratoriumTests.Remove(test);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Laboratorium test deleted: {id}");

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
            _logger.LogError(ex, $"Error deleting laboratorium test {id}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while deleting the laboratorium test"
            });
        }
    }
}
