using System.Text.Json;
using System.Text.Json.Serialization;
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
public class LaboratoriumController : ControllerBase
{
    private readonly MedizIDDbContext _context;
    private readonly ILogger<LaboratoriumController> _logger;
    private readonly OpenAISettings _openAISettings;
    private readonly HttpClient _httpClient;

    public LaboratoriumController(MedizIDDbContext context, ILogger<LaboratoriumController> logger, OpenAISettings openAISettings, HttpClient httpClient)
    {
        _context = context;
        _logger = logger;
        _openAISettings = openAISettings;
        _httpClient = httpClient;
    }

    /// <summary>
    /// Get all laboratory tests with pagination
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<LaboratoriumDetailResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLaboratoriumTests(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] Guid? appointmentId = null)
    {
        try
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var query = _context.LaboratoriumTests
                .Include(l => l.LaboratoriumTestMaster)
                .AsQueryable();

            if (appointmentId.HasValue)
                query = query.Where(l => l.AppointmentId == appointmentId.Value);

            var total = await query.CountAsync();

            var tests = await query
                .OrderByDescending(l => l.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(l => new LaboratoriumDetailResponse
                {
                    Id = l.Id,
                    AppointmentId = l.AppointmentId,
                    LaboratoriumTestMasterId = l.LaboratoriumTestMasterId,
                    TestName = l.LaboratoriumTestMaster.TestName,
                    TestCode = l.LaboratoriumTestMaster.TestCode,
                    Category = l.LaboratoriumTestMaster.Category.ToString(),
                    Unit = l.LaboratoriumTestMaster.Unit,
                    ReferenceRange = l.LaboratoriumTestMaster.ReferenceRange,
                    SampleType = l.LaboratoriumTestMaster.SampleType.ToString(),
                    Result = l.Result,
                    Status = l.Status.ToString(),
                    TestDate = l.TestDate,
                    SampleCollectionDate = l.SampleCollectionDate,
                    SampleCollectionLocation = l.SampleCollectionLocation,
                    LabTechnician = l.LabTechnician,
                    TestNotes = l.TestNotes,
                    IsRecommendedByAI = l.IsRecommendedByAI,
                    AIRecommendationConfidence = l.AIRecommendationConfidence,
                    AIClinicalNotes = l.AIClinicalNotes,
                    CreatedAt = l.CreatedAt,
                    UpdatedAt = l.UpdatedAt
                })
                .ToListAsync();

            return Ok(new PagedResult<LaboratoriumDetailResponse>
            {
                Items = tests,
                Total = total,
                Page = page,
                PageSize = pageSize
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching laboratory tests");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching laboratory tests"
            });
        }
    }

    /// <summary>
    /// Get laboratory test by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(LaboratoriumDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetLaboratoriumTest(Guid id)
    {
        try
        {
            var test = await _context.LaboratoriumTests
                .Include(l => l.LaboratoriumTestMaster)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (test == null)
            {
                _logger.LogWarning($"Laboratory test not found: {id}");
                throw new NotFoundException($"Laboratory test with ID {id} not found");
            }

            var response = new LaboratoriumDetailResponse
            {
                Id = test.Id,
                AppointmentId = test.AppointmentId,
                LaboratoriumTestMasterId = test.LaboratoriumTestMasterId,
                TestName = test.LaboratoriumTestMaster.TestName,
                TestCode = test.LaboratoriumTestMaster.TestCode,
                Category = test.LaboratoriumTestMaster.Category.ToString(),
                Unit = test.LaboratoriumTestMaster.Unit,
                ReferenceRange = test.LaboratoriumTestMaster.ReferenceRange,
                SampleType = test.LaboratoriumTestMaster.SampleType.ToString(),
                Result = test.Result,
                Status = test.Status.ToString(),
                TestDate = test.TestDate,
                SampleCollectionDate = test.SampleCollectionDate,
                SampleCollectionLocation = test.SampleCollectionLocation,
                LabTechnician = test.LabTechnician,
                TestNotes = test.TestNotes,
                IsRecommendedByAI = test.IsRecommendedByAI,
                AIRecommendationConfidence = test.AIRecommendationConfidence,
                AIClinicalNotes = test.AIClinicalNotes,
                CreatedAt = test.CreatedAt,
                UpdatedAt = test.UpdatedAt
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
            _logger.LogError(ex, $"Error fetching laboratory test {id}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching the laboratory test"
            });
        }
    }

    /// <summary>
    /// Get laboratory tests by appointment ID
    /// </summary>
    [HttpGet("appointment/{appointmentId}")]
    [ProducesResponseType(typeof(PagedResult<LaboratoriumDetailResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetLaboratoriumTestsByAppointmentId(
        Guid appointmentId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.Id == appointmentId);
            if (appointment == null)
            {
                _logger.LogWarning($"Appointment not found: {appointmentId}");
                throw new NotFoundException($"Appointment with ID {appointmentId} not found");
            }

            var total = await _context.LaboratoriumTests
                .Where(l => l.AppointmentId == appointmentId)
                .CountAsync();

            var tests = await _context.LaboratoriumTests
                .Where(l => l.AppointmentId == appointmentId)
                .Include(l => l.LaboratoriumTestMaster)
                .OrderByDescending(l => l.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(l => new LaboratoriumDetailResponse
                {
                    Id = l.Id,
                    AppointmentId = l.AppointmentId,
                    LaboratoriumTestMasterId = l.LaboratoriumTestMasterId,
                    TestName = l.LaboratoriumTestMaster.TestName,
                    TestCode = l.LaboratoriumTestMaster.TestCode,
                    Category = l.LaboratoriumTestMaster.Category.ToString(),
                    Unit = l.LaboratoriumTestMaster.Unit,
                    ReferenceRange = l.LaboratoriumTestMaster.ReferenceRange,
                    SampleType = l.LaboratoriumTestMaster.SampleType.ToString(),
                    Result = l.Result,
                    Status = l.Status.ToString(),
                    TestDate = l.TestDate,
                    SampleCollectionDate = l.SampleCollectionDate,
                    SampleCollectionLocation = l.SampleCollectionLocation,
                    LabTechnician = l.LabTechnician,
                    TestNotes = l.TestNotes,
                    IsRecommendedByAI = l.IsRecommendedByAI,
                    AIRecommendationConfidence = l.AIRecommendationConfidence,
                    AIClinicalNotes = l.AIClinicalNotes,
                    CreatedAt = l.CreatedAt,
                    UpdatedAt = l.UpdatedAt
                })
                .ToListAsync();

            return Ok(new PagedResult<LaboratoriumDetailResponse>
            {
                Items = tests,
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
            _logger.LogError(ex, $"Error fetching laboratory tests for appointment {appointmentId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching laboratory tests"
            });
        }
    }

    /// <summary>
    /// Create a new laboratory test result
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(LaboratoriumDetailResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateLaboratoriumTest([FromBody] CreateLaboratoriumRequest request)
    {
        try
        {
            // Validate appointment exists
            var appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.Id == request.AppointmentId);
            if (appointment == null)
            {
                return BadRequest(new ErrorResponse
                {
                    ErrorCode = "APPOINTMENT_NOT_FOUND",
                    Message = "Appointment not found"
                });
            }

            // Validate master test exists
            var masterTest = await _context.LaboratoriumTestMasters
                .FirstOrDefaultAsync(m => m.Id == request.LaboratoriumTestMasterId && m.IsActive);

            if (masterTest == null)
            {
                return BadRequest(new ErrorResponse
                {
                    ErrorCode = "TEST_MASTER_NOT_FOUND",
                    Message = "Laboratory test master record not found or is inactive"
                });
            }

            // Validate status enum
            if (!Enum.TryParse<LaboratoriumStatusEnum>(request.Status ?? "Pending", out var statusEnum))
            {
                return BadRequest(new ErrorResponse
                {
                    ErrorCode = "INVALID_STATUS",
                    Message = "Invalid laboratory status. Valid values: Normal, Abnormal, Critical, Pending, Inconclusive, NotPerformed"
                });
            }

            var test = new Laboratorium
            {
                Id = Guid.NewGuid(),
                AppointmentId = request.AppointmentId,
                LaboratoriumTestMasterId = request.LaboratoriumTestMasterId,
                Result = request.Result?.Trim(),
                Status = statusEnum,
                TestDate = request.TestDate ?? DateTime.UtcNow,
                SampleCollectionDate = request.SampleCollectionDate,
                SampleCollectionLocation = request.SampleCollectionLocation?.Trim(),
                LabTechnician = request.LabTechnician?.Trim(),
                TestNotes = request.TestNotes?.Trim(),
                IsRecommendedByAI = false,
                CreatedAt = DateTime.UtcNow
            };

            _context.LaboratoriumTests.Add(test);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Laboratory test created: {test.Id} for appointment {request.AppointmentId}");

            var response = await MapToDetailResponse(test);
            return CreatedAtAction(nameof(GetLaboratoriumTest), new { id = test.Id }, response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating laboratory test");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while creating the laboratory test"
            });
        }
    }

    /// <summary>
    /// Create multiple laboratory tests (batch)
    /// </summary>
    [HttpPost("batch")]
    [ProducesResponseType(typeof(BatchLaboratoriumResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> BatchCreateLaboratoriumTests([FromBody] BatchCreateLaboratoriumRequest request)
    {
        try
        {
            // Validate appointment exists
            var appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.Id == request.AppointmentId);
            if (appointment == null)
            {
                return BadRequest(new ErrorResponse
                {
                    ErrorCode = "APPOINTMENT_NOT_FOUND",
                    Message = "Appointment not found"
                });
            }

            if (request.Tests == null || request.Tests.Count == 0)
            {
                return BadRequest(new ErrorResponse
                {
                    ErrorCode = "VALIDATION_ERROR",
                    Message = "At least one test is required"
                });
            }

            var createdTests = new List<LaboratoriumDetailResponse>();
            var failedTests = new List<string>();

            foreach (var testItem in request.Tests)
            {
                try
                {
                    // Validate master test exists
                    var masterTest = await _context.LaboratoriumTestMasters
                        .FirstOrDefaultAsync(m => m.Id == testItem.LaboratoriumTestMasterId && m.IsActive);

                    if (masterTest == null)
                    {
                        failedTests.Add($"Test {testItem.LaboratoriumTestMasterId}: Master record not found");
                        continue;
                    }

                    // Parse status
                    if (!Enum.TryParse<LaboratoriumStatusEnum>(testItem.Status ?? "Pending", out var statusEnum))
                        statusEnum = LaboratoriumStatusEnum.Pending;

                    var test = new Laboratorium
                    {
                        Id = Guid.NewGuid(),
                        AppointmentId = request.AppointmentId,
                        LaboratoriumTestMasterId = testItem.LaboratoriumTestMasterId,
                        Result = testItem.Result?.Trim(),
                        Status = statusEnum,
                        TestDate = testItem.TestDate ?? DateTime.UtcNow,
                        SampleCollectionDate = testItem.SampleCollectionDate,
                        SampleCollectionLocation = testItem.SampleCollectionLocation?.Trim(),
                        LabTechnician = testItem.LabTechnician?.Trim(),
                        TestNotes = testItem.TestNotes?.Trim(),
                        IsRecommendedByAI = false,
                        CreatedAt = DateTime.UtcNow
                    };

                    _context.LaboratoriumTests.Add(test);
                    createdTests.Add(await MapToDetailResponse(test));
                }
                catch (Exception ex)
                {
                    failedTests.Add($"Test {testItem.LaboratoriumTestMasterId}: {ex.Message}");
                }
            }

            if (createdTests.Count > 0)
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Batch created {createdTests.Count} laboratory tests for appointment {request.AppointmentId}");
            }

            var response = new BatchLaboratoriumResponse
            {
                CreatedTests = createdTests,
                TotalCreated = createdTests.Count,
                TotalFailed = failedTests.Count,
                FailureMessages = failedTests.Count > 0 ? failedTests : null,
                CreatedAt = DateTime.UtcNow
            };

            return CreatedAtAction(nameof(GetLaboratoriumTests), response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error batch creating laboratory tests");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while creating laboratory tests"
            });
        }
    }

    /// <summary>
    /// Update laboratory test
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(LaboratoriumDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateLaboratoriumTest(Guid id, [FromBody] UpdateLaboratoriumRequest request)
    {
        try
        {
            var test = await _context.LaboratoriumTests
                .Include(l => l.LaboratoriumTestMaster)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (test == null)
            {
                _logger.LogWarning($"Laboratory test not found: {id}");
                throw new NotFoundException($"Laboratory test with ID {id} not found");
            }

            // Update result
            if (!string.IsNullOrWhiteSpace(request.Result))
                test.Result = request.Result.Trim();

            // Update status
            if (!string.IsNullOrWhiteSpace(request.Status))
            {
                if (Enum.TryParse<LaboratoriumStatusEnum>(request.Status, out var statusEnum))
                    test.Status = statusEnum;
                else
                {
                    return BadRequest(new ErrorResponse
                    {
                        ErrorCode = "INVALID_STATUS",
                        Message = "Invalid laboratory status. Valid values: Normal, Abnormal, Critical, Pending, Inconclusive, NotPerformed"
                    });
                }
            }

            // Update sample info
            if (request.SampleCollectionDate.HasValue)
                test.SampleCollectionDate = request.SampleCollectionDate.Value;
            if (!string.IsNullOrWhiteSpace(request.SampleCollectionLocation))
                test.SampleCollectionLocation = request.SampleCollectionLocation.Trim();
            if (!string.IsNullOrWhiteSpace(request.LabTechnician))
                test.LabTechnician = request.LabTechnician.Trim();
            if (!string.IsNullOrWhiteSpace(request.TestNotes))
                test.TestNotes = request.TestNotes.Trim();

            test.UpdatedAt = DateTime.UtcNow;

            _context.LaboratoriumTests.Update(test);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Laboratory test updated: {test.Id}");

            var response = await MapToDetailResponse(test);
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
            _logger.LogError(ex, $"Error updating laboratory test {id}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while updating the laboratory test"
            });
        }
    }

    /// <summary>
    /// Delete laboratory test
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteLaboratoriumTest(Guid id)
    {
        try
        {
            var test = await _context.LaboratoriumTests.FirstOrDefaultAsync(l => l.Id == id);
            if (test == null)
            {
                _logger.LogWarning($"Laboratory test not found: {id}");
                throw new NotFoundException($"Laboratory test with ID {id} not found");
            }

            _context.LaboratoriumTests.Remove(test);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Laboratory test deleted: {id}");

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
            _logger.LogError(ex, $"Error deleting laboratory test {id}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while deleting the laboratory test"
            });
        }
    }

    /// <summary>
    /// Batch delete laboratory tests
    /// </summary>
    [HttpDelete]
    [ProducesResponseType(typeof(DeleteResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> BatchDeleteLaboratoriumTests([FromBody] List<Guid> ids)
    {
        try
        {
            if (ids == null || ids.Count == 0)
            {
                return BadRequest(new ErrorResponse
                {
                    ErrorCode = "VALIDATION_ERROR",
                    Message = "No laboratory test IDs provided"
                });
            }

            var tests = await _context.LaboratoriumTests
                .Where(l => ids.Contains(l.Id))
                .ToListAsync();

            if (tests.Count == 0)
            {
                return BadRequest(new ErrorResponse
                {
                    ErrorCode = "VALIDATION_ERROR",
                    Message = "No laboratory tests found with the provided IDs"
                });
            }

            _context.LaboratoriumTests.RemoveRange(tests);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Batch deleted {tests.Count} laboratory tests");

            return Ok(new DeleteResponse
            {
                DeletedCount = tests.Count,
                Message = $"Successfully deleted {tests.Count} laboratory test(s)"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error batch deleting laboratory tests");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while deleting laboratory tests"
            });
        }
    }

    /// <summary>
    /// Search laboratory tests by name or category
    /// </summary>
    [HttpGet("search")]
    [ProducesResponseType(typeof(PagedResult<LaboratoriumDetailResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchLaboratoriumTests(
        [FromQuery] string? query,
        [FromQuery] string? category = null,
        [FromQuery] Guid? appointmentId = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var dbQuery = _context.LaboratoriumTests
                .Include(l => l.LaboratoriumTestMaster)
                .AsQueryable();

            if (appointmentId.HasValue)
                dbQuery = dbQuery.Where(l => l.AppointmentId == appointmentId.Value);

            if (!string.IsNullOrWhiteSpace(query))
            {
                var searchQuery = query.Trim().ToLower();
                dbQuery = dbQuery.Where(l =>
                    l.LaboratoriumTestMaster.TestName.ToLower().Contains(searchQuery) ||
                    (l.LaboratoriumTestMaster.TestCode != null && l.LaboratoriumTestMaster.TestCode.ToLower().Contains(searchQuery))
                );
            }

            if (!string.IsNullOrWhiteSpace(category))
            {
                var categoryFilter = category.Trim();
                dbQuery = dbQuery.Where(l => l.LaboratoriumTestMaster.Category.ToString().Contains(categoryFilter));
            }

            var total = await dbQuery.CountAsync();

            var tests = await dbQuery
                .OrderByDescending(l => l.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(l => new LaboratoriumDetailResponse
                {
                    Id = l.Id,
                    AppointmentId = l.AppointmentId,
                    LaboratoriumTestMasterId = l.LaboratoriumTestMasterId,
                    TestName = l.LaboratoriumTestMaster.TestName,
                    TestCode = l.LaboratoriumTestMaster.TestCode,
                    Category = l.LaboratoriumTestMaster.Category.ToString(),
                    Unit = l.LaboratoriumTestMaster.Unit,
                    ReferenceRange = l.LaboratoriumTestMaster.ReferenceRange,
                    SampleType = l.LaboratoriumTestMaster.SampleType.ToString(),
                    Result = l.Result,
                    Status = l.Status.ToString(),
                    TestDate = l.TestDate,
                    SampleCollectionDate = l.SampleCollectionDate,
                    SampleCollectionLocation = l.SampleCollectionLocation,
                    LabTechnician = l.LabTechnician,
                    TestNotes = l.TestNotes,
                    IsRecommendedByAI = l.IsRecommendedByAI,
                    AIRecommendationConfidence = l.AIRecommendationConfidence,
                    AIClinicalNotes = l.AIClinicalNotes,
                    CreatedAt = l.CreatedAt,
                    UpdatedAt = l.UpdatedAt
                })
                .ToListAsync();

            return Ok(new PagedResult<LaboratoriumDetailResponse>
            {
                Items = tests,
                Total = total,
                Page = page,
                PageSize = pageSize
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching laboratory tests");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while searching laboratory tests"
            });
        }
    }

    /// <summary>
    /// Get summary of laboratory tests for an appointment
    /// </summary>
    [HttpGet("appointment/{appointmentId}/summary")]
    [ProducesResponseType(typeof(PatientLaboratoriumSummary), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetLaboratoriumTestsSummary(Guid appointmentId)
    {
        try
        {
            var appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.Id == appointmentId);
            if (appointment == null)
            {
                _logger.LogWarning($"Appointment not found: {appointmentId}");
                throw new NotFoundException($"Appointment with ID {appointmentId} not found");
            }

            var tests = await _context.LaboratoriumTests
                .Include(l => l.LaboratoriumTestMaster)
                .Where(l => l.AppointmentId == appointmentId)
                .ToListAsync();

            var testResponses = new List<LaboratoriumResponse>();
            foreach (var test in tests)
            {
                testResponses.Add(new LaboratoriumResponse
                {
                    Id = test.Id,
                    AppointmentId = test.AppointmentId,
                    TestName = test.LaboratoriumTestMaster.TestName,
                    TestCode = test.LaboratoriumTestMaster.TestCode,
                    Category = test.LaboratoriumTestMaster.Category.ToString(),
                    Result = test.Result,
                    Unit = test.LaboratoriumTestMaster.Unit,
                    ReferenceRange = test.LaboratoriumTestMaster.ReferenceRange,
                    Status = test.Status.ToString(),
                    SampleType = test.LaboratoriumTestMaster.SampleType.ToString(),
                    SampleCollectionDate = test.SampleCollectionDate,
                    LabTechnician = test.LabTechnician,
                    TestNotes = test.TestNotes,
                    TestDate = test.TestDate,
                    CreatedAt = test.CreatedAt,
                    UpdatedAt = test.UpdatedAt
                });
            }

            var response = new PatientLaboratoriumSummary
            {
                AppointmentId = appointmentId,
                Tests = testResponses,
                TotalTests = tests.Count,
                NormalResults = tests.Count(t => t.Status == LaboratoriumStatusEnum.Normal),
                AbnormalResults = tests.Count(t => t.Status == LaboratoriumStatusEnum.Abnormal),
                PendingResults = tests.Count(t => t.Status == LaboratoriumStatusEnum.Pending),
                SummaryDate = DateTime.UtcNow
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
            _logger.LogError(ex, $"Error fetching laboratory tests summary for appointment {appointmentId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching laboratory tests summary"
            });
        }
    }

    /// <summary>
    /// Generate laboratory test recommendations using OpenAI based on appointment anamnesis
    /// </summary>
    [HttpPost("{appointmentId}/ai-recommendations")]
    [ProducesResponseType(typeof(List<LaboratoriumDetailResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GenerateLaboratoriumRecommendations(Guid appointmentId)
    {
        try
        {
            // Validate appointment exists
            var appointment = await _context.Appointments
                .Include(a => a.Anamnesis)
                .FirstOrDefaultAsync(a => a.Id == appointmentId);

            if (appointment == null)
            {
                _logger.LogWarning($"Appointment not found: {appointmentId}");
                return NotFound(new ErrorResponse
                {
                    ErrorCode = "APPOINTMENT_NOT_FOUND",
                    Message = "Appointment not found"
                });
            }

            // Get anamnesis for the appointment
            var anamnesis = await _context.Set<Anamnesis>()
                .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);

            if (anamnesis == null)
            {
                _logger.LogWarning($"Anamnesis not found for appointment: {appointmentId}");
                return BadRequest(new ErrorResponse
                {
                    ErrorCode = "ANAMNESIS_NOT_FOUND",
                    Message = "Anamnesis not found for this appointment"
                });
            }

            // Get diagnoses for the appointment if any
            var diagnoses = await _context.Diagnoses
                .Where(d => d.AppointmentId == appointmentId)
                .ToListAsync();

            // Build symptoms and clinical findings from anamnesis
            var symptomsBuilder = new System.Text.StringBuilder();
            symptomsBuilder.AppendLine($"Chief Complaint: {anamnesis.ChiefComplaint ?? "Not specified"}");
            if (!string.IsNullOrWhiteSpace(anamnesis.AdditionalComplaints))
                symptomsBuilder.AppendLine($"Additional Complaints: {anamnesis.AdditionalComplaints}");
            if (!string.IsNullOrWhiteSpace(anamnesis.PresentIllnessHistory))
                symptomsBuilder.AppendLine($"Present Illness History: {anamnesis.PresentIllnessHistory}");

            var clinicalFindingsBuilder = new System.Text.StringBuilder();
            if (!string.IsNullOrWhiteSpace(anamnesis.PastMedicalHistory))
                clinicalFindingsBuilder.AppendLine($"Past Medical History: {anamnesis.PastMedicalHistory}");
            if (!string.IsNullOrWhiteSpace(anamnesis.FamilyHistory))
                clinicalFindingsBuilder.AppendLine($"Family History: {anamnesis.FamilyHistory}");
            if (!string.IsNullOrWhiteSpace(anamnesis.DrugAllergies))
                clinicalFindingsBuilder.AppendLine($"Drug Allergies: {anamnesis.DrugAllergies}");
            if (!string.IsNullOrWhiteSpace(anamnesis.FoodAllergies))
                clinicalFindingsBuilder.AppendLine($"Food Allergies: {anamnesis.FoodAllergies}");

            // Add diagnoses if available
            if (diagnoses.Count > 0)
            {
                clinicalFindingsBuilder.AppendLine();
                clinicalFindingsBuilder.AppendLine("CURRENT DIAGNOSES:");
                foreach (var diagnosis in diagnoses)
                {
                    clinicalFindingsBuilder.AppendLine($"  - {diagnosis.ScientificDescription} ({diagnosis.ICD10Code})");
                    if (!string.IsNullOrWhiteSpace(diagnosis.ClinicalNotes))
                        clinicalFindingsBuilder.AppendLine($"    Notes: {diagnosis.ClinicalNotes}");
                }
            }

            var symptoms = symptomsBuilder.ToString();
            var clinicalFindings = clinicalFindingsBuilder.ToString();

            _logger.LogInformation($"Generating laboratory test recommendations for appointment {appointmentId} using OpenAI");

            // Get all available master tests organized by category
            var masterTests = await _context.LaboratoriumTestMasters
                .Where(m => m.IsActive)
                .OrderBy(m => m.Category)
                .ThenBy(m => m.TestName)
                .ToListAsync();

            if (masterTests.Count == 0)
            {
                _logger.LogWarning("No active laboratory test masters available for recommendations");
                return BadRequest(new ErrorResponse
                {
                    ErrorCode = "NO_TESTS_AVAILABLE",
                    Message = "No laboratory tests available for recommendations"
                });
            }

            // Build list of available tests grouped by category
            var availableTestsBuilder = new System.Text.StringBuilder();
            availableTestsBuilder.AppendLine("AVAILABLE LABORATORY TESTS IN DATABASE:");
            availableTestsBuilder.AppendLine();
            
            var groupedByCategory = masterTests.GroupBy(m => m.Category);
            foreach (var categoryGroup in groupedByCategory)
            {
                availableTestsBuilder.AppendLine($"{categoryGroup.Key}:");
                foreach (var test in categoryGroup)
                {
                    availableTestsBuilder.AppendLine($"  - ID:{test.Id} Name:{test.TestName} Unit:{test.Unit}");
                }
                availableTestsBuilder.AppendLine();
            }

            var availableTestsList = availableTestsBuilder.ToString();

            // Create OpenAI prompt
            var maxRecommendations = 10;
            var prompt = $@"You are an experienced clinical laboratory diagnostician. Based on the following patient information, recommend the most relevant laboratory tests from the AVAILABLE TESTS LIST. Return ONLY a JSON array with test recommendations.

PATIENT SYMPTOMS AND COMPLAINTS:
{symptoms}

CLINICAL FINDINGS AND MEDICAL HISTORY:
{clinicalFindings}

{availableTestsList}

IMPORTANT INSTRUCTIONS:
1. Respond ONLY with a valid JSON array, no markdown formatting, no code blocks
2. ONLY recommend tests that appear in the AVAILABLE LABORATORY TESTS IN DATABASE list above
3. Use the EXACT test ID and name from the database list
4. For each test include: testId (string, UUID from database ID), testName (string, EXACT name from database), category (string from the list), confidenceScore (integer 0-100), and clinicalNotes (string, brief explanation)
5. Order by confidence score (highest first)
6. If diagnoses are present, prioritize tests that would help confirm, monitor, or rule out those conditions
7. Include only realistic and clinically appropriate laboratory tests that are actually available
8. Do not include any text outside the JSON array
9. Do not recommend tests that are NOT in the available tests list

Example format:
[
  {{
    ""testId"": ""550e8400-e29b-41d4-a716-446655440000"",
    ""testName"": ""Hemoglobin"",
    ""category"": ""Hematologi"",
    ""confidenceScore"": 90,
    ""clinicalNotes"": ""To assess for anemia given patient's reported fatigue.""
  }},
  {{
    ""testId"": ""550e8400-e29b-41d4-a716-446655440001"",
    ""testName"": ""Hematokrit"",
    ""category"": ""Hematologi"",
    ""confidenceScore"": 85,
    ""clinicalNotes"": ""To evaluate red blood cell volume as part of CBC.""
  }}
]";

            try
            {
                // Validate OpenAI API key
                if (string.IsNullOrWhiteSpace(_openAISettings.ApiKey))
                {
                    _logger.LogError("OpenAI API key is not configured");
                    return StatusCode(500, new ErrorResponse
                    {
                        ErrorCode = "OPENAI_NOT_CONFIGURED",
                        Message = "OpenAI service is not properly configured"
                    });
                }

                // Create OpenAI API request
                var requestContent = new
                {
                    model = _openAISettings.ModelName,
                    messages = new[]
                    {
                        new { role = "system", content = "You are a clinical laboratory diagnostician AI. Respond ONLY with a valid JSON array. Do not include markdown formatting or code blocks." },
                        new { role = "user", content = prompt }
                    },
                    temperature = 0.7,
                    max_completion_tokens = 2048,
                    top_p = 1,
                    frequency_penalty = 0,
                    presence_penalty = 0,
                };

                using (var httpRequest = new HttpRequestMessage(HttpMethod.Post, new Uri(new Uri(_openAISettings.ApiBaseUrl), "chat/completions").ToString()))
                {
                    httpRequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _openAISettings.ApiKey);
                    httpRequest.Content = new StringContent(
                        JsonSerializer.Serialize(requestContent),
                        System.Text.Encoding.UTF8,
                        "application/json"
                    );

                    var response = await _httpClient.SendAsync(httpRequest);

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        _logger.LogError($"OpenAI API error: {response.StatusCode} - {errorContent}");
                        return StatusCode(503, new ErrorResponse
                        {
                            ErrorCode = "OPENAI_SERVICE_ERROR",
                            Message = "OpenAI service returned an error"
                        });
                    }

                    var responseContent = await response.Content.ReadAsStringAsync();
                    var openAiResponse = JsonSerializer.Deserialize<OpenAIResponse>(responseContent);

                    if (openAiResponse?.Choices == null || openAiResponse.Choices.Count == 0)
                    {
                        _logger.LogWarning("OpenAI returned no choices");
                        return Ok(new List<LaboratoriumDetailResponse>());
                    }

                    var responseText = openAiResponse.Choices[0].Message.Content.Trim();

                    // Clean the response if it contains markdown code blocks
                    if (responseText.StartsWith("```json"))
                        responseText = responseText[7..];
                    if (responseText.StartsWith("```"))
                        responseText = responseText[3..];
                    if (responseText.EndsWith("```"))
                        responseText = responseText[..^3];
                    responseText = responseText.Trim();

                    _logger.LogInformation($"OpenAI response received: {responseText}");

                    // Parse the response
                    List<LaboratoriumRecommendationItem>? recommendations = null;

                    try
                    {
                        recommendations = JsonSerializer.Deserialize<List<LaboratoriumRecommendationItem>>(responseText);
                    }
                    catch (JsonException jsonEx)
                    {
                        _logger.LogError($"Failed to parse OpenAI response: {jsonEx.Message}");
                    }

                    if (recommendations == null || recommendations.Count == 0)
                    {
                        _logger.LogWarning("No valid recommendations found in OpenAI response");
                        return Ok(new List<LaboratoriumDetailResponse>());
                    }

                    var createdTests = new List<LaboratoriumDetailResponse>();

                    foreach (var rec in recommendations.Take(maxRecommendations))
                    {
                        if (string.IsNullOrWhiteSpace(rec.TestId) && string.IsNullOrWhiteSpace(rec.TestName))
                            continue;

                        // Find matching master test by ID first (most reliable)
                        LaboratoriumTestMaster? masterTest = null;
                        
                        if (!string.IsNullOrWhiteSpace(rec.TestId) && Guid.TryParse(rec.TestId, out var testId))
                        {
                            masterTest = masterTests.FirstOrDefault(m => m.Id == testId);
                        }

                        // Fallback to name matching if ID not provided
                        if (masterTest == null && !string.IsNullOrWhiteSpace(rec.TestName))
                        {
                            masterTest = masterTests.FirstOrDefault(m => m.TestName == rec.TestName);
                        }

                        if (masterTest == null)
                        {
                            _logger.LogWarning($"AI recommended test not found in master list - ID: {rec.TestId}, Name: {rec.TestName}");
                            continue;
                        }

                        _logger.LogInformation($"Creating AI-recommended laboratory test: {masterTest.TestName} (ID: {masterTest.Id})");

                        var test = new Laboratorium
                        {
                            Id = Guid.NewGuid(),
                            AppointmentId = appointmentId,
                            LaboratoriumTestMasterId = masterTest.Id,
                            Status = LaboratoriumStatusEnum.Pending,
                            AIClinicalNotes = rec.ClinicalNotes?.Trim(),
                            IsRecommendedByAI = true,
                            AIRecommendationConfidence = rec.ConfidenceScore,
                            TestDate = DateTime.UtcNow,
                            CreatedAt = DateTime.UtcNow
                        };

                        _context.LaboratoriumTests.Add(test);
                        createdTests.Add(await MapToDetailResponse(test));
                    }

                    if (createdTests.Count > 0)
                    {
                        await _context.SaveChangesAsync();
                        _logger.LogInformation($"Created {createdTests.Count} AI-recommended laboratory tests for appointment {appointmentId}");
                    }

                    return Ok(createdTests);
                }
            }
            catch (JsonException jsonEx)
            {
                _logger.LogError($"JSON parsing error: {jsonEx.Message}");
                return StatusCode(500, new ErrorResponse
                {
                    ErrorCode = "JSON_PARSING_ERROR",
                    Message = "Failed to parse OpenAI response"
                });
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError($"HTTP request error: {httpEx.Message}");
                return StatusCode(503, new ErrorResponse
                {
                    ErrorCode = "OPENAI_REQUEST_ERROR",
                    Message = "Failed to communicate with OpenAI service"
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating laboratory test recommendations");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while generating laboratory test recommendations"
            });
        }
    }

    // Helper method to map Laboratorium to LaboratoriumDetailResponse
    private async Task<LaboratoriumDetailResponse> MapToDetailResponse(Laboratorium test)
    {
        await _context.Entry(test)
            .Reference(l => l.LaboratoriumTestMaster)
            .LoadAsync();

        return new LaboratoriumDetailResponse
        {
            Id = test.Id,
            AppointmentId = test.AppointmentId,
            LaboratoriumTestMasterId = test.LaboratoriumTestMasterId,
            TestName = test.LaboratoriumTestMaster.TestName,
            TestCode = test.LaboratoriumTestMaster.TestCode,
            Category = test.LaboratoriumTestMaster.Category.ToString(),
            Unit = test.LaboratoriumTestMaster.Unit,
            ReferenceRange = test.LaboratoriumTestMaster.ReferenceRange,
            SampleType = test.LaboratoriumTestMaster.SampleType.ToString(),
            Result = test.Result,
            Status = test.Status.ToString(),
            TestDate = test.TestDate,
            SampleCollectionDate = test.SampleCollectionDate,
            SampleCollectionLocation = test.SampleCollectionLocation,
            LabTechnician = test.LabTechnician,
            TestNotes = test.TestNotes,
            IsRecommendedByAI = test.IsRecommendedByAI,
            AIRecommendationConfidence = test.AIRecommendationConfidence,
            AIClinicalNotes = test.AIClinicalNotes,
            CreatedAt = test.CreatedAt,
            UpdatedAt = test.UpdatedAt
        };
    }
}

// Helper class for OpenAI response parsing
internal class OpenAIResponse
{
    [JsonPropertyName("choices")]
    public List<OpenAIChoice> Choices { get; set; } = new();
}

internal class OpenAIChoice
{
    [JsonPropertyName("message")]
    public OpenAIMessage Message { get; set; } = new();
}

internal class OpenAIMessage
{
    [JsonPropertyName("content")]
    public string Content { get; set; } = string.Empty;
}
