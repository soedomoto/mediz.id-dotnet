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
[Route("api/v1/MaternalChildHealth/AntenatalCareObservation")]
[Produces("application/json")]
[Authorize]
public class AntenatalCareObservationController : ControllerBase
{
    private readonly MedizIDDbContext _context;
    private readonly ILogger<AntenatalCareObservationController> _logger;

    public AntenatalCareObservationController(MedizIDDbContext context, ILogger<AntenatalCareObservationController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Get all antenatal care observations with pagination
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<AntenatalCareObservationResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAntenatalCareObservations(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var query = _context.AntenatalCareObservations.AsQueryable();

            var total = await query.CountAsync();

            var observations = await query
                .OrderByDescending(a => a.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new AntenatalCareObservationResponse
                {
                    Id = a.Id,
                    AppointmentId = a.AppointmentId,
                    MedicalPersonnelId = a.MedicalPersonnelId,
                    MedicalPersonnelName = a.MedicalPersonnelName,
                    NurseId = a.NurseId,
                    NurseName = a.NurseName,
                    MaternityHealthPostName = a.MaternityHealthPostName,
                    CadreName = a.CadreName,
                    TraditionalBirthAttendantName = a.TraditionalBirthAttendantName,
                    ObstetricComplicationHistory = a.ObstetricComplicationHistory,
                    ChronicDiseaseAndAllergy = a.ChronicDiseaseAndAllergy,
                    DiseaseHistory = a.DiseaseHistory,
                    Gravida = a.Gravida,
                    Partus = a.Partus,
                    Abortus = a.Abortus,
                    AliveChildren = a.AliveChildren,
                    PlannedDeliveryDate = a.PlannedDeliveryDate,
                    PlannedDeliveryAssistant = a.PlannedDeliveryAssistant.ToString(),
                    PlannedDeliveryPlace = a.PlannedDeliveryPlace.ToString(),
                    PlannedCompanion = a.PlannedCompanion.ToString(),
                    PlannedTransportation = a.PlannedTransportation.ToString(),
                    BloodDonorStatus = a.BloodDonorStatus.ToString(),
                    LastMenstrualPeriodDate = a.LastMenstrualPeriodDate,
                    EstimatedDeliveryDate = a.EstimatedDeliveryDate,
                    PreviousDeliveryDate = a.PreviousDeliveryDate,
                    KiaBookStatus = a.KiaBookStatus.ToString(),
                    PrePregnancyWeight = a.PrePregnancyWeight,
                    Height = a.Height,
                    MotherKsurScore = a.MotherKsurScore,
                    PregnancyRiskCategory = a.PregnancyRiskCategory.ToString(),
                    HighRiskDescription = a.HighRiskDescription,
                    CasuisticRiskType = a.CasuisticRiskType.ToString(),
                    CreatedAt = a.CreatedAt,
                    UpdatedAt = a.UpdatedAt
                })
                .ToListAsync();

            return Ok(new PagedResult<AntenatalCareObservationResponse>
            {
                Items = observations,
                Total = total,
                Page = page,
                PageSize = pageSize
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching antenatal care observations");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching antenatal care observations"
            });
        }
    }

