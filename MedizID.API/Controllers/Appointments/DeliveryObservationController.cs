using MedizID.API.Common;
using MedizID.API.Data;
using MedizID.API.DTOs;
using MedizID.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedizID.API.Controllers.Appointments;

/// <summary>
/// Controller for managing delivery observations (clinical data related to childbirth/delivery).
/// Provides CRUD operations for recording and retrieving comprehensive delivery outcome information
/// including maternal/neonatal status, delivery method, and complications.
/// </summary>
[ApiController]
[Route("api/v1/maternal-child-health/delivery-observation")]
[Authorize]
public class DeliveryObservationController : ControllerBase
{
    private readonly MedizIDDbContext _context;
    private readonly ILogger<DeliveryObservationController> _logger;

    public DeliveryObservationController(MedizIDDbContext context, ILogger<DeliveryObservationController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>Creates a new delivery observation record.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(DeliveryObservationResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateDeliveryObservation([FromBody] CreateDeliveryObservationRequest request)
    {
        try
        {
            // Verify appointment exists
            var appointment = await _context.Appointments.FindAsync(request.AppointmentId);
            if (appointment == null)
            {
                return NotFound(new ErrorResponse { Message = "Appointment not found" });
            }

            var observation = new DeliveryObservation
            {
                Id = Guid.NewGuid(),
                AppointmentId = request.AppointmentId,
                GestationalAge = request.GestationalAge,
                GestationalAgeFromLmp = request.GestationalAgeFromLmp,
                MaternalCondition = request.MaternalCondition,
                MaternalDischargingCondition = request.MaternalDischargingCondition,
                NeonatalCondition = request.NeonatalCondition,
                NeonatalWeight = request.NeonatalWeight,
                Presentation = request.Presentation,
                DeliveryLocation = request.DeliveryLocation,
                BirthAttendant = request.BirthAttendant,
                DeliveryMode = request.DeliveryMode,
                OxytocinAdministered = request.OxytocinAdministered,
                ControlledCordTraction = request.ControlledCordTraction,
                UterineMassage = request.UterineMassage,
                BloodTransfusion = request.BloodTransfusion,
                AntibioticTherapy = request.AntibioticTherapy,
                NeonatalResuscitation = request.NeonatalResuscitation,
                ProgramIntegration = request.ProgramIntegration,
                AntiretroviralProphylaxis = request.AntiretroviralProphylaxis,
                HasComplications = request.HasComplications,
                ComplicationsDescription = request.ComplicationsDescription,
                WasReferred = request.WasReferred,
                ReferralDestination = request.ReferralDestination,
                MaternalConditionAtReferral = request.MaternalConditionAtReferral,
                DeliveryAddress = request.DeliveryAddress,
                CreatedAt = DateTimeOffset.UtcNow
            };

            _context.DeliveryObservations.Add(observation);
            await _context.SaveChangesAsync();

            var response = new DeliveryObservationResponse
            {
                Id = observation.Id,
                AppointmentId = observation.AppointmentId,
                GestationalAge = observation.GestationalAge,
                GestationalAgeFromLmp = observation.GestationalAgeFromLmp,
                MaternalCondition = observation.MaternalCondition,
                MaternalDischargingCondition = observation.MaternalDischargingCondition,
                NeonatalCondition = observation.NeonatalCondition,
                NeonatalWeight = observation.NeonatalWeight,
                Presentation = observation.Presentation,
                DeliveryLocation = observation.DeliveryLocation,
                BirthAttendant = observation.BirthAttendant,
                DeliveryMode = observation.DeliveryMode,
                OxytocinAdministered = observation.OxytocinAdministered,
                ControlledCordTraction = observation.ControlledCordTraction,
                UterineMassage = observation.UterineMassage,
                BloodTransfusion = observation.BloodTransfusion,
                AntibioticTherapy = observation.AntibioticTherapy,
                NeonatalResuscitation = observation.NeonatalResuscitation,
                ProgramIntegration = observation.ProgramIntegration,
                AntiretroviralProphylaxis = observation.AntiretroviralProphylaxis,
                HasComplications = observation.HasComplications,
                ComplicationsDescription = observation.ComplicationsDescription,
                WasReferred = observation.WasReferred,
                ReferralDestination = observation.ReferralDestination,
                MaternalConditionAtReferral = observation.MaternalConditionAtReferral,
                DeliveryAddress = observation.DeliveryAddress,
                CreatedAt = observation.CreatedAt.DateTime,
                UpdatedAt = observation.UpdatedAt?.DateTime
            };

            _logger.LogInformation("Delivery observation created with ID {DeliveryObservationId}", observation.Id);
            return CreatedAtAction(nameof(GetDeliveryObservationById), new { id = observation.Id }, response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating delivery observation");
            return BadRequest(new ErrorResponse { Message = "Error creating delivery observation", Details = ex.Message });
        }
    }

    /// <summary>Retrieves all delivery observations for an appointment.</summary>
    [HttpGet("appointment/{appointmentId}")]
    [ProducesResponseType(typeof(DeliveryObservationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDeliveryObservationByAppointment(Guid appointmentId)
    {
        try
        {
            var observation = await _context.DeliveryObservations
                .Where(o => o.AppointmentId == appointmentId)
                .FirstOrDefaultAsync();

            if (observation == null)
            {
                return NotFound(new ErrorResponse { Message = "Delivery observation not found" });
            }

            var response = MapToResponse(observation);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving delivery observation");
            return BadRequest(new ErrorResponse { Message = "Error retrieving delivery observation", Details = ex.Message });
        }
    }

    /// <summary>Retrieves a specific delivery observation by ID.</summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(DeliveryObservationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDeliveryObservationById(Guid id)
    {
        try
        {
            var observation = await _context.DeliveryObservations.FindAsync(id);
            if (observation == null)
            {
                return NotFound(new ErrorResponse { Message = "Delivery observation not found" });
            }

            var response = MapToResponse(observation);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving delivery observation");
            return BadRequest(new ErrorResponse { Message = "Error retrieving delivery observation", Details = ex.Message });
        }
    }

    /// <summary>Updates an existing delivery observation record.</summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(DeliveryObservationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateDeliveryObservation(Guid id, [FromBody] UpdateDeliveryObservationRequest request)
    {
        try
        {
            var observation = await _context.DeliveryObservations.FindAsync(id);
            if (observation == null)
            {
                return NotFound(new ErrorResponse { Message = "Delivery observation not found" });
            }

            observation.GestationalAge = request.GestationalAge ?? observation.GestationalAge;
            observation.GestationalAgeFromLmp = request.GestationalAgeFromLmp ?? observation.GestationalAgeFromLmp;
            observation.MaternalCondition = request.MaternalCondition ?? observation.MaternalCondition;
            observation.MaternalDischargingCondition = request.MaternalDischargingCondition ?? observation.MaternalDischargingCondition;
            observation.NeonatalCondition = request.NeonatalCondition ?? observation.NeonatalCondition;
            observation.NeonatalWeight = request.NeonatalWeight ?? observation.NeonatalWeight;
            observation.Presentation = request.Presentation ?? observation.Presentation;
            observation.DeliveryLocation = request.DeliveryLocation ?? observation.DeliveryLocation;
            observation.BirthAttendant = request.BirthAttendant ?? observation.BirthAttendant;
            observation.DeliveryMode = request.DeliveryMode ?? observation.DeliveryMode;
            observation.OxytocinAdministered = request.OxytocinAdministered ?? observation.OxytocinAdministered;
            observation.ControlledCordTraction = request.ControlledCordTraction ?? observation.ControlledCordTraction;
            observation.UterineMassage = request.UterineMassage ?? observation.UterineMassage;
            observation.BloodTransfusion = request.BloodTransfusion ?? observation.BloodTransfusion;
            observation.AntibioticTherapy = request.AntibioticTherapy ?? observation.AntibioticTherapy;
            observation.NeonatalResuscitation = request.NeonatalResuscitation ?? observation.NeonatalResuscitation;
            observation.ProgramIntegration = request.ProgramIntegration ?? observation.ProgramIntegration;
            observation.AntiretroviralProphylaxis = request.AntiretroviralProphylaxis ?? observation.AntiretroviralProphylaxis;
            observation.HasComplications = request.HasComplications ?? observation.HasComplications;
            observation.ComplicationsDescription = request.ComplicationsDescription ?? observation.ComplicationsDescription;
            observation.WasReferred = request.WasReferred ?? observation.WasReferred;
            observation.ReferralDestination = request.ReferralDestination ?? observation.ReferralDestination;
            observation.MaternalConditionAtReferral = request.MaternalConditionAtReferral ?? observation.MaternalConditionAtReferral;
            observation.DeliveryAddress = request.DeliveryAddress ?? observation.DeliveryAddress;
            observation.UpdatedAt = DateTimeOffset.UtcNow;

            _context.DeliveryObservations.Update(observation);
            await _context.SaveChangesAsync();

            var response = MapToResponse(observation);
            _logger.LogInformation("Delivery observation {DeliveryObservationId} updated", id);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating delivery observation");
            return BadRequest(new ErrorResponse { Message = "Error updating delivery observation", Details = ex.Message });
        }
    }

    /// <summary>Deletes a delivery observation record.</summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteDeliveryObservation(Guid id)
    {
        try
        {
            var observation = await _context.DeliveryObservations.FindAsync(id);
            if (observation == null)
            {
                return NotFound(new ErrorResponse { Message = "Delivery observation not found" });
            }

            _context.DeliveryObservations.Remove(observation);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Delivery observation {DeliveryObservationId} deleted", id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting delivery observation");
            return BadRequest(new ErrorResponse { Message = "Error deleting delivery observation", Details = ex.Message });
        }
    }

    private DeliveryObservationResponse MapToResponse(DeliveryObservation observation)
    {
        return new DeliveryObservationResponse
        {
            Id = observation.Id,
            AppointmentId = observation.AppointmentId,
            GestationalAge = observation.GestationalAge,
            GestationalAgeFromLmp = observation.GestationalAgeFromLmp,
            MaternalCondition = observation.MaternalCondition,
            MaternalDischargingCondition = observation.MaternalDischargingCondition,
            NeonatalCondition = observation.NeonatalCondition,
            NeonatalWeight = observation.NeonatalWeight,
            Presentation = observation.Presentation,
            DeliveryLocation = observation.DeliveryLocation,
            BirthAttendant = observation.BirthAttendant,
            DeliveryMode = observation.DeliveryMode,
            OxytocinAdministered = observation.OxytocinAdministered,
            ControlledCordTraction = observation.ControlledCordTraction,
            UterineMassage = observation.UterineMassage,
            BloodTransfusion = observation.BloodTransfusion,
            AntibioticTherapy = observation.AntibioticTherapy,
            NeonatalResuscitation = observation.NeonatalResuscitation,
            ProgramIntegration = observation.ProgramIntegration,
            AntiretroviralProphylaxis = observation.AntiretroviralProphylaxis,
            HasComplications = observation.HasComplications,
            ComplicationsDescription = observation.ComplicationsDescription,
            WasReferred = observation.WasReferred,
            ReferralDestination = observation.ReferralDestination,
            MaternalConditionAtReferral = observation.MaternalConditionAtReferral,
            DeliveryAddress = observation.DeliveryAddress,
            CreatedAt = observation.CreatedAt.DateTime,
            UpdatedAt = observation.UpdatedAt?.DateTime
        };
    }
}
