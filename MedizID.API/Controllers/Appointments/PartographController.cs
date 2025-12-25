using MedizID.API.Common;
using MedizID.API.Data;
using MedizID.API.DTOs;
using MedizID.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedizID.API.Controllers.Appointments;

/// <summary>
/// Controller for managing partographs (labour progress charts).
/// Provides CRUD operations for recording and retrieving comprehensive labor progress information
/// including cervical dilation, fetal descent, maternal vital signs, and interventions.
/// </summary>
[ApiController]
[Route("api/v1/maternal-child-health/partograph")]
[Authorize]
public class PartographController : ControllerBase
{
    private readonly MedizIDDbContext _context;
    private readonly ILogger<PartographController> _logger;

    public PartographController(MedizIDDbContext context, ILogger<PartographController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>Creates a new partograph record.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(PartographResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreatePartograph([FromBody] CreatePartographRequest request)
    {
        try
        {
            // Verify appointment exists
            var appointment = await _context.Appointments.FindAsync(request.AppointmentId);
            if (appointment == null)
            {
                return NotFound(new ErrorResponse { Message = "Appointment not found" });
            }

            var partograph = new Partograph
            {
                Id = Guid.NewGuid(),
                AppointmentId = request.AppointmentId,
                AdmissionDate = request.AdmissionDate,
                AdmissionTime = request.AdmissionTime,
                OnsetOfLaborDate = request.OnsetOfLaborDate,
                OnsetOfLaborTime = request.OnsetOfLaborTime,
                RuptureOfMembranesDate = request.RuptureOfMembranesDate,
                RuptureOfMembranesTime = request.RuptureOfMembranesTime,
                CervicalDilation = request.CervicalDilation,
                CervicalEffacement = request.CervicalEffacement,
                FetalDescent = request.FetalDescent,
                Molding = request.Molding,
                FetalHeartRateReadings = request.FetalHeartRateReadings,
                AmnioticFluidStatus = request.AmnioticFluidStatus,
                MoldingMonitoring = request.MoldingMonitoring,
                PulseRateReadings = request.PulseRateReadings,
                BloodPressureReadings = request.BloodPressureReadings,
                TemperatureReadings = request.TemperatureReadings,
                UrineOutput = request.UrineOutput,
                OxytocinAdministration = request.OxytocinAdministration,
                IVFluidAdministration = request.IVFluidAdministration,
                OtherMedications = request.OtherMedications,
                LaborNotes = request.LaborNotes,
                Complications = request.Complications,
                ComplicationActions = request.ComplicationActions,
                DeliveryDateTime = request.DeliveryDateTime,
                PostpartumMaternalCondition = request.PostpartumMaternalCondition,
                PlacentaDelivery = request.PlacentaDelivery,
                ThirdStageDuration = request.ThirdStageDuration,
                MaternalHemorrhageEstimate = request.MaternalHemorrhageEstimate,
                PerinealCondition = request.PerinealCondition,
                BladderStatus = request.BladderStatus,
                UterineContractionStatus = request.UterineContractionStatus,
                CreatedAt = DateTimeOffset.UtcNow
            };

            _context.Partographs.Add(partograph);
            await _context.SaveChangesAsync();

            var response = MapToResponse(partograph);
            _logger.LogInformation("Partograph created with ID {PartographId}", partograph.Id);
            return CreatedAtAction(nameof(GetPartographById), new { id = partograph.Id }, response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating partograph");
            return BadRequest(new ErrorResponse { Message = "Error creating partograph", Details = ex.Message });
        }
    }

    /// <summary>Retrieves a partograph for an appointment.</summary>
    [HttpGet("appointment/{appointmentId}")]
    [ProducesResponseType(typeof(PartographResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPartographByAppointment(Guid appointmentId)
    {
        try
        {
            var partograph = await _context.Partographs
                .Where(p => p.AppointmentId == appointmentId)
                .FirstOrDefaultAsync();

            if (partograph == null)
            {
                return NotFound(new ErrorResponse { Message = "Partograph not found" });
            }

            var response = MapToResponse(partograph);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving partograph");
            return BadRequest(new ErrorResponse { Message = "Error retrieving partograph", Details = ex.Message });
        }
    }

    /// <summary>Retrieves a specific partograph by ID.</summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PartographResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPartographById(Guid id)
    {
        try
        {
            var partograph = await _context.Partographs.FindAsync(id);
            if (partograph == null)
            {
                return NotFound(new ErrorResponse { Message = "Partograph not found" });
            }

            var response = MapToResponse(partograph);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving partograph");
            return BadRequest(new ErrorResponse { Message = "Error retrieving partograph", Details = ex.Message });
        }
    }

    /// <summary>Updates an existing partograph record.</summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(PartographResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdatePartograph(Guid id, [FromBody] UpdatePartographRequest request)
    {
        try
        {
            var partograph = await _context.Partographs.FindAsync(id);
            if (partograph == null)
            {
                return NotFound(new ErrorResponse { Message = "Partograph not found" });
            }

            partograph.AdmissionDate = request.AdmissionDate ?? partograph.AdmissionDate;
            partograph.AdmissionTime = request.AdmissionTime ?? partograph.AdmissionTime;
            partograph.OnsetOfLaborDate = request.OnsetOfLaborDate ?? partograph.OnsetOfLaborDate;
            partograph.OnsetOfLaborTime = request.OnsetOfLaborTime ?? partograph.OnsetOfLaborTime;
            partograph.RuptureOfMembranesDate = request.RuptureOfMembranesDate ?? partograph.RuptureOfMembranesDate;
            partograph.RuptureOfMembranesTime = request.RuptureOfMembranesTime ?? partograph.RuptureOfMembranesTime;
            partograph.CervicalDilation = request.CervicalDilation ?? partograph.CervicalDilation;
            partograph.CervicalEffacement = request.CervicalEffacement ?? partograph.CervicalEffacement;
            partograph.FetalDescent = request.FetalDescent ?? partograph.FetalDescent;
            partograph.Molding = request.Molding ?? partograph.Molding;
            partograph.FetalHeartRateReadings = request.FetalHeartRateReadings ?? partograph.FetalHeartRateReadings;
            partograph.AmnioticFluidStatus = request.AmnioticFluidStatus ?? partograph.AmnioticFluidStatus;
            partograph.MoldingMonitoring = request.MoldingMonitoring ?? partograph.MoldingMonitoring;
            partograph.PulseRateReadings = request.PulseRateReadings ?? partograph.PulseRateReadings;
            partograph.BloodPressureReadings = request.BloodPressureReadings ?? partograph.BloodPressureReadings;
            partograph.TemperatureReadings = request.TemperatureReadings ?? partograph.TemperatureReadings;
            partograph.UrineOutput = request.UrineOutput ?? partograph.UrineOutput;
            partograph.OxytocinAdministration = request.OxytocinAdministration ?? partograph.OxytocinAdministration;
            partograph.IVFluidAdministration = request.IVFluidAdministration ?? partograph.IVFluidAdministration;
            partograph.OtherMedications = request.OtherMedications ?? partograph.OtherMedications;
            partograph.LaborNotes = request.LaborNotes ?? partograph.LaborNotes;
            partograph.Complications = request.Complications ?? partograph.Complications;
            partograph.ComplicationActions = request.ComplicationActions ?? partograph.ComplicationActions;
            partograph.DeliveryDateTime = request.DeliveryDateTime ?? partograph.DeliveryDateTime;
            partograph.PostpartumMaternalCondition = request.PostpartumMaternalCondition ?? partograph.PostpartumMaternalCondition;
            partograph.PlacentaDelivery = request.PlacentaDelivery ?? partograph.PlacentaDelivery;
            partograph.ThirdStageDuration = request.ThirdStageDuration ?? partograph.ThirdStageDuration;
            partograph.MaternalHemorrhageEstimate = request.MaternalHemorrhageEstimate ?? partograph.MaternalHemorrhageEstimate;
            partograph.PerinealCondition = request.PerinealCondition ?? partograph.PerinealCondition;
            partograph.BladderStatus = request.BladderStatus ?? partograph.BladderStatus;
            partograph.UterineContractionStatus = request.UterineContractionStatus ?? partograph.UterineContractionStatus;
            partograph.UpdatedAt = DateTimeOffset.UtcNow;

            _context.Partographs.Update(partograph);
            await _context.SaveChangesAsync();

            var response = MapToResponse(partograph);
            _logger.LogInformation("Partograph {PartographId} updated", id);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating partograph");
            return BadRequest(new ErrorResponse { Message = "Error updating partograph", Details = ex.Message });
        }
    }

    /// <summary>Deletes a partograph record.</summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeletePartograph(Guid id)
    {
        try
        {
            var partograph = await _context.Partographs.FindAsync(id);
            if (partograph == null)
            {
                return NotFound(new ErrorResponse { Message = "Partograph not found" });
            }

            _context.Partographs.Remove(partograph);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Partograph {PartographId} deleted", id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting partograph");
            return BadRequest(new ErrorResponse { Message = "Error deleting partograph", Details = ex.Message });
        }
    }

    private PartographResponse MapToResponse(Partograph partograph)
    {
        return new PartographResponse
        {
            Id = partograph.Id,
            AppointmentId = partograph.AppointmentId,
            AdmissionDate = partograph.AdmissionDate,
            AdmissionTime = partograph.AdmissionTime,
            OnsetOfLaborDate = partograph.OnsetOfLaborDate,
            OnsetOfLaborTime = partograph.OnsetOfLaborTime,
            RuptureOfMembranesDate = partograph.RuptureOfMembranesDate,
            RuptureOfMembranesTime = partograph.RuptureOfMembranesTime,
            CervicalDilation = partograph.CervicalDilation,
            CervicalEffacement = partograph.CervicalEffacement,
            FetalDescent = partograph.FetalDescent,
            Molding = partograph.Molding,
            FetalHeartRateReadings = partograph.FetalHeartRateReadings,
            AmnioticFluidStatus = partograph.AmnioticFluidStatus,
            MoldingMonitoring = partograph.MoldingMonitoring,
            PulseRateReadings = partograph.PulseRateReadings,
            BloodPressureReadings = partograph.BloodPressureReadings,
            TemperatureReadings = partograph.TemperatureReadings,
            UrineOutput = partograph.UrineOutput,
            OxytocinAdministration = partograph.OxytocinAdministration,
            IVFluidAdministration = partograph.IVFluidAdministration,
            OtherMedications = partograph.OtherMedications,
            LaborNotes = partograph.LaborNotes,
            Complications = partograph.Complications,
            ComplicationActions = partograph.ComplicationActions,
            DeliveryDateTime = partograph.DeliveryDateTime,
            PostpartumMaternalCondition = partograph.PostpartumMaternalCondition,
            PlacentaDelivery = partograph.PlacentaDelivery,
            ThirdStageDuration = partograph.ThirdStageDuration,
            MaternalHemorrhageEstimate = partograph.MaternalHemorrhageEstimate,
            PerinealCondition = partograph.PerinealCondition,
            BladderStatus = partograph.BladderStatus,
            UterineContractionStatus = partograph.UterineContractionStatus,
            CreatedAt = partograph.CreatedAt.DateTime,
            UpdatedAt = partograph.UpdatedAt?.DateTime
        };
    }
}
