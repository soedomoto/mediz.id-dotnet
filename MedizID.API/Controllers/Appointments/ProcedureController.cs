using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedizID.API.Common;
using MedizID.API.Common.Exceptions;
using MedizID.API.Data;
using MedizID.API.DTOs;
using MedizID.API.Models;

namespace MedizID.API.Controllers.Appointments;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
[Authorize]
public class ProcedureController : ControllerBase
{
    private readonly MedizIDDbContext _context;
    private readonly ILogger<ProcedureController> _logger;
    private readonly OpenAISettings _openAISettings;
    private readonly HttpClient _httpClient;

    public ProcedureController(MedizIDDbContext context, ILogger<ProcedureController> logger, OpenAISettings openAISettings, HttpClient httpClient)
    {
        _context = context;
        _logger = logger;
        _openAISettings = openAISettings;
        _httpClient = httpClient;
    }

    /// <summary>
    /// Get all procedures with pagination
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<ProcedureDetailResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProcedures(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] Guid? appointmentId = null)
    {
        try
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var query = _context.Procedures.AsQueryable();

            if (appointmentId.HasValue)
                query = query.Where(p => p.AppointmentId == appointmentId.Value);

            var total = await query.CountAsync();

            var procedures = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ProcedureDetailResponse
                {
                    Id = p.Id,
                    AppointmentId = p.AppointmentId,
                    ICD10PCSCode = p.ICD10PCSCode,
                    ICD10PCSDescription = p.ICD10PCSDescription,
                    ProcedureType = p.Type.ToString(),
                    Notes = p.Notes,
                    PlannedAt = p.PlannedAt,
                    StartedAt = p.StartedAt,
                    FinishedAt = p.FinishedAt,
                    IsRecommendedByAI = p.IsRecommendedByAI,
                    AIRecommendationConfidence = p.AIRecommendationConfidence,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                })
                .ToListAsync();

            return Ok(new PagedResult<ProcedureDetailResponse>
            {
                Items = procedures,
                Total = total,
                Page = page,
                PageSize = pageSize
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching procedures");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching procedures"
            });
        }
    }

    /// <summary>
    /// Get procedure record by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProcedureDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProcedure(Guid id)
    {
        try
        {
            var procedure = await _context.Procedures
                .Include(p => p.Appointment)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (procedure == null)
            {
                _logger.LogWarning($"Procedure record not found: {id}");
                throw new NotFoundException($"Procedure record with ID {id} not found");
            }

            var response = new ProcedureDetailResponse
            {
                Id = procedure.Id,
                AppointmentId = procedure.AppointmentId,
                ICD10PCSCode = procedure.ICD10PCSCode,
                ICD10PCSDescription = procedure.ICD10PCSDescription,
                ProcedureType = procedure.Type.ToString(),
                Notes = procedure.Notes,
                PlannedAt = procedure.PlannedAt,
                StartedAt = procedure.StartedAt,
                FinishedAt = procedure.FinishedAt,
                IsRecommendedByAI = procedure.IsRecommendedByAI,
                AIRecommendationConfidence = procedure.AIRecommendationConfidence,
                CreatedAt = procedure.CreatedAt,
                UpdatedAt = procedure.UpdatedAt
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
            _logger.LogError(ex, $"Error fetching procedure {id}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching the procedure"
            });
        }
    }

    /// <summary>
    /// Get procedures by appointment ID
    /// </summary>
    [HttpGet("appointment/{appointmentId}")]
    [ProducesResponseType(typeof(PagedResult<ProcedureDetailResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProceduresByAppointmentId(
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

            var total = await _context.Procedures
                .Where(p => p.AppointmentId == appointmentId)
                .CountAsync();

            var procedures = await _context.Procedures
                .Where(p => p.AppointmentId == appointmentId)
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ProcedureDetailResponse
                {
                    Id = p.Id,
                    AppointmentId = p.AppointmentId,
                    ICD10PCSCode = p.ICD10PCSCode,
                    ICD10PCSDescription = p.ICD10PCSDescription,
                    ProcedureType = p.Type.ToString(),
                    Notes = p.Notes,
                    PlannedAt = p.PlannedAt,
                    StartedAt = p.StartedAt,
                    FinishedAt = p.FinishedAt,
                    IsRecommendedByAI = p.IsRecommendedByAI,
                    AIRecommendationConfidence = p.AIRecommendationConfidence,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                })
                .ToListAsync();

            return Ok(new PagedResult<ProcedureDetailResponse>
            {
                Items = procedures,
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
            _logger.LogError(ex, $"Error fetching procedures for appointment {appointmentId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching procedures"
            });
        }
    }

    /// <summary>
    /// Create a new procedure
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ProcedureDetailResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateProcedure([FromBody] CreateProcedureRequest request)
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

            // Validate required fields
            if (string.IsNullOrWhiteSpace(request.ICD10PCSCode))
            {
                return BadRequest(new ErrorResponse
                {
                    ErrorCode = "VALIDATION_ERROR",
                    Message = "ICD-10-PCS Code is required"
                });
            }

            if (string.IsNullOrWhiteSpace(request.ICD10PCSDescription))
            {
                return BadRequest(new ErrorResponse
                {
                    ErrorCode = "VALIDATION_ERROR",
                    Message = "ICD-10-PCS Description is required"
                });
            }

            var procedure = new Procedure
            {
                Id = Guid.NewGuid(),
                AppointmentId = request.AppointmentId,
                ICD10PCSCode = request.ICD10PCSCode.ToUpper().Trim(),
                ICD10PCSDescription = request.ICD10PCSDescription.Trim(),
                Type = Enum.Parse<Common.Enums.ProcedureTypeEnum>(request.ProcedureType ?? "Regular"),
                Notes = request.Notes?.Trim(),
                PlannedAt = request.PlannedAt,
                CreatedAt = DateTime.UtcNow
            };

            _context.Procedures.Add(procedure);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Procedure created: {procedure.Id}");

            var response = new ProcedureDetailResponse
            {
                Id = procedure.Id,
                AppointmentId = procedure.AppointmentId,
                ICD10PCSCode = procedure.ICD10PCSCode,
                ICD10PCSDescription = procedure.ICD10PCSDescription,
                ProcedureType = procedure.Type.ToString(),
                Notes = procedure.Notes,
                PlannedAt = procedure.PlannedAt,
                StartedAt = procedure.StartedAt,
                FinishedAt = procedure.FinishedAt,
                IsRecommendedByAI = procedure.IsRecommendedByAI,
                AIRecommendationConfidence = procedure.AIRecommendationConfidence,
                CreatedAt = procedure.CreatedAt,
                UpdatedAt = procedure.UpdatedAt
            };

            return CreatedAtAction(nameof(GetProcedure), new { id = procedure.Id }, response);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid procedure type");
            return BadRequest(new ErrorResponse
            {
                ErrorCode = "VALIDATION_ERROR",
                Message = "Invalid procedure type"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating procedure");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while creating the procedure"
            });
        }
    }

    /// <summary>
    /// Update procedure
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ProcedureDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateProcedure(Guid id, [FromBody] UpdateProcedureRequest request)
    {
        try
        {
            var procedure = await _context.Procedures.FirstOrDefaultAsync(p => p.Id == id);
            if (procedure == null)
            {
                _logger.LogWarning($"Procedure not found: {id}");
                throw new NotFoundException($"Procedure with ID {id} not found");
            }

            // Validate required fields
            if (string.IsNullOrWhiteSpace(request.ICD10PCSCode))
            {
                return BadRequest(new ErrorResponse
                {
                    ErrorCode = "VALIDATION_ERROR",
                    Message = "ICD-10-PCS Code is required"
                });
            }

            if (string.IsNullOrWhiteSpace(request.ICD10PCSDescription))
            {
                return BadRequest(new ErrorResponse
                {
                    ErrorCode = "VALIDATION_ERROR",
                    Message = "ICD-10-PCS Description is required"
                });
            }

            procedure.ICD10PCSCode = request.ICD10PCSCode.ToUpper().Trim();
            procedure.ICD10PCSDescription = request.ICD10PCSDescription.Trim();
            procedure.Type = Enum.Parse<Common.Enums.ProcedureTypeEnum>(request.ProcedureType ?? "Regular");
            procedure.IsRecommendedByAI = false;
            procedure.AIRecommendationConfidence = null;
            procedure.Notes = request.Notes?.Trim();
            procedure.PlannedAt = request.PlannedAt ?? DateTime.UtcNow;
            procedure.StartedAt = request.StartedAt;
            procedure.FinishedAt = request.FinishedAt;
            procedure.UpdatedAt = DateTime.UtcNow;

            _context.Procedures.Update(procedure);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Procedure updated: {procedure.Id}");

            var response = new ProcedureDetailResponse
            {
                Id = procedure.Id,
                AppointmentId = procedure.AppointmentId,
                ICD10PCSCode = procedure.ICD10PCSCode,
                ICD10PCSDescription = procedure.ICD10PCSDescription,
                ProcedureType = procedure.Type.ToString(),
                Notes = procedure.Notes,
                PlannedAt = procedure.PlannedAt,
                StartedAt = procedure.StartedAt,
                FinishedAt = procedure.FinishedAt,
                IsRecommendedByAI = procedure.IsRecommendedByAI,
                AIRecommendationConfidence = procedure.AIRecommendationConfidence,
                CreatedAt = procedure.CreatedAt,
                UpdatedAt = procedure.UpdatedAt
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
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid procedure type");
            return BadRequest(new ErrorResponse
            {
                ErrorCode = "VALIDATION_ERROR",
                Message = "Invalid procedure type"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating procedure {id}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while updating the procedure"
            });
        }
    }

    /// <summary>
    /// Delete procedure
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProcedure(Guid id)
    {
        try
        {
            var procedure = await _context.Procedures.FirstOrDefaultAsync(p => p.Id == id);
            if (procedure == null)
            {
                _logger.LogWarning($"Procedure not found: {id}");
                throw new NotFoundException($"Procedure with ID {id} not found");
            }

            _context.Procedures.Remove(procedure);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Procedure deleted: {id}");

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
            _logger.LogError(ex, $"Error deleting procedure {id}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while deleting the procedure"
            });
        }
    }

    /// <summary>
    /// Batch delete procedures
    /// </summary>
    [HttpDelete]
    [ProducesResponseType(typeof(DeleteResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> BatchDeleteProcedures([FromBody] List<Guid> ids)
    {
        try
        {
            if (ids == null || ids.Count == 0)
            {
                return BadRequest(new ErrorResponse
                {
                    ErrorCode = "VALIDATION_ERROR",
                    Message = "No procedure IDs provided"
                });
            }

            var procedures = await _context.Procedures
                .Where(p => ids.Contains(p.Id))
                .ToListAsync();

            if (procedures.Count == 0)
            {
                return BadRequest(new ErrorResponse
                {
                    ErrorCode = "VALIDATION_ERROR",
                    Message = "No procedures found with the provided IDs"
                });
            }

            _context.Procedures.RemoveRange(procedures);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Batch deleted {procedures.Count} procedures");

            return Ok(new DeleteResponse
            {
                DeletedCount = procedures.Count,
                Message = $"Successfully deleted {procedures.Count} procedure record(s)"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error batch deleting procedures");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while deleting procedures"
            });
        }
    }

    /// <summary>
    /// Generate procedure recommendations using OpenAI based on appointment diagnosis
    /// </summary>
    [HttpPost("{appointmentId}/ai-recommendations")]
    [ProducesResponseType(typeof(List<ProcedureDetailResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GenerateProcedureRecommendations(Guid appointmentId)
    {
        try
        {
            // Validate appointment exists
            var appointment = await _context.Appointments
                .Include(a => a.Diagnoses)
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

            // Get diagnoses for the appointment to inform procedure recommendations
            var diagnoses = await _context.Diagnoses
                .Where(d => d.AppointmentId == appointmentId)
                .ToListAsync();

            if (diagnoses == null || diagnoses.Count == 0)
            {
                _logger.LogWarning($"No diagnoses found for appointment: {appointmentId}");
                return BadRequest(new ErrorResponse
                {
                    ErrorCode = "DIAGNOSES_NOT_FOUND",
                    Message = "No diagnoses found for this appointment. Please create diagnoses first to generate procedure recommendations."
                });
            }

            // Build diagnosis information for context
            var diagnosesBuilder = new System.Text.StringBuilder();
            foreach (var diagnosis in diagnoses)
            {
                diagnosesBuilder.AppendLine($"- {diagnosis.ICD10Code}: {diagnosis.ScientificDescription}");
            }

            var diagnosisText = diagnosesBuilder.ToString();

            _logger.LogInformation($"Generating procedure recommendations for appointment {appointmentId} using OpenAI");

            // Create OpenAI prompt for ICD-10-PCS medical procedures
            var maxRecommendations = 5;
            var prompt = $@"You are an experienced medical procedures specialist. Based on the following diagnoses, generate the top {maxRecommendations} most appropriate medical procedures that should be performed according to ICD-10-PCS standards.

DIAGNOSES:
{diagnosisText}

IMPORTANT INSTRUCTIONS:
1. Respond ONLY with a valid JSON object containing a ""possibleProcedures"" property
2. The ""possibleProcedures"" property MUST be an array of procedure objects
3. The array can be empty if no procedures are recommended, but it MUST be an array, never a single object
4. For each procedure include: icd10pcsCode (string, format like 87.03), scientificDescription (string, detailed medical procedure description), confidenceScore (integer 0-100), and clinicalNotes (string, brief explanation of why this procedure is recommended)
5. Order by confidence score (highest first)
6. Include only realistic ICD-10-PCS procedure codes
7. Do not include any text outside the JSON object
8. Do not include markdown code blocks or formatting
9. ALWAYS wrap the array in a ""possibleProcedures"" property - NEVER return a bare array

Example response format (ALWAYS follow this structure):
{{
  ""possibleProcedures"": [
    {{
      ""icd10pcsCode"": ""87.03"",
      ""scientificDescription"": ""Routine chest x-ray, one view"",
      ""confidenceScore"": 85,
      ""clinicalNotes"": ""Recommended to rule out pneumonia and assess lung status.""
    }},
    {{
      ""icd10pcsCode"": ""88.72"",
      ""scientificDescription"": ""Abdominal ultrasound"",
      ""confidenceScore"": 75,
      ""clinicalNotes"": ""Indicated for abdominal pain evaluation.""
    }}
  ]
}}

Example with single recommendation:
{{
  ""possibleProcedures"": [
    {{
      ""icd10pcsCode"": ""3E0P3ZZ"",
      ""scientificDescription"": ""Introduction of other therapeutic substance into respiratory tract"",
      ""confidenceScore"": 80,
      ""clinicalNotes"": ""Administration of inhaled medications to relieve symptoms.""
    }}
  ]
}}

Example with no recommendations:
{{
  ""possibleProcedures"": []
}}";

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
                        new { role = "system", content = "You are a medical AI assistant specializing in ICD-10-PCS procedure recommendations. Respond ONLY with a valid JSON array. Do not include markdown formatting or code blocks." },
                        new { role = "user", content = prompt }
                    },
                    response_format = new { type = "json_object" },
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
                        return Ok(new List<ProcedureDetailResponse>());
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

                    // Parse the response - handle both array and object with possibleProcedures property
                    List<ProcedureRecommendationItem> recommendations = null;
                    
                    try
                    {
                        // Try to parse as an object with possibleProcedures property
                        var wrappedResponse = JsonSerializer.Deserialize<ProcedureRecommendationResponse>(responseText);
                        if (wrappedResponse?.PossibleProcedures != null)
                        {
                            recommendations = wrappedResponse.PossibleProcedures;
                        }
                        else
                        {
                            // Try to parse directly as an array
                            recommendations = JsonSerializer.Deserialize<List<ProcedureRecommendationItem>>(responseText);
                        }
                    }
                    catch (JsonException)
                    {
                        // Try to parse directly as an array as fallback
                        try
                        {
                            recommendations = JsonSerializer.Deserialize<List<ProcedureRecommendationItem>>(responseText);
                        }
                        catch (JsonException ex)
                        {
                            _logger.LogError(ex, $"Failed to parse OpenAI response: {responseText}");
                            throw;
                        }
                    }

                    if (recommendations == null || recommendations.Count == 0)
                    {
                        _logger.LogWarning("OpenAI returned no procedure recommendations");
                        return Ok(new List<ProcedureDetailResponse>());
                    }

                    // Create procedure records from recommendations
                    var createdProcedures = new List<ProcedureDetailResponse>();

                    foreach (var rec in recommendations.Take(maxRecommendations))
                    {
                        var procedure = new Procedure
                        {
                            Id = Guid.NewGuid(),
                            AppointmentId = appointmentId,
                            ICD10PCSCode = rec.Icd10pcsCode.Trim(),
                            ICD10PCSDescription = rec.ScientificDescription.Trim(),
                            Type = Common.Enums.ProcedureTypeEnum.Diagnostic,
                            IsRecommendedByAI = true,
                            AIRecommendationConfidence = rec.ConfidenceScore,
                            Notes = rec.ClinicalNotes,
                            PlannedAt = DateTime.UtcNow.AddDays(1),
                            CreatedAt = DateTime.UtcNow
                        };

                        _context.Procedures.Add(procedure);

                        createdProcedures.Add(new ProcedureDetailResponse
                        {
                            Id = procedure.Id,
                            AppointmentId = procedure.AppointmentId,
                            ICD10PCSCode = procedure.ICD10PCSCode,
                            ICD10PCSDescription = procedure.ICD10PCSDescription,
                            ProcedureType = procedure.Type.ToString(),
                            Notes = procedure.Notes,
                            PlannedAt = procedure.PlannedAt,
                            StartedAt = procedure.StartedAt,
                            FinishedAt = procedure.FinishedAt,
                            IsRecommendedByAI = procedure.IsRecommendedByAI,
                            AIRecommendationConfidence = procedure.AIRecommendationConfidence,
                            CreatedAt = procedure.CreatedAt,
                            UpdatedAt = procedure.UpdatedAt
                        });
                    }

                    await _context.SaveChangesAsync();

                    _logger.LogInformation($"Successfully created {createdProcedures.Count} procedure recommendations for appointment {appointmentId}");

                    return Ok(createdProcedures);
                }
            }
            catch (JsonException jsonEx)
            {
                _logger.LogError(jsonEx, "Failed to parse OpenAI response as JSON");
                return StatusCode(500, new ErrorResponse
                {
                    ErrorCode = "OPENAI_PARSE_ERROR",
                    Message = "Failed to parse OpenAI response"
                });
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError(httpEx, "OpenAI API request failed");
                return StatusCode(503, new ErrorResponse
                {
                    ErrorCode = "OPENAI_SERVICE_ERROR",
                    Message = "OpenAI service is currently unavailable"
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating procedure recommendations");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while generating procedure recommendations"
            });
        }
    }

    /// <summary>
    /// Search procedures by ICD-10-PCS code or description
    /// </summary>
    [HttpGet("search")]
    [ProducesResponseType(typeof(PagedResult<ProcedureDetailResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchProcedures(
        [FromQuery] string? query,
        [FromQuery] Guid? appointmentId = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var dbQuery = _context.Procedures.AsQueryable();

            if (appointmentId.HasValue)
                dbQuery = dbQuery.Where(p => p.AppointmentId == appointmentId.Value);

            if (!string.IsNullOrWhiteSpace(query))
            {
                var searchQuery = query.Trim().ToLower();
                dbQuery = dbQuery.Where(p =>
                    p.ICD10PCSCode.ToLower().Contains(searchQuery) ||
                    p.ICD10PCSDescription.ToLower().Contains(searchQuery)
                );
            }

            var total = await dbQuery.CountAsync();

            var procedures = await dbQuery
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ProcedureDetailResponse
                {
                    Id = p.Id,
                    AppointmentId = p.AppointmentId,
                    ICD10PCSCode = p.ICD10PCSCode,
                    ICD10PCSDescription = p.ICD10PCSDescription,
                    ProcedureType = p.Type.ToString(),
                    Notes = p.Notes,
                    PlannedAt = p.PlannedAt,
                    StartedAt = p.StartedAt,
                    FinishedAt = p.FinishedAt,
                    IsRecommendedByAI = p.IsRecommendedByAI,
                    AIRecommendationConfidence = p.AIRecommendationConfidence,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                })
                .ToListAsync();

            return Ok(new PagedResult<ProcedureDetailResponse>
            {
                Items = procedures,
                Total = total,
                Page = page,
                PageSize = pageSize
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching procedures");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while searching procedures"
            });
        }
    }
}