    /// <summary>
    /// Get antenatal care observation by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(AntenatalCareObservationDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAntenatalCareObservation(Guid id)
    {
        try
        {
            var observation = await _context.AntenatalCareObservations
                .Include(a => a.Appointment)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (observation == null)
            {
                _logger.LogWarning($"Antenatal care observation not found: {id}");
                throw new NotFoundException($"Antenatal care observation with ID {id} not found");
            }

            var response = MapToDetailResponse(observation);
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
            _logger.LogError(ex, $"Error fetching antenatal care observation {id}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching the antenatal care observation"
            });
        }
    }

    /// <summary>
    /// Get antenatal care observation by appointment ID
    /// </summary>
    [HttpGet("appointment/{appointmentId}")]
    [ProducesResponseType(typeof(AntenatalCareObservationDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAntenatalCareObservationByAppointmentId(Guid appointmentId)
    {
        try
        {
            var appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.Id == appointmentId);
            if (appointment == null)
            {
                _logger.LogWarning($"Appointment not found: {appointmentId}");
                throw new NotFoundException($"Appointment with ID {appointmentId} not found");
            }

            var observation = await _context.AntenatalCareObservations
                .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);

            if (observation == null)
            {
                _logger.LogWarning($"No antenatal care observation found for appointment: {appointmentId}");
                throw new NotFoundException($"No antenatal care observation found for appointment {appointmentId}");
            }

            var response = MapToDetailResponse(observation);
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
            _logger.LogError(ex, $"Error fetching antenatal care observation for appointment {appointmentId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching the antenatal care observation"
            });
        }
    }

    /// <summary>
    /// Create a new antenatal care observation
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(AntenatalCareObservationDetailResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAntenatalCareObservation([FromBody] CreateAntenatalCareObservationRequest request)
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

            // Check if observation already exists
            var existingObservation = await _context.AntenatalCareObservations
                .FirstOrDefaultAsync(a => a.AppointmentId == request.AppointmentId);
            
            if (existingObservation != null)
            {
                return BadRequest(new ErrorResponse
                {
                    ErrorCode = "OBSERVATION_ALREADY_EXISTS",
                    Message = "An antenatal care observation already exists for this appointment"
                });
            }

            var observation = new AntenatalCareObservation
            {
                Id = Guid.NewGuid(),
                AppointmentId = request.AppointmentId,
                MedicalPersonnelId = request.MedicalPersonnelId,
                MedicalPersonnelName = request.MedicalPersonnelName?.Trim(),
                NurseId = request.NurseId,
                NurseName = request.NurseName?.Trim(),
                MaternityHealthPostName = request.MaternityHealthPostName?.Trim(),
                CadreName = request.CadreName?.Trim(),
                TraditionalBirthAttendantName = request.TraditionalBirthAttendantName?.Trim(),
                ObstetricComplicationHistory = request.ObstetricComplicationHistory?.Trim(),
                ChronicDiseaseAndAllergy = request.ChronicDiseaseAndAllergy?.Trim(),
                DiseaseHistory = request.DiseaseHistory?.Trim(),
                Gravida = request.Gravida,
                Partus = request.Partus,
                Abortus = request.Abortus,
                AliveChildren = request.AliveChildren,
                PlannedDeliveryDate = request.PlannedDeliveryDate,
                PlannedDeliveryAssistant = TryParseEnum<Common.Enums.DeliveryAssistantTypeEnum>(request.PlannedDeliveryAssistant),
                PlannedDeliveryPlace = TryParseEnum<Common.Enums.DeliveryPlaceEnum>(request.PlannedDeliveryPlace),
                PlannedCompanion = TryParseEnum<Common.Enums.CompanionTypeEnum>(request.PlannedCompanion),
                PlannedTransportation = TryParseEnum<Common.Enums.TransportationTypeEnum>(request.PlannedTransportation),
                BloodDonorStatus = TryParseEnum<Common.Enums.BloodDonorStatusEnum>(request.BloodDonorStatus),
                LastMenstrualPeriodDate = request.LastMenstrualPeriodDate,
                EstimatedDeliveryDate = request.EstimatedDeliveryDate,
                PreviousDeliveryDate = request.PreviousDeliveryDate,
                KiaBookStatus = TryParseEnum<Common.Enums.KiaBookStatusEnum>(request.KiaBookStatus),
                PrePregnancyWeight = request.PrePregnancyWeight,
                Height = request.Height,
                MotherKsurScore = request.MotherKsurScore,
                PregnancyRiskCategory = TryParseEnum<Common.Enums.KsurRiskCategoryEnum>(request.PregnancyRiskCategory),
                HighRiskDescription = request.HighRiskDescription?.Trim(),
                CasuisticRiskType = TryParseEnum<Common.Enums.CasuisticRiskTypeEnum>(request.CasuisticRiskType),
                CreatedAt = DateTime.UtcNow
            };

            _context.AntenatalCareObservations.Add(observation);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Antenatal care observation created: {observation.Id}");

            var response = MapToDetailResponse(observation);
            return CreatedAtAction(nameof(GetAntenatalCareObservation), new { id = observation.Id }, response);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid enum value");
            return BadRequest(new ErrorResponse
            {
                ErrorCode = "VALIDATION_ERROR",
                Message = "Invalid enum value"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating antenatal care observation");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while creating the antenatal care observation"
            });
        }
    }

    /// <summary>
    /// Update antenatal care observation
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(AntenatalCareObservationDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAntenatalCareObservation(Guid id, [FromBody] CreateAntenatalCareObservationRequest request)
    {
        try
        {
            var observation = await _context.AntenatalCareObservations.FirstOrDefaultAsync(a => a.Id == id);
            if (observation == null)
            {
                _logger.LogWarning($"Antenatal care observation not found: {id}");
                throw new NotFoundException($"Antenatal care observation with ID {id} not found");
            }

            // Update all fields
            observation.MedicalPersonnelId = request.MedicalPersonnelId;
            observation.MedicalPersonnelName = request.MedicalPersonnelName?.Trim();
            observation.NurseId = request.NurseId;
            observation.NurseName = request.NurseName?.Trim();
            observation.MaternityHealthPostName = request.MaternityHealthPostName?.Trim();
            observation.CadreName = request.CadreName?.Trim();
            observation.TraditionalBirthAttendantName = request.TraditionalBirthAttendantName?.Trim();
            observation.ObstetricComplicationHistory = request.ObstetricComplicationHistory?.Trim();
            observation.ChronicDiseaseAndAllergy = request.ChronicDiseaseAndAllergy?.Trim();
            observation.DiseaseHistory = request.DiseaseHistory?.Trim();
            observation.Gravida = request.Gravida;
            observation.Partus = request.Partus;
            observation.Abortus = request.Abortus;
            observation.AliveChildren = request.AliveChildren;
            observation.PlannedDeliveryDate = request.PlannedDeliveryDate;
            observation.PlannedDeliveryAssistant = TryParseEnum<Common.Enums.DeliveryAssistantTypeEnum>(request.PlannedDeliveryAssistant);
            observation.PlannedDeliveryPlace = TryParseEnum<Common.Enums.DeliveryPlaceEnum>(request.PlannedDeliveryPlace);
            observation.PlannedCompanion = TryParseEnum<Common.Enums.CompanionTypeEnum>(request.PlannedCompanion);
            observation.PlannedTransportation = TryParseEnum<Common.Enums.TransportationTypeEnum>(request.PlannedTransportation);
            observation.BloodDonorStatus = TryParseEnum<Common.Enums.BloodDonorStatusEnum>(request.BloodDonorStatus);
            observation.LastMenstrualPeriodDate = request.LastMenstrualPeriodDate;
            observation.EstimatedDeliveryDate = request.EstimatedDeliveryDate;
            observation.PreviousDeliveryDate = request.PreviousDeliveryDate;
            observation.KiaBookStatus = TryParseEnum<Common.Enums.KiaBookStatusEnum>(request.KiaBookStatus);
            observation.PrePregnancyWeight = request.PrePregnancyWeight;
            observation.Height = request.Height;
            observation.MotherKsurScore = request.MotherKsurScore;
            observation.PregnancyRiskCategory = TryParseEnum<Common.Enums.KsurRiskCategoryEnum>(request.PregnancyRiskCategory);
            observation.HighRiskDescription = request.HighRiskDescription?.Trim();
            observation.CasuisticRiskType = TryParseEnum<Common.Enums.CasuisticRiskTypeEnum>(request.CasuisticRiskType);
            observation.UpdatedAt = DateTime.UtcNow;

            _context.AntenatalCareObservations.Update(observation);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Antenatal care observation updated: {observation.Id}");

            var response = MapToDetailResponse(observation);
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
            _logger.LogWarning(ex, "Invalid enum value");
            return BadRequest(new ErrorResponse
            {
                ErrorCode = "VALIDATION_ERROR",
                Message = "Invalid enum value"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating antenatal care observation {id}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while updating the antenatal care observation"
            });
        }
    }

    /// <summary>
    /// Delete antenatal care observation
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAntenatalCareObservation(Guid id)
    {
        try
        {
            var observation = await _context.AntenatalCareObservations.FirstOrDefaultAsync(a => a.Id == id);
            if (observation == null)
            {
                _logger.LogWarning($"Antenatal care observation not found: {id}");
                throw new NotFoundException($"Antenatal care observation with ID {id} not found");
            }

            _context.AntenatalCareObservations.Remove(observation);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Antenatal care observation deleted: {id}");

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
            _logger.LogError(ex, $"Error deleting antenatal care observation {id}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while deleting the antenatal care observation"
            });
        }
    }

    // Helper method to parse enum values safely
    private T? TryParseEnum<T>(string? value) where T : struct, Enum
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        if (Enum.TryParse<T>(value, ignoreCase: true, out var result))
            return result;

        return null;
    }

    // Helper method to map to detail response
    private AntenatalCareObservationDetailResponse MapToDetailResponse(AntenatalCareObservation observation)
    {
        return new AntenatalCareObservationDetailResponse
        {
            Id = observation.Id,
            AppointmentId = observation.AppointmentId,
            MedicalPersonnelId = observation.MedicalPersonnelId,
            MedicalPersonnelName = observation.MedicalPersonnelName,
            NurseId = observation.NurseId,
            NurseName = observation.NurseName,
            MaternityHealthPostName = observation.MaternityHealthPostName,
            CadreName = observation.CadreName,
            TraditionalBirthAttendantName = observation.TraditionalBirthAttendantName,
            ObstetricComplicationHistory = observation.ObstetricComplicationHistory,
            ChronicDiseaseAndAllergy = observation.ChronicDiseaseAndAllergy,
            DiseaseHistory = observation.DiseaseHistory,
            Gravida = observation.Gravida,
            Partus = observation.Partus,
            Abortus = observation.Abortus,
            AliveChildren = observation.AliveChildren,
            PlannedDeliveryDate = observation.PlannedDeliveryDate,
            PlannedDeliveryAssistant = observation.PlannedDeliveryAssistant?.ToString(),
            PlannedDeliveryPlace = observation.PlannedDeliveryPlace?.ToString(),
            PlannedCompanion = observation.PlannedCompanion?.ToString(),
            PlannedTransportation = observation.PlannedTransportation?.ToString(),
            BloodDonorStatus = observation.BloodDonorStatus?.ToString(),
            LastMenstrualPeriodDate = observation.LastMenstrualPeriodDate,
            EstimatedDeliveryDate = observation.EstimatedDeliveryDate,
            PreviousDeliveryDate = observation.PreviousDeliveryDate,
            KiaBookStatus = observation.KiaBookStatus?.ToString(),
            PrePregnancyWeight = observation.PrePregnancyWeight,
            Height = observation.Height,
            MotherKsurScore = observation.MotherKsurScore,
            PregnancyRiskCategory = observation.PregnancyRiskCategory?.ToString(),
            HighRiskDescription = observation.HighRiskDescription,
            CasuisticRiskType = observation.CasuisticRiskType?.ToString(),
            CreatedAt = observation.CreatedAt,
            UpdatedAt = observation.UpdatedAt
        };
    }
}
