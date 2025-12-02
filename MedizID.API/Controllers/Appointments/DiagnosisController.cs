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
public class DiagnosisController : ControllerBase
{
    private readonly MedizIDDbContext _context;
    private readonly ILogger<DiagnosisController> _logger;
    private readonly OpenAISettings _openAISettings;
    private readonly HttpClient _httpClient;

    public DiagnosisController(MedizIDDbContext context, ILogger<DiagnosisController> logger, OpenAISettings openAISettings, HttpClient httpClient)
    {
        _context = context;
        _logger = logger;
        _openAISettings = openAISettings;
        _httpClient = httpClient;
    }

    /// <summary>
    /// Get all diagnoses with pagination
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<DiagnosisDetailResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDiagnoses(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] Guid? appointmentId = null)
    {
        try
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var query = _context.Diagnoses.AsQueryable();

            if (appointmentId.HasValue)
                query = query.Where(d => d.AppointmentId == appointmentId.Value);

            var total = await query.CountAsync();

            var diagnoses = await query
                .OrderByDescending(d => d.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(d => new DiagnosisDetailResponse
                {
                    Id = d.Id,
                    AppointmentId = d.AppointmentId,
                    ICD10Code = d.ICD10Code,
                    ScientificDescription = d.ScientificDescription,
                    DiagnosisType = d.DiagnosisType.ToString(),
                    CaseType = d.CaseType.ToString(),
                    ConfidencePercentage = d.ConfidencePercentage,
                    ClinicalNotes = d.ClinicalNotes,
                    IsRecommendedByAI = d.IsRecommendedByAI,
                    AIRecommendationConfidence = d.AIRecommendationConfidence,
                    CreatedAt = d.CreatedAt,
                    UpdatedAt = d.UpdatedAt
                })
                .ToListAsync();

            return Ok(new PagedResult<DiagnosisDetailResponse>
            {
                Items = diagnoses,
                Total = total,
                Page = page,
                PageSize = pageSize
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching diagnoses");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching diagnoses"
            });
        }
    }

    /// <summary>
    /// Get diagnosis record by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(DiagnosisDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDiagnosis(Guid id)
    {
        try
        {
            var diagnosis = await _context.Diagnoses
                .Include(d => d.Appointment)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (diagnosis == null)
            {
                _logger.LogWarning($"Diagnosis record not found: {id}");
                throw new NotFoundException($"Diagnosis record with ID {id} not found");
            }

            var response = new DiagnosisDetailResponse
            {
                Id = diagnosis.Id,
                AppointmentId = diagnosis.AppointmentId,
                ICD10Code = diagnosis.ICD10Code,
                ScientificDescription = diagnosis.ScientificDescription,
                DiagnosisType = diagnosis.DiagnosisType.ToString(),
                CaseType = diagnosis.CaseType.ToString(),
                ConfidencePercentage = diagnosis.ConfidencePercentage,
                ClinicalNotes = diagnosis.ClinicalNotes,
                IsRecommendedByAI = diagnosis.IsRecommendedByAI,
                AIRecommendationConfidence = diagnosis.AIRecommendationConfidence,
                CreatedAt = diagnosis.CreatedAt,
                UpdatedAt = diagnosis.UpdatedAt
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
            _logger.LogError(ex, $"Error fetching diagnosis {id}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching the diagnosis"
            });
        }
    }

    /// <summary>
    /// Get diagnoses by appointment ID
    /// </summary>
    [HttpGet("appointment/{appointmentId}")]
    [ProducesResponseType(typeof(PagedResult<DiagnosisDetailResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDiagnosesByAppointmentId(
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

            var total = await _context.Diagnoses
                .Where(d => d.AppointmentId == appointmentId)
                .CountAsync();

            var diagnoses = await _context.Diagnoses
                .Where(d => d.AppointmentId == appointmentId)
                .OrderByDescending(d => d.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(d => new DiagnosisDetailResponse
                {
                    Id = d.Id,
                    AppointmentId = d.AppointmentId,
                    ICD10Code = d.ICD10Code,
                    ScientificDescription = d.ScientificDescription,
                    DiagnosisType = d.DiagnosisType.ToString(),
                    CaseType = d.CaseType.ToString(),
                    ConfidencePercentage = d.ConfidencePercentage,
                    ClinicalNotes = d.ClinicalNotes,
                    IsRecommendedByAI = d.IsRecommendedByAI,
                    AIRecommendationConfidence = d.AIRecommendationConfidence,
                    CreatedAt = d.CreatedAt,
                    UpdatedAt = d.UpdatedAt
                })
                .ToListAsync();

            return Ok(new PagedResult<DiagnosisDetailResponse>
            {
                Items = diagnoses,
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
            _logger.LogError(ex, $"Error fetching diagnoses for appointment {appointmentId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching diagnoses"
            });
        }
    }

    /// <summary>
    /// Create a new diagnosis
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(DiagnosisDetailResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateDiagnosis([FromBody] CreateDiagnosisRequest request)
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
            if (string.IsNullOrWhiteSpace(request.ICD10Code))
            {
                return BadRequest(new ErrorResponse
                {
                    ErrorCode = "VALIDATION_ERROR",
                    Message = "ICD-10 Code is required"
                });
            }

            if (string.IsNullOrWhiteSpace(request.ScientificDescription))
            {
                return BadRequest(new ErrorResponse
                {
                    ErrorCode = "VALIDATION_ERROR",
                    Message = "Scientific Description is required"
                });
            }

            var diagnosis = new Diagnosis
            {
                Id = Guid.NewGuid(),
                AppointmentId = request.AppointmentId,
                ICD10Code = request.ICD10Code.ToUpper().Trim(),
                ScientificDescription = request.ScientificDescription.Trim(),
                DiagnosisType = Enum.Parse<Common.Enums.DiagnosisTypeEnum>(request.DiagnosisType ?? "Primary"),
                CaseType = Enum.Parse<Common.Enums.DiagnosisCaseTypeEnum>(request.CaseType ?? "New"),
                ConfidencePercentage = request.ConfidencePercentage,
                ClinicalNotes = request.ClinicalNotes?.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            _context.Diagnoses.Add(diagnosis);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Diagnosis created: {diagnosis.Id}");

            var response = new DiagnosisDetailResponse
            {
                Id = diagnosis.Id,
                AppointmentId = diagnosis.AppointmentId,
                ICD10Code = diagnosis.ICD10Code,
                ScientificDescription = diagnosis.ScientificDescription,
                DiagnosisType = diagnosis.DiagnosisType.ToString(),
                CaseType = diagnosis.CaseType.ToString(),
                ConfidencePercentage = diagnosis.ConfidencePercentage,
                ClinicalNotes = diagnosis.ClinicalNotes,
                IsRecommendedByAI = diagnosis.IsRecommendedByAI,
                AIRecommendationConfidence = diagnosis.AIRecommendationConfidence,
                CreatedAt = diagnosis.CreatedAt,
                UpdatedAt = diagnosis.UpdatedAt
            };

            return CreatedAtAction(nameof(GetDiagnosis), new { id = diagnosis.Id }, response);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid diagnosis type or case type");
            return BadRequest(new ErrorResponse
            {
                ErrorCode = "VALIDATION_ERROR",
                Message = "Invalid diagnosis type or case type"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating diagnosis");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while creating the diagnosis"
            });
        }
    }

    /// <summary>
    /// Update diagnosis
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(DiagnosisDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateDiagnosis(Guid id, [FromBody] CreateDiagnosisRequest request)
    {
        try
        {
            var diagnosis = await _context.Diagnoses.FirstOrDefaultAsync(d => d.Id == id);
            if (diagnosis == null)
            {
                _logger.LogWarning($"Diagnosis not found: {id}");
                throw new NotFoundException($"Diagnosis with ID {id} not found");
            }

            // Validate required fields
            if (string.IsNullOrWhiteSpace(request.ICD10Code))
            {
                return BadRequest(new ErrorResponse
                {
                    ErrorCode = "VALIDATION_ERROR",
                    Message = "ICD-10 Code is required"
                });
            }

            if (string.IsNullOrWhiteSpace(request.ScientificDescription))
            {
                return BadRequest(new ErrorResponse
                {
                    ErrorCode = "VALIDATION_ERROR",
                    Message = "Scientific Description is required"
                });
            }

            diagnosis.ICD10Code = request.ICD10Code.ToUpper().Trim();
            diagnosis.ScientificDescription = request.ScientificDescription.Trim();
            diagnosis.DiagnosisType = Enum.Parse<Common.Enums.DiagnosisTypeEnum>(request.DiagnosisType ?? "Primary");
            diagnosis.CaseType = Enum.Parse<Common.Enums.DiagnosisCaseTypeEnum>(request.CaseType ?? "New");
            diagnosis.IsRecommendedByAI = false;
            diagnosis.ConfidencePercentage = 0;
            diagnosis.ClinicalNotes = request.ClinicalNotes?.Trim();
            diagnosis.UpdatedAt = DateTime.UtcNow;

            _context.Diagnoses.Update(diagnosis);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Diagnosis updated: {diagnosis.Id}");

            var response = new DiagnosisDetailResponse
            {
                Id = diagnosis.Id,
                AppointmentId = diagnosis.AppointmentId,
                ICD10Code = diagnosis.ICD10Code,
                ScientificDescription = diagnosis.ScientificDescription,
                DiagnosisType = diagnosis.DiagnosisType.ToString(),
                CaseType = diagnosis.CaseType.ToString(),
                ConfidencePercentage = diagnosis.ConfidencePercentage,
                ClinicalNotes = diagnosis.ClinicalNotes,
                IsRecommendedByAI = diagnosis.IsRecommendedByAI,
                AIRecommendationConfidence = diagnosis.AIRecommendationConfidence,
                CreatedAt = diagnosis.CreatedAt,
                UpdatedAt = diagnosis.UpdatedAt
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
            _logger.LogWarning(ex, "Invalid diagnosis type or case type");
            return BadRequest(new ErrorResponse
            {
                ErrorCode = "VALIDATION_ERROR",
                Message = "Invalid diagnosis type or case type"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating diagnosis {id}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while updating the diagnosis"
            });
        }
    }

    /// <summary>
    /// Delete diagnosis
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteDiagnosis(Guid id)
    {
        try
        {
            var diagnosis = await _context.Diagnoses.FirstOrDefaultAsync(d => d.Id == id);
            if (diagnosis == null)
            {
                _logger.LogWarning($"Diagnosis not found: {id}");
                throw new NotFoundException($"Diagnosis with ID {id} not found");
            }

            _context.Diagnoses.Remove(diagnosis);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Diagnosis deleted: {id}");

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
            _logger.LogError(ex, $"Error deleting diagnosis {id}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while deleting the diagnosis"
            });
        }
    }

    /// <summary>
    /// Batch delete diagnoses
    /// </summary>
    [HttpDelete]
    [ProducesResponseType(typeof(DeleteResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> BatchDeleteDiagnoses([FromBody] List<Guid> ids)
    {
        try
        {
            if (ids == null || ids.Count == 0)
            {
                return BadRequest(new ErrorResponse
                {
                    ErrorCode = "VALIDATION_ERROR",
                    Message = "No diagnosis IDs provided"
                });
            }

            var diagnoses = await _context.Diagnoses
                .Where(d => ids.Contains(d.Id))
                .ToListAsync();

            if (diagnoses.Count == 0)
            {
                return BadRequest(new ErrorResponse
                {
                    ErrorCode = "VALIDATION_ERROR",
                    Message = "No diagnoses found with the provided IDs"
                });
            }

            _context.Diagnoses.RemoveRange(diagnoses);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Batch deleted {diagnoses.Count} diagnoses");

            return Ok(new DeleteResponse
            {
                DeletedCount = diagnoses.Count,
                Message = $"Successfully deleted {diagnoses.Count} diagnosis record(s)"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error batch deleting diagnoses");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while deleting diagnoses"
            });
        }
    }

    /// <summary>
    /// Generate diagnosis recommendations using OpenAI based on appointment anamnesis
    /// </summary>
    [HttpPost("{appointmentId}/ai-recommendations")]
    [ProducesResponseType(typeof(List<DiagnosisDetailResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GenerateDiagnosisRecommendations(Guid appointmentId)
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

            var symptoms = symptomsBuilder.ToString();
            var clinicalFindings = clinicalFindingsBuilder.ToString();

            _logger.LogInformation($"Generating diagnosis recommendations for appointment {appointmentId} using OpenAI");

            // Create OpenAI prompt
            var maxRecommendations = 5;
            var prompt = $@"You are an experienced medical diagnostician assistant. Based on the following patient information, generate the top {maxRecommendations} most likely diagnoses.

PATIENT SYMPTOMS AND COMPLAINTS:
{symptoms}

CLINICAL FINDINGS AND MEDICAL HISTORY:
{clinicalFindings}

IMPORTANT INSTRUCTIONS:
1. Respond ONLY with a valid JSON array
2. For each diagnosis include: ICD10Code (string, format like A00.0), ScientificDescription (string, detailed medical term), ConfidenceScore (integer 0-100), and ClinicalNotes (string, brief explanation of your reasoning)
3. Order by confidence score (highest first)
4. Include only realistic ICD-10 codes
5. Do not include any text outside the JSON array
6. Do not include markdown code blocks or formatting

Example format of `possibleDiagnoses`:
```json
[
  {{
    ""icd10Code"": ""A00.0"",
    ""scientificDescription"": ""Cholera due to Vibrio cholerae 01, biovar cholerae"",
    ""confidenceScore"": 85,
    ""clinicalNotes"": ""Patient exhibits severe watery diarrhea and dehydration.""
  }},
  {{
    ""icd10Code"": ""A01.0"",
    ""scientificDescription"": ""Typhoid fever"",
    ""confidenceScore"": 45,
    ""clinicalNotes"": ""Patient has prolonged fever and abdominal pain.""
  }}
]
```";

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
                        new { role = "system", content = "You are a medical AI assistant specializing in differential diagnosis. Respond ONLY with a valid JSON array. Do not include markdown formatting or code blocks." },
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
                        return Ok(new List<DiagnosisDetailResponse>());
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

                    // Parse the response - handle both array and object with possibleDiagnoses property
                    List<DiagnosisRecommendationItem> recommendations = null;
                    
                    try
                    {
                        // Try to parse as an object with possibleDiagnoses property
                        var wrappedResponse = JsonSerializer.Deserialize<DiagnosisRecommendationResponse>(responseText);
                        if (wrappedResponse?.PossibleDiagnoses != null)
                        {
                            recommendations = wrappedResponse.PossibleDiagnoses;
                        }
                        else
                        {
                            // Try to parse directly as an array
                            recommendations = JsonSerializer.Deserialize<List<DiagnosisRecommendationItem>>(responseText);
                        }
                    }
                    catch (JsonException)
                    {
                        // Try to parse directly as an array as fallback
                        try
                        {
                            recommendations = JsonSerializer.Deserialize<List<DiagnosisRecommendationItem>>(responseText);
                        }
                        catch (JsonException ex)
                        {
                            _logger.LogError(ex, $"Failed to parse OpenAI response: {responseText}");
                            throw;
                        }
                    }

                    if (recommendations == null || recommendations.Count == 0)
                    {
                        _logger.LogWarning("OpenAI returned no diagnosis recommendations");
                        return Ok(new List<DiagnosisDetailResponse>());
                    }

                    // Create diagnosis records from recommendations
                    var createdDiagnoses = new List<DiagnosisDetailResponse>();

                    foreach (var rec in recommendations.Take(maxRecommendations))
                    {
                        var diagnosis = new Diagnosis
                        {
                            Id = Guid.NewGuid(),
                            AppointmentId = appointmentId,
                            ICD10Code = rec.Icd10Code.ToUpper().Trim(),
                            ScientificDescription = rec.ScientificDescription.Trim(),
                            DiagnosisType = Common.Enums.DiagnosisTypeEnum.Primary,
                            CaseType = Common.Enums.DiagnosisCaseTypeEnum.New,
                            ConfidencePercentage = rec.ConfidenceScore,
                            IsRecommendedByAI = true,
                            AIRecommendationConfidence = rec.ConfidenceScore,
                            ClinicalNotes = rec.ClinicalNotes,
                            CreatedAt = DateTime.UtcNow
                        };

                        _context.Diagnoses.Add(diagnosis);

                        createdDiagnoses.Add(new DiagnosisDetailResponse
                        {
                            Id = diagnosis.Id,
                            AppointmentId = diagnosis.AppointmentId,
                            ICD10Code = diagnosis.ICD10Code,
                            ScientificDescription = diagnosis.ScientificDescription,
                            DiagnosisType = diagnosis.DiagnosisType.ToString(),
                            CaseType = diagnosis.CaseType.ToString(),
                            ConfidencePercentage = diagnosis.ConfidencePercentage,
                            ClinicalNotes = diagnosis.ClinicalNotes,
                            IsRecommendedByAI = diagnosis.IsRecommendedByAI,
                            AIRecommendationConfidence = diagnosis.AIRecommendationConfidence,
                            CreatedAt = diagnosis.CreatedAt,
                            UpdatedAt = diagnosis.UpdatedAt
                        });
                    }

                    await _context.SaveChangesAsync();

                    _logger.LogInformation($"Successfully created {createdDiagnoses.Count} diagnosis recommendations for appointment {appointmentId}");

                    return Ok(createdDiagnoses);
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
            _logger.LogError(ex, "Error generating diagnosis recommendations");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while generating diagnosis recommendations"
            });
        }
    }

    /// <summary>
    /// Search diagnoses by ICD-10 code or description
    /// </summary>
    [HttpGet("search")]
    [ProducesResponseType(typeof(PagedResult<DiagnosisDetailResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchDiagnoses(
        [FromQuery] string? query,
        [FromQuery] Guid? appointmentId = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var dbQuery = _context.Diagnoses.AsQueryable();

            if (appointmentId.HasValue)
                dbQuery = dbQuery.Where(d => d.AppointmentId == appointmentId.Value);

            if (!string.IsNullOrWhiteSpace(query))
            {
                var searchQuery = query.Trim().ToLower();
                dbQuery = dbQuery.Where(d =>
                    d.ICD10Code.ToLower().Contains(searchQuery) ||
                    d.ScientificDescription.ToLower().Contains(searchQuery)
                );
            }

            var total = await dbQuery.CountAsync();

            var diagnoses = await dbQuery
                .OrderByDescending(d => d.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(d => new DiagnosisDetailResponse
                {
                    Id = d.Id,
                    AppointmentId = d.AppointmentId,
                    ICD10Code = d.ICD10Code,
                    ScientificDescription = d.ScientificDescription,
                    DiagnosisType = d.DiagnosisType.ToString(),
                    CaseType = d.CaseType.ToString(),
                    ConfidencePercentage = d.ConfidencePercentage,
                    ClinicalNotes = d.ClinicalNotes,
                    IsRecommendedByAI = d.IsRecommendedByAI,
                    AIRecommendationConfidence = d.AIRecommendationConfidence,
                    CreatedAt = d.CreatedAt,
                    UpdatedAt = d.UpdatedAt
                })
                .ToListAsync();

            return Ok(new PagedResult<DiagnosisDetailResponse>
            {
                Items = diagnoses,
                Total = total,
                Page = page,
                PageSize = pageSize
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching diagnoses");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while searching diagnoses"
            });
        }
    }
}
