using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MedizID.API.Data;
using MedizID.API.DTOs;
using MedizID.API.Models;
using MedizID.API.Common;
using Microsoft.EntityFrameworkCore;

namespace MedizID.API.Controllers.Appointments;

/// <summary>
/// Controller for managing postpartum observations
/// </summary>
[Authorize]
[ApiController]
[Route("api/v1/maternal-child-health/postpartum-observation")]
public class PostpartumObservationController : ControllerBase
{
    private readonly MedizIDDbContext _context;
    private readonly ILogger<PostpartumObservationController> _logger;

    public PostpartumObservationController(MedizIDDbContext context, ILogger<PostpartumObservationController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Create a new postpartum observation
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(PostpartumObservationResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreatePostpartumObservation([FromBody] CreatePostpartumObservationRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Verify appointment exists
            var appointment = await _context.Appointments.FindAsync(request.AppointmentId);
            if (appointment == null)
            {
                return NotFound(new { message = "Appointment not found" });
            }

            var observation = new PostpartumObservation
            {
                AppointmentId = request.AppointmentId,
                PncDate = request.PncDate,
                BloodPressure = request.BloodPressure,
                Temperature = request.Temperature,
                Complications = request.Complications,
                RespiratoryRate = request.RespiratoryRate,
                PulseRate = request.PulseRate,
                VaginalBleeding = request.VaginalBleeding,
                PerinealCondition = request.PerinealCondition,
                InfectionSigns = request.InfectionSigns,
                UterineContraction = request.UterineContraction,
                BirthCanalExamination = request.BirthCanalExamination,
                BreastExamination = request.BreastExamination,
                MilkProduction = request.MilkProduction,
                HighRiskComplicationManagement = request.HighRiskComplicationManagement,
                BowelMovements = request.BowelMovements,
                Urination = request.Urination,
                PostpartumDay = request.PostpartumDay,
                RecordedInKiaBook = request.RecordedInKiaBook,
                IronSupplementation = request.IronSupplementation,
                VitaminA = request.VitaminA,
                ReferralDestination = request.ReferralDestination,
                ArtStatus = request.ArtStatus,
                AntiMalariaInfo = request.AntiMalariaInfo,
                AntiTbcInfo = request.AntiTbcInfo,
                ThoraxPhotoStatus = request.ThoraxPhotoStatus,
                Cd4IfComplications = request.Cd4IfComplications,
                ConditionAtArrival = request.ConditionAtArrival,
                ConditionAtDischarge = request.ConditionAtDischarge,
                ContraceptionMethod = request.ContraceptionMethod,
                ContraceptionPlannedDate = request.ContraceptionPlannedDate,
                ContraceptionImplementationDate = request.ContraceptionImplementationDate
            };

            _context.PostpartumObservations.Add(observation);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Postpartum observation created with ID: {ObservationId} for appointment {AppointmentId}", observation.Id, request.AppointmentId);

            return CreatedAtAction(nameof(GetPostpartumObservationById), new { id = observation.Id }, MapToResponse(observation));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating postpartum observation");
            return StatusCode(500, new { message = "An error occurred while creating the observation" });
        }
    }

    /// <summary>
    /// Get postpartum observation by appointment ID
    /// </summary>
    [HttpGet("appointment/{appointmentId}")]
    [ProducesResponseType(typeof(PostpartumObservationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPostpartumObservationByAppointment(Guid appointmentId)
    {
        try
        {
            var observation = await _context.PostpartumObservations
                .FirstOrDefaultAsync(o => o.AppointmentId == appointmentId);

            if (observation == null)
            {
                return NotFound();
            }

            return Ok(MapToResponse(observation));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving postpartum observation for appointment {AppointmentId}", appointmentId);
            return StatusCode(500, new { message = "An error occurred while retrieving the observation" });
        }
    }

    /// <summary>
    /// Get postpartum observation by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PostpartumObservationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPostpartumObservationById(Guid id)
    {
        try
        {
            var observation = await _context.PostpartumObservations.FindAsync(id);

            if (observation == null)
            {
                return NotFound();
            }

            return Ok(MapToResponse(observation));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving postpartum observation with ID: {ObservationId}", id);
            return StatusCode(500, new { message = "An error occurred while retrieving the observation" });
        }
    }

    /// <summary>
    /// Update postpartum observation
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(PostpartumObservationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdatePostpartumObservation(Guid id, [FromBody] UpdatePostpartumObservationRequest request)
    {
        try
        {
            var observation = await _context.PostpartumObservations.FindAsync(id);

            if (observation == null)
            {
                return NotFound();
            }

            observation.PncDate = request.PncDate;
            observation.BloodPressure = request.BloodPressure;
            observation.Temperature = request.Temperature;
            observation.Complications = request.Complications;
            observation.RespiratoryRate = request.RespiratoryRate;
            observation.PulseRate = request.PulseRate;
            observation.VaginalBleeding = request.VaginalBleeding;
            observation.PerinealCondition = request.PerinealCondition;
            observation.InfectionSigns = request.InfectionSigns;
            observation.UterineContraction = request.UterineContraction;
            observation.BirthCanalExamination = request.BirthCanalExamination;
            observation.BreastExamination = request.BreastExamination;
            observation.MilkProduction = request.MilkProduction;
            observation.HighRiskComplicationManagement = request.HighRiskComplicationManagement;
            observation.BowelMovements = request.BowelMovements;
            observation.Urination = request.Urination;
            observation.PostpartumDay = request.PostpartumDay;
            observation.RecordedInKiaBook = request.RecordedInKiaBook;
            observation.IronSupplementation = request.IronSupplementation;
            observation.VitaminA = request.VitaminA;
            observation.ReferralDestination = request.ReferralDestination;
            observation.ArtStatus = request.ArtStatus;
            observation.AntiMalariaInfo = request.AntiMalariaInfo;
            observation.AntiTbcInfo = request.AntiTbcInfo;
            observation.ThoraxPhotoStatus = request.ThoraxPhotoStatus;
            observation.Cd4IfComplications = request.Cd4IfComplications;
            observation.ConditionAtArrival = request.ConditionAtArrival;
            observation.ConditionAtDischarge = request.ConditionAtDischarge;
            observation.ContraceptionMethod = request.ContraceptionMethod;
            observation.ContraceptionPlannedDate = request.ContraceptionPlannedDate;
            observation.ContraceptionImplementationDate = request.ContraceptionImplementationDate;
            observation.UpdatedAt = DateTimeOffset.UtcNow;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Postpartum observation with ID: {ObservationId} updated successfully", id);

            return Ok(MapToResponse(observation));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating postpartum observation with ID: {ObservationId}", id);
            return StatusCode(500, new { message = "An error occurred while updating the observation" });
        }
    }

    /// <summary>
    /// Delete postpartum observation
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeletePostpartumObservation(Guid id)
    {
        try
        {
            var observation = await _context.PostpartumObservations.FindAsync(id);

            if (observation == null)
            {
                return NotFound();
            }

            _context.PostpartumObservations.Remove(observation);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Postpartum observation with ID: {ObservationId} deleted successfully", id);

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting postpartum observation with ID: {ObservationId}", id);
            return StatusCode(500, new { message = "An error occurred while deleting the observation" });
        }
    }

    /// <summary>
    /// Map PostpartumObservation entity to response DTO
    /// </summary>
    private static PostpartumObservationResponse MapToResponse(PostpartumObservation observation)
    {
        return new PostpartumObservationResponse
        {
            Id = observation.Id,
            AppointmentId = observation.AppointmentId,
            PncDate = observation.PncDate,
            BloodPressure = observation.BloodPressure,
            Temperature = observation.Temperature,
            Complications = observation.Complications,
            RespiratoryRate = observation.RespiratoryRate,
            PulseRate = observation.PulseRate,
            VaginalBleeding = observation.VaginalBleeding,
            PerinealCondition = observation.PerinealCondition,
            InfectionSigns = observation.InfectionSigns,
            UterineContraction = observation.UterineContraction,
            BirthCanalExamination = observation.BirthCanalExamination,
            BreastExamination = observation.BreastExamination,
            MilkProduction = observation.MilkProduction,
            HighRiskComplicationManagement = observation.HighRiskComplicationManagement,
            BowelMovements = observation.BowelMovements,
            Urination = observation.Urination,
            PostpartumDay = observation.PostpartumDay,
            RecordedInKiaBook = observation.RecordedInKiaBook,
            IronSupplementation = observation.IronSupplementation,
            VitaminA = observation.VitaminA,
            ReferralDestination = observation.ReferralDestination,
            ArtStatus = observation.ArtStatus,
            AntiMalariaInfo = observation.AntiMalariaInfo,
            AntiTbcInfo = observation.AntiTbcInfo,
            ThoraxPhotoStatus = observation.ThoraxPhotoStatus,
            Cd4IfComplications = observation.Cd4IfComplications,
            ConditionAtArrival = observation.ConditionAtArrival,
            ConditionAtDischarge = observation.ConditionAtDischarge,
            ContraceptionMethod = observation.ContraceptionMethod,
            ContraceptionPlannedDate = observation.ContraceptionPlannedDate,
            ContraceptionImplementationDate = observation.ContraceptionImplementationDate,
            CreatedAt = observation.CreatedAt,
            UpdatedAt = observation.UpdatedAt
        };
    }
}
