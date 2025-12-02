using MedizID.API.Common;
using MedizID.API.Common.Exceptions;
using MedizID.API.Data;
using MedizID.API.DTOs;
using MedizID.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace MedizID.API.Controllers.Appointments;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
[Authorize]
public class PrescriptionsController : ControllerBase
{
    private readonly MedizIDDbContext _context;
    private readonly ILogger<PrescriptionsController> _logger;
    private readonly OpenAISettings _openAISettings;
    private readonly HttpClient _httpClient;

    public PrescriptionsController(
        MedizIDDbContext context, 
        ILogger<PrescriptionsController> logger,
        OpenAISettings openAISettings,
        HttpClient httpClient)
    {
        _context = context;
        _logger = logger;
        _openAISettings = openAISettings;
        _httpClient = httpClient;
    }

    /// <summary>
    /// Get all prescriptions with pagination
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<PrescriptionResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPrescriptions(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] Guid? appointmentId = null)
    {
        try
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var query = _context.Prescriptions
                .Include(p => p.PrescriptionDetails)
                .AsQueryable();

            if (appointmentId.HasValue)
                query = query.Where(p => p.AppointmentId == appointmentId.Value);

            var total = await query.CountAsync();

            var prescriptions = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new PrescriptionResponse
                {
                    Id = p.Id,
                    AppointmentId = p.AppointmentId,
                    IsRecommendedByAI = p.IsRecommendedByAI,
                    AIRecommendationConfidence = p.AIRecommendationConfidence,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,
                    Details = p.PrescriptionDetails.Select(pd => new PrescriptionDetailResponse
                    {
                        Id = pd.Id,
                        PrescriptionId = pd.PrescriptionId,
                        DrugId = pd.DrugId,
                        MedicalEquipmentId = pd.MedicalEquipmentId,
                        MedicationName = pd.MedicationName,
                        Dosage = pd.Dosage,
                        Signa = pd.Signa,
                        Frequency = pd.Frequency,
                        Quantity = pd.Quantity,
                        Instructions = pd.Instructions,
                        Notes = pd.Notes,
                        Price = pd.Price,
                        Packaging = pd.Packaging,
                        RecipeType = pd.RecipeType,
                        RequestedQuantity = pd.RequestedQuantity,
                        CreatedAt = pd.CreatedAt,
                        UpdatedAt = pd.UpdatedAt
                    }).ToList()
                })
                .ToListAsync();

            return Ok(new PagedResult<PrescriptionResponse>
            {
                Items = prescriptions,
                Total = total,
                Page = page,
                PageSize = pageSize
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching prescriptions");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching prescriptions"
            });
        }
    }

    /// <summary>
    /// Get prescription by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PrescriptionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPrescription(Guid id)
    {
        try
        {
            var prescription = await _context.Prescriptions
                .Include(p => p.PrescriptionDetails)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (prescription == null)
            {
                _logger.LogWarning($"Prescription not found: {id}");
                throw new NotFoundException($"Prescription with ID {id} not found");
            }

            var response = new PrescriptionResponse
            {
                Id = prescription.Id,
                AppointmentId = prescription.AppointmentId,
                IsRecommendedByAI = prescription.IsRecommendedByAI,
                AIRecommendationConfidence = prescription.AIRecommendationConfidence,
                CreatedAt = prescription.CreatedAt,
                UpdatedAt = prescription.UpdatedAt,
                Details = prescription.PrescriptionDetails.Select(pd => new PrescriptionDetailResponse
                {
                    Id = pd.Id,
                    PrescriptionId = pd.PrescriptionId,
                    DrugId = pd.DrugId,
                    MedicalEquipmentId = pd.MedicalEquipmentId,
                    MedicationName = pd.MedicationName,
                    Dosage = pd.Dosage,
                    Signa = pd.Signa,
                    Frequency = pd.Frequency,
                    Quantity = pd.Quantity,
                    Instructions = pd.Instructions,
                    Notes = pd.Notes,
                    Price = pd.Price,
                    Packaging = pd.Packaging,
                    RecipeType = pd.RecipeType,
                    RequestedQuantity = pd.RequestedQuantity,
                    CreatedAt = pd.CreatedAt,
                    UpdatedAt = pd.UpdatedAt
                }).ToList()
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
            _logger.LogError(ex, $"Error fetching prescription {id}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching the prescription"
            });
        }
    }

    /// <summary>
    /// Get prescriptions by appointment ID
    /// </summary>
    [HttpGet("appointment/{appointmentId}")]
    [ProducesResponseType(typeof(PagedResult<PrescriptionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPrescriptionsByAppointmentId(
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

            var total = await _context.Prescriptions
                .Where(p => p.AppointmentId == appointmentId)
                .CountAsync();

            var prescriptions = await _context.Prescriptions
                .Where(p => p.AppointmentId == appointmentId)
                .Include(p => p.PrescriptionDetails)
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new PrescriptionResponse
                {
                    Id = p.Id,
                    AppointmentId = p.AppointmentId,
                    IsRecommendedByAI = p.IsRecommendedByAI,
                    AIRecommendationConfidence = p.AIRecommendationConfidence,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,
                    Details = p.PrescriptionDetails.Select(pd => new PrescriptionDetailResponse
                    {
                        Id = pd.Id,
                        PrescriptionId = pd.PrescriptionId,
                        DrugId = pd.DrugId,
                        MedicalEquipmentId = pd.MedicalEquipmentId,
                        MedicationName = pd.MedicationName,
                        Dosage = pd.Dosage,
                        Signa = pd.Signa,
                        Frequency = pd.Frequency,
                        Quantity = pd.Quantity,
                        Instructions = pd.Instructions,
                        Notes = pd.Notes,
                        Price = pd.Price,
                        Packaging = pd.Packaging,
                        RecipeType = pd.RecipeType,
                        RequestedQuantity = pd.RequestedQuantity,
                        CreatedAt = pd.CreatedAt,
                        UpdatedAt = pd.UpdatedAt
                    }).ToList()
                })
                .ToListAsync();

            return Ok(new PagedResult<PrescriptionResponse>
            {
                Items = prescriptions,
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
            _logger.LogError(ex, $"Error fetching prescriptions for appointment {appointmentId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching prescriptions"
            });
        }
    }

    /// <summary>
    /// Create a new prescription
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(PrescriptionResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreatePrescription([FromBody] CreatePrescriptionRequest request)
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

            var prescription = new Prescription
            {
                Id = Guid.NewGuid(),
                AppointmentId = request.AppointmentId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Prescriptions.Add(prescription);

            // Add prescription details
            foreach (var detail in request.Details)
            {
                var prescriptionDetail = new PrescriptionDetail
                {
                    Id = Guid.NewGuid(),
                    PrescriptionId = prescription.Id,
                    DrugId = detail.DrugId,
                    MedicalEquipmentId = detail.MedicalEquipmentId,
                    MedicationName = detail.MedicationName,
                    Dosage = detail.Dosage,
                    Signa = detail.Signa,
                    Frequency = detail.Frequency,
                    Quantity = detail.Quantity,
                    Instructions = detail.Instructions,
                    Notes = detail.Notes,
                    Price = detail.Price,
                    Packaging = detail.Packaging,
                    RecipeType = detail.RecipeType,
                    RequestedQuantity = detail.RequestedQuantity,
                    CreatedAt = DateTime.UtcNow
                };

                _context.PrescriptionDetails.Add(prescriptionDetail);
            }

            await _context.SaveChangesAsync();

            _logger.LogInformation($"Prescription created: {prescription.Id}");

            var response = new PrescriptionResponse
            {
                Id = prescription.Id,
                AppointmentId = prescription.AppointmentId,
                IsRecommendedByAI = prescription.IsRecommendedByAI,
                AIRecommendationConfidence = prescription.AIRecommendationConfidence,
                CreatedAt = prescription.CreatedAt,
                UpdatedAt = prescription.UpdatedAt,
                Details = request.Details.Select((d, i) => new PrescriptionDetailResponse
                {
                    MedicationName = d.MedicationName,
                    Dosage = d.Dosage,
                    Signa = d.Signa,
                    Frequency = d.Frequency,
                    Quantity = d.Quantity,
                    Instructions = d.Instructions,
                    Notes = d.Notes,
                    Price = d.Price,
                    Packaging = d.Packaging,
                    RecipeType = d.RecipeType,
                    RequestedQuantity = d.RequestedQuantity
                }).ToList()
            };

            return CreatedAtAction(nameof(GetPrescription), new { id = prescription.Id }, response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating prescription");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while creating the prescription"
            });
        }
    }

    /// <summary>
    /// Update prescription
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(PrescriptionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdatePrescription(Guid id, [FromBody] CreatePrescriptionRequest request)
    {
        try
        {
            var prescription = await _context.Prescriptions
                .Include(p => p.PrescriptionDetails)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (prescription == null)
            {
                throw new NotFoundException($"Prescription with ID {id} not found");
            }

            // Remove existing details
            _context.PrescriptionDetails.RemoveRange(prescription.PrescriptionDetails);

            // Add new details
            foreach (var detail in request.Details)
            {
                var prescriptionDetail = new PrescriptionDetail
                {
                    Id = Guid.NewGuid(),
                    PrescriptionId = prescription.Id,
                    DrugId = detail.DrugId,
                    MedicalEquipmentId = detail.MedicalEquipmentId,
                    MedicationName = detail.MedicationName,
                    Dosage = detail.Dosage,
                    Signa = detail.Signa,
                    Frequency = detail.Frequency,
                    Quantity = detail.Quantity,
                    Instructions = detail.Instructions,
                    Notes = detail.Notes,
                    Price = detail.Price,
                    Packaging = detail.Packaging,
                    RecipeType = detail.RecipeType,
                    RequestedQuantity = detail.RequestedQuantity,
                    CreatedAt = DateTime.UtcNow
                };

                _context.PrescriptionDetails.Add(prescriptionDetail);
            }

            prescription.UpdatedAt = DateTime.UtcNow;
            _context.Prescriptions.Update(prescription);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Prescription updated: {prescription.Id}");

            var response = new PrescriptionResponse
            {
                Id = prescription.Id,
                AppointmentId = prescription.AppointmentId,
                IsRecommendedByAI = prescription.IsRecommendedByAI,
                AIRecommendationConfidence = prescription.AIRecommendationConfidence,
                CreatedAt = prescription.CreatedAt,
                UpdatedAt = prescription.UpdatedAt,
                Details = request.Details.Select(d => new PrescriptionDetailResponse
                {
                    MedicationName = d.MedicationName,
                    Dosage = d.Dosage,
                    Signa = d.Signa,
                    Frequency = d.Frequency,
                    Quantity = d.Quantity,
                    Instructions = d.Instructions,
                    Notes = d.Notes,
                    Price = d.Price,
                    Packaging = d.Packaging,
                    RecipeType = d.RecipeType,
                    RequestedQuantity = d.RequestedQuantity
                }).ToList()
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
            _logger.LogError(ex, $"Error updating prescription {id}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while updating the prescription"
            });
        }
    }

    /// <summary>
    /// Delete prescription
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeletePrescription(Guid id)
    {
        try
        {
            var prescription = await _context.Prescriptions
                .Include(p => p.PrescriptionDetails)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (prescription == null)
            {
                throw new NotFoundException($"Prescription with ID {id} not found");
            }

            _context.Prescriptions.Remove(prescription);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Prescription deleted: {id}");

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
            _logger.LogError(ex, $"Error deleting prescription {id}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while deleting the prescription"
            });
        }
    }

    /// <summary>
    /// Batch delete prescriptions
    /// </summary>
    [HttpDelete]
    [ProducesResponseType(typeof(DeleteResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> BatchDeletePrescriptions([FromBody] List<Guid> ids)
    {
        try
        {
            if (ids == null || ids.Count == 0)
            {
                return BadRequest(new ErrorResponse
                {
                    ErrorCode = "VALIDATION_ERROR",
                    Message = "No prescription IDs provided"
                });
            }

            var prescriptions = await _context.Prescriptions
                .Where(p => ids.Contains(p.Id))
                .ToListAsync();

            if (prescriptions.Count == 0)
            {
                return BadRequest(new ErrorResponse
                {
                    ErrorCode = "VALIDATION_ERROR",
                    Message = "No prescriptions found with the provided IDs"
                });
            }

            _context.Prescriptions.RemoveRange(prescriptions);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Batch deleted {prescriptions.Count} prescriptions");

            return Ok(new DeleteResponse
            {
                DeletedCount = prescriptions.Count,
                Message = $"Successfully deleted {prescriptions.Count} prescription record(s)"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error batch deleting prescriptions");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while deleting prescriptions"
            });
        }
    }

    /// <summary>
    /// Generate prescription recommendations using OpenAI based on appointment diagnoses and anamnesis
    /// </summary>
    [HttpPost("{appointmentId}/ai-recommendations")]
    [ProducesResponseType(typeof(PrescriptionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GeneratePrescriptionRecommendations(Guid appointmentId)
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

            // Get patient information
            var facilityPatient = await _context.Set<FacilityPatient>()
                .Include(fp => fp.Patient)
                .FirstOrDefaultAsync(fp => fp.Id == appointment.FacilityPatientId);

            if (facilityPatient?.Patient == null)
            {
                _logger.LogWarning($"Patient information not found for appointment: {appointmentId}");
                return BadRequest(new ErrorResponse
                {
                    ErrorCode = "PATIENT_NOT_FOUND",
                    Message = "Patient information not found"
                });
            }

            // Get diagnoses for the appointment
            var diagnoses = await _context.Diagnoses
                .Where(d => d.AppointmentId == appointmentId)
                .ToListAsync();

            if (diagnoses.Count == 0)
            {
                _logger.LogWarning($"No diagnoses found for appointment: {appointmentId}");
                return BadRequest(new ErrorResponse
                {
                    ErrorCode = "DIAGNOSES_NOT_FOUND",
                    Message = "No diagnoses found for this appointment"
                });
            }

            // Get anamnesis for the appointment
            var anamnesis = await _context.Set<Anamnesis>()
                .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);

            // Build clinical information
            var patientAge = facilityPatient.Patient.DateOfBirth.HasValue 
                ? (int)((DateTime.UtcNow - facilityPatient.Patient.DateOfBirth.Value).TotalDays / 365.25)
                : (int?)null;

            var patientGender = facilityPatient.Patient.Gender ?? "Unknown";

            var diagnosisBuilder = new System.Text.StringBuilder();
            diagnosisBuilder.AppendLine("Diagnoses:");
            foreach (var diagnosis in diagnoses)
            {
                diagnosisBuilder.AppendLine($"- {diagnosis.ICD10Code}: {diagnosis.ScientificDescription} (Confidence: {diagnosis.ConfidencePercentage}%)");
            }

            var allergiesBuilder = new System.Text.StringBuilder();
            allergiesBuilder.AppendLine("Patient Allergies:");
            if (!string.IsNullOrWhiteSpace(anamnesis?.DrugAllergies))
                allergiesBuilder.AppendLine($"- Drug Allergies: {anamnesis.DrugAllergies}");
            else
                allergiesBuilder.AppendLine("- Drug Allergies: None reported");
                
            if (!string.IsNullOrWhiteSpace(anamnesis?.FoodAllergies))
                allergiesBuilder.AppendLine($"- Food Allergies: {anamnesis.FoodAllergies}");
            else
                allergiesBuilder.AppendLine("- Food Allergies: None reported");
                
            if (!string.IsNullOrWhiteSpace(anamnesis?.OtherAllergies))
                allergiesBuilder.AppendLine($"- Other Allergies: {anamnesis.OtherAllergies}");

            var anamnesisBuilder = new System.Text.StringBuilder();
            if (anamnesis != null)
            {
                if (!string.IsNullOrWhiteSpace(anamnesis.ChiefComplaint))
                    anamnesisBuilder.AppendLine($"Chief Complaint: {anamnesis.ChiefComplaint}");
                if (!string.IsNullOrWhiteSpace(anamnesis.PastMedicalHistory))
                    anamnesisBuilder.AppendLine($"Past Medical History: {anamnesis.PastMedicalHistory}");
                if (!string.IsNullOrWhiteSpace(anamnesis.CurrentMedications))
                    anamnesisBuilder.AppendLine($"Current Medications: {anamnesis.CurrentMedications}");
            }

            var diagnosisInfo = diagnosisBuilder.ToString();
            var allergiesInfo = allergiesBuilder.ToString();
            var anamnesisInfo = anamnesisBuilder.ToString();

            _logger.LogInformation($"Generating prescription recommendations for appointment {appointmentId} using OpenAI");

            // Create OpenAI prompt
            var maxRecommendations = 10;
            var ageInfo = patientAge.HasValue ? $"{patientAge} years old" : "Age unknown";
            var prompt = $@"You are an experienced pharmacist and medical prescriber. Based on the following patient information, diagnoses, and medical history, generate appropriate medication recommendations that are safe for the patient's age and consider all known allergies.

PATIENT INFORMATION:
- Age: {ageInfo}
- Gender: {patientGender}

PATIENT ALLERGIES:
{allergiesInfo}

PATIENT DIAGNOSES:
{diagnosisInfo}

PATIENT MEDICAL HISTORY:
{anamnesisInfo}

IMPORTANT INSTRUCTIONS:
Generate a JSON response with the following structure. Respond ONLY with valid JSON, no markdown formatting or extra text:

{{
  ""recommendations"": [
    {{
      ""medicationName"": ""string (medication name)"",
      ""dosage"": ""string (e.g., 500mg, 1x500mg per day)"",
      ""signa"": ""string (e.g., 2x1, 3x1 meaning times per day)"",
      ""frequency"": ""string (e.g., Twice daily, Three times daily)"",
      ""quantity"": integer,
      ""instructions"": ""string (how to take the medication)"",
      ""notes"": ""string (any special instructions or warnings)"",
      ""confidenceScore"": integer (0-100)
    }}
  ]
}}

Requirements:
1. Include 3-10 medication recommendations based on the diagnoses
2. Order recommendations by confidence score (highest first)
3. Respond ONLY with the JSON object above - no additional text, code blocks, or formatting
4. Each recommendation must have all fields filled in
5. confidenceScore should reflect how appropriate this medication is for the diagnoses
6. CRITICAL: Do NOT recommend medications to which the patient has known allergies
7. Consider the patient's age when recommending dosages and medications (avoid age-inappropriate options)
8. Include any relevant warnings or precautions in the notes field based on patient allergies and age";

Console.WriteLine("Generated OpenAI Prompt:");
Console.WriteLine(prompt);

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
                        new { role = "system", content = "You are a pharmacist AI assistant specializing in medication recommendations. You must respond ONLY with valid JSON in the exact format requested - no additional text, markdown formatting, or code blocks." },
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
                        return Ok(new PrescriptionResponse { Id = Guid.NewGuid(), AppointmentId = appointmentId, Details = new List<PrescriptionDetailResponse>() });
                    }

                    var responseText = openAiResponse.Choices[0].Message.Content.Trim();

                    Console.WriteLine("OpenAI Response Text:");
                    Console.WriteLine(responseText);

                    // Clean the response if it contains markdown code blocks
                    if (responseText.StartsWith("```json"))
                        responseText = responseText[7..];
                    if (responseText.StartsWith("```"))
                        responseText = responseText[3..];
                    if (responseText.EndsWith("```"))
                        responseText = responseText[..^3];
                    responseText = responseText.Trim();

                    _logger.LogInformation($"OpenAI response received: {responseText}");

                    // Check if response contains an error message
                    if (responseText.Contains("\"error\"") && responseText.Contains("must be a valid JSON array"))
                    {
                        _logger.LogError($"OpenAI returned an error about JSON format: {responseText}");
                        return StatusCode(503, new ErrorResponse
                        {
                            ErrorCode = "OPENAI_INVALID_RESPONSE",
                            Message = "OpenAI returned invalid response format. Please try again."
                        });
                    }

                    // Parse the response - handle both wrapped object and direct array formats
                    List<PrescriptionRecommendationItem> recommendations = null;

                    try
                    {
                        // First try to parse as wrapped response with "recommendations" property
                        try
                        {
                            var wrappedResponse = JsonSerializer.Deserialize<PrescriptionRecommendationResponse>(responseText);
                            if (wrappedResponse?.Recommendations != null && wrappedResponse.Recommendations.Count > 0)
                            {
                                recommendations = wrappedResponse.Recommendations;
                            }
                        }
                        catch (JsonException)
                        {
                            // If wrapped parsing fails, try to parse directly as an array
                            recommendations = JsonSerializer.Deserialize<List<PrescriptionRecommendationItem>>(responseText);
                        }
                    }
                    catch (JsonException ex)
                    {
                        _logger.LogError(ex, $"Failed to parse OpenAI response: {responseText}");
                        return StatusCode(500, new ErrorResponse
                        {
                            ErrorCode = "JSON_PARSE_ERROR",
                            Message = "Failed to parse medication recommendations from OpenAI response"
                        });
                    }

                    if (recommendations == null || recommendations.Count == 0)
                    {
                        _logger.LogWarning("OpenAI returned no prescription recommendations");
                        return Ok(new PrescriptionResponse { Id = Guid.NewGuid(), AppointmentId = appointmentId, Details = new List<PrescriptionDetailResponse>() });
                    }

                    // Create prescription record with details from recommendations
                    var prescription = new Prescription
                    {
                        Id = Guid.NewGuid(),
                        AppointmentId = appointmentId,
                        IsRecommendedByAI = true,
                        AIRecommendationConfidence = (int)recommendations.Average(r => r.ConfidenceScore),
                        CreatedAt = DateTime.UtcNow
                    };

                    _context.Prescriptions.Add(prescription);

                    var prescriptionDetails = new List<PrescriptionDetailResponse>();

                    foreach (var rec in recommendations.Take(maxRecommendations))
                    {
                        var detail = new PrescriptionDetail
                        {
                            Id = Guid.NewGuid(),
                            PrescriptionId = prescription.Id,
                            MedicationName = rec.MedicationName,
                            Dosage = rec.Dosage,
                            Signa = rec.Signa,
                            Frequency = rec.Frequency,
                            Quantity = rec.Quantity,
                            Instructions = rec.Instructions,
                            Notes = rec.Notes,
                            CreatedAt = DateTime.UtcNow
                        };

                        _context.PrescriptionDetails.Add(detail);

                        prescriptionDetails.Add(new PrescriptionDetailResponse
                        {
                            Id = detail.Id,
                            PrescriptionId = detail.PrescriptionId,
                            MedicationName = detail.MedicationName,
                            Dosage = detail.Dosage,
                            Signa = detail.Signa,
                            Frequency = detail.Frequency,
                            Quantity = detail.Quantity,
                            Instructions = detail.Instructions,
                            Notes = detail.Notes,
                            CreatedAt = detail.CreatedAt,
                            UpdatedAt = detail.UpdatedAt
                        });
                    }

                    await _context.SaveChangesAsync();

                    _logger.LogInformation($"Successfully created prescription with {prescriptionDetails.Count} recommendations for appointment {appointmentId}");

                    return Ok(new PrescriptionResponse
                    {
                        Id = prescription.Id,
                        AppointmentId = prescription.AppointmentId,
                        IsRecommendedByAI = prescription.IsRecommendedByAI,
                        AIRecommendationConfidence = prescription.AIRecommendationConfidence,
                        CreatedAt = prescription.CreatedAt,
                        UpdatedAt = prescription.UpdatedAt,
                        Details = prescriptionDetails
                    });
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
            _logger.LogError(ex, "Error generating prescription recommendations");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while generating prescription recommendations"
            });
        }
    }
}

