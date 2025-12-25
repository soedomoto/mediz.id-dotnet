using MedizID.API.Common.Exceptions;
using MedizID.API.Common.Enums;
using MedizID.API.Data;
using MedizID.API.DTOs;
using MedizID.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MedizID.API.Controllers.Appointments;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
[Authorize]
public class ImmunizationController : ControllerBase
{
    private readonly MedizIDDbContext _context;
    private readonly ILogger<ImmunizationController> _logger;

    public ImmunizationController(MedizIDDbContext context, ILogger<ImmunizationController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Create or update immunization record for an appointment (one-to-one relationship)
    /// </summary>
    [HttpPost("appointments/{appointmentId}/immunization")]
    [ProducesResponseType(typeof(ImmunizationResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ImmunizationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateOrUpdateImmunizationRecord(Guid appointmentId, [FromBody] CreateImmunizationRequest request)
    {
        try
        {
            var appointment = await _context.Appointments
                .Include(a => a.Immunization)
                .FirstOrDefaultAsync(a => a.Id == appointmentId);

            if (appointment == null)
            {
                throw new NotFoundException($"Appointment with ID {appointmentId} not found");
            }

            // Check if immunization already exists for this appointment
            var immunization = appointment.Immunization;
            var isNew = immunization == null;

            if (isNew)
            {
                immunization = new Immunization
                {
                    Id = Guid.NewGuid(),
                    AppointmentId = appointmentId,
                    CreatedAt = DateTime.UtcNow
                };
            }
            else
            {
                immunization.UpdatedAt = DateTime.UtcNow;
            }

            // Update all fields
            immunization.DateOfBirth = ConvertToUtc(request.DateOfBirth);
            immunization.BirthPlace = request.BirthPlace;
            immunization.Gender = request.Gender;
            immunization.Address = request.Address;
            immunization.BirthWeight = request.BirthWeight;
            immunization.BirthLength = request.BirthLength;
            immunization.FatherName = request.FatherName;
            immunization.FatherOccupation = request.FatherOccupation;
            immunization.MotherName = request.MotherName;
            immunization.MotherOccupation = request.MotherOccupation;
            immunization.DeliveryType = request.DeliveryType.HasValue ? (DeliveryType)request.DeliveryType : null;
            immunization.DeliveryPersonnel = request.DeliveryPersonnel.HasValue ? (DeliveryPersonnel)request.DeliveryPersonnel : null;
            immunization.DeliveryPlace = request.DeliveryPlace.HasValue ? (DeliveryPlace)request.DeliveryPlace : null;
            immunization.ChildNumber = request.ChildNumber;
            immunization.NeonatalVisit1 = request.NeonatalVisit1;
            immunization.NeonatalVisit2 = request.NeonatalVisit2;
            immunization.NeonatalVisit3 = request.NeonatalVisit3;
            immunization.NeonatalVisit4 = request.NeonatalVisit4;
            immunization.BreastfeedingStatus = request.BreastfeedingStatus.HasValue ? (BreastfeedingStatus)request.BreastfeedingStatus : null;
            immunization.VitaminAStatusSixMonths = request.VitaminAStatusSixMonths.HasValue ? (VitaminAStatus)request.VitaminAStatusSixMonths : null;
            immunization.VaccineType = request.VaccineType.HasValue ? (VaccineType)request.VaccineType : null;
            immunization.VaccineName = request.VaccineName;
            immunization.VaccineDate = ConvertToUtc(request.VaccineDate);
            immunization.DoseNumber = request.DoseNumber;
            immunization.Lot = request.Lot;
            immunization.Route = request.Route.HasValue ? (VaccineRoute)request.Route : null;
            immunization.Site = request.Site.HasValue ? (VaccineSite)request.Site : null;
            immunization.Reactions = request.Reactions;
            immunization.ReactionSeverity = request.ReactionSeverity.HasValue ? (VaccineReactionSeverity)request.ReactionSeverity : null;
            immunization.AgeCategory = request.AgeCategory.HasValue ? (ImmunizationAgeCategory)request.AgeCategory : null;
            immunization.ServiceDate = ConvertToUtc(request.ServiceDate);
            immunization.AgeYears = request.AgeYears;
            immunization.DoctorName = request.DoctorName;
            immunization.NurseName = request.NurseName;
            immunization.ProviderId = request.ProviderId;
            immunization.NurseId = request.NurseId;

            if (isNew)
            {
                _context.Immunizations.Add(immunization);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Immunization record created: {immunization.Id}");
                return CreatedAtAction(nameof(GetImmunizationRecord), 
                    new { appointmentId = appointmentId }, 
                    MapToResponse(immunization));
            }
            else
            {
                _context.Immunizations.Update(immunization);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Immunization record updated: {immunization.Id}");
                return Ok(MapToResponse(immunization));
            }
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
            _logger.LogError(ex, "Error creating or updating immunization record");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while creating or updating immunization record"
            });
        }
    }

    /// <summary>
    /// Get immunization record for an appointment (one-to-one relationship)
    /// </summary>
    [HttpGet("appointments/{appointmentId}/immunization")]
    [ProducesResponseType(typeof(ImmunizationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetImmunizationRecord(Guid appointmentId)
    {
        try
        {
            var immunization = await _context.Immunizations
                .FirstOrDefaultAsync(i => i.AppointmentId == appointmentId);

            if (immunization == null)
            {
                throw new NotFoundException($"Immunization record for appointment {appointmentId} not found");
            }

            return Ok(MapToResponse(immunization));
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
            _logger.LogError(ex, "Error fetching immunization record");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching immunization record"
            });
        }
    }

    /// <summary>
    /// Update immunization record for an appointment
    /// </summary>
    [HttpPut("appointments/{appointmentId}/immunization")]
    [ProducesResponseType(typeof(ImmunizationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateImmunizationRecord(Guid appointmentId, [FromBody] UpdateImmunizationRequest request)
    {
        try
        {
            var immunization = await _context.Immunizations
                .FirstOrDefaultAsync(i => i.AppointmentId == appointmentId);

            if (immunization == null)
            {
                throw new NotFoundException($"Immunization record for appointment {appointmentId} not found");
            }

            // Update fields
            immunization.DateOfBirth = request.DateOfBirth ?? immunization.DateOfBirth;
            immunization.BirthPlace = request.BirthPlace ?? immunization.BirthPlace;
            immunization.Gender = request.Gender ?? immunization.Gender;
            immunization.Address = request.Address ?? immunization.Address;
            immunization.BirthWeight = request.BirthWeight ?? immunization.BirthWeight;
            immunization.BirthLength = request.BirthLength ?? immunization.BirthLength;
            immunization.FatherName = request.FatherName ?? immunization.FatherName;
            immunization.FatherOccupation = request.FatherOccupation ?? immunization.FatherOccupation;
            immunization.MotherName = request.MotherName ?? immunization.MotherName;
            immunization.MotherOccupation = request.MotherOccupation ?? immunization.MotherOccupation;
            
            if (request.DeliveryType.HasValue)
                immunization.DeliveryType = (DeliveryType)request.DeliveryType;
            if (request.DeliveryPersonnel.HasValue)
                immunization.DeliveryPersonnel = (DeliveryPersonnel)request.DeliveryPersonnel;
            if (request.DeliveryPlace.HasValue)
                immunization.DeliveryPlace = (DeliveryPlace)request.DeliveryPlace;
                
            immunization.ChildNumber = request.ChildNumber ?? immunization.ChildNumber;
            immunization.NeonatalVisit1 = request.NeonatalVisit1 ?? immunization.NeonatalVisit1;
            immunization.NeonatalVisit2 = request.NeonatalVisit2 ?? immunization.NeonatalVisit2;
            immunization.NeonatalVisit3 = request.NeonatalVisit3 ?? immunization.NeonatalVisit3;
            immunization.NeonatalVisit4 = request.NeonatalVisit4 ?? immunization.NeonatalVisit4;
            
            if (request.BreastfeedingStatus.HasValue)
                immunization.BreastfeedingStatus = (BreastfeedingStatus)request.BreastfeedingStatus;
            if (request.VitaminAStatusSixMonths.HasValue)
                immunization.VitaminAStatusSixMonths = (VitaminAStatus)request.VitaminAStatusSixMonths;
            if (request.VaccineType.HasValue)
                immunization.VaccineType = (VaccineType)request.VaccineType;
                
            immunization.VaccineName = request.VaccineName ?? immunization.VaccineName;
            if (request.VaccineDate.HasValue)
                immunization.VaccineDate = request.VaccineDate.Value;
            immunization.DoseNumber = request.DoseNumber ?? immunization.DoseNumber;
            immunization.Lot = request.Lot ?? immunization.Lot;
            
            if (request.Route.HasValue)
                immunization.Route = (VaccineRoute)request.Route;
            if (request.Site.HasValue)
                immunization.Site = (VaccineSite)request.Site;
                
            immunization.Reactions = request.Reactions ?? immunization.Reactions;
            
            if (request.ReactionSeverity.HasValue)
                immunization.ReactionSeverity = (VaccineReactionSeverity)request.ReactionSeverity;
            if (request.AgeCategory.HasValue)
                immunization.AgeCategory = (ImmunizationAgeCategory)request.AgeCategory;
                
            immunization.ServiceDate = request.ServiceDate ?? immunization.ServiceDate;
            immunization.AgeYears = request.AgeYears ?? immunization.AgeYears;
            immunization.DoctorName = request.DoctorName ?? immunization.DoctorName;
            immunization.NurseName = request.NurseName ?? immunization.NurseName;
            immunization.ProviderId = request.ProviderId ?? immunization.ProviderId;
            immunization.NurseId = request.NurseId ?? immunization.NurseId;
            immunization.UpdatedAt = DateTime.UtcNow;

            _context.Immunizations.Update(immunization);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Immunization record updated: {immunization.Id}");

            return Ok(MapToResponse(immunization));
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
            _logger.LogError(ex, "Error updating immunization record");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while updating immunization record"
            });
        }
    }

    /// <summary>
    /// Delete immunization record for an appointment
    /// </summary>
    [HttpDelete("appointments/{appointmentId}/immunization")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteImmunizationRecord(Guid appointmentId)
    {
        try
        {
            var immunization = await _context.Immunizations
                .FirstOrDefaultAsync(i => i.AppointmentId == appointmentId);

            if (immunization == null)
            {
                throw new NotFoundException($"Immunization record for appointment {appointmentId} not found");
            }

            _context.Immunizations.Remove(immunization);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Immunization record deleted: {immunization.Id}");

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
            _logger.LogError(ex, "Error deleting immunization record");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while deleting immunization record"
            });
        }
    }

    /// <summary>
    /// Get enum reference data for dropdowns
    /// </summary>
    [HttpGet("enums")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public IActionResult GetEnumData()
    {
        try
        {
            var vaccineTypes = Enum.GetValues(typeof(VaccineType))
                .Cast<VaccineType>()
                .Select(e => new { value = (int)e, label = GetEnumDisplay(e) })
                .ToList();

            var routes = Enum.GetValues(typeof(VaccineRoute))
                .Cast<VaccineRoute>()
                .Select(e => new { value = (int)e, label = GetEnumDisplay(e) })
                .ToList();

            var sites = Enum.GetValues(typeof(VaccineSite))
                .Cast<VaccineSite>()
                .Select(e => new { value = (int)e, label = GetEnumDisplay(e) })
                .ToList();

            var ageCategories = Enum.GetValues(typeof(ImmunizationAgeCategory))
                .Cast<ImmunizationAgeCategory>()
                .Select(e => new { value = (int)e, label = GetEnumDisplay(e) })
                .ToList();

            var reactions = Enum.GetValues(typeof(VaccineReactionSeverity))
                .Cast<VaccineReactionSeverity>()
                .Select(e => new { value = (int)e, label = GetEnumDisplay(e) })
                .ToList();

            var deliveryTypes = Enum.GetValues(typeof(DeliveryType))
                .Cast<DeliveryType>()
                .Select(e => new { value = (int)e, label = GetEnumDisplay(e) })
                .ToList();

            var deliveryPersonnel = Enum.GetValues(typeof(DeliveryPersonnel))
                .Cast<DeliveryPersonnel>()
                .Select(e => new { value = (int)e, label = GetEnumDisplay(e) })
                .ToList();

            var deliveryPlaces = Enum.GetValues(typeof(DeliveryPlace))
                .Cast<DeliveryPlace>()
                .Select(e => new { value = (int)e, label = GetEnumDisplay(e) })
                .ToList();

            var breastfeedingStatus = Enum.GetValues(typeof(BreastfeedingStatus))
                .Cast<BreastfeedingStatus>()
                .Select(e => new { value = (int)e, label = GetEnumDisplay(e) })
                .ToList();

            var vitaminAStatus = Enum.GetValues(typeof(VitaminAStatus))
                .Cast<VitaminAStatus>()
                .Select(e => new { value = (int)e, label = GetEnumDisplay(e) })
                .ToList();

            return Ok(new
            {
                vaccineTypes,
                routes,
                sites,
                ageCategories,
                reactions,
                deliveryTypes,
                deliveryPersonnel,
                deliveryPlaces,
                breastfeedingStatus,
                vitaminAStatus
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching enum data");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching enum data"
            });
        }
    }

    private ImmunizationResponse MapToResponse(Immunization immunization)
    {
        return new ImmunizationResponse
        {
            Id = immunization.Id,
            AppointmentId = immunization.AppointmentId,
            DateOfBirth = immunization.DateOfBirth,
            BirthPlace = immunization.BirthPlace,
            Gender = immunization.Gender,
            Address = immunization.Address,
            BirthWeight = immunization.BirthWeight,
            BirthLength = immunization.BirthLength,
            FatherName = immunization.FatherName,
            FatherOccupation = immunization.FatherOccupation,
            MotherName = immunization.MotherName,
            MotherOccupation = immunization.MotherOccupation,
            DeliveryType = (int?)immunization.DeliveryType,
            DeliveryTypeLabel = immunization.DeliveryType.HasValue ? GetEnumDisplay(immunization.DeliveryType) : null,
            DeliveryPersonnel = (int?)immunization.DeliveryPersonnel,
            DeliveryPersonnelLabel = immunization.DeliveryPersonnel.HasValue ? GetEnumDisplay(immunization.DeliveryPersonnel) : null,
            DeliveryPlace = (int?)immunization.DeliveryPlace,
            DeliveryPlaceLabel = immunization.DeliveryPlace.HasValue ? GetEnumDisplay(immunization.DeliveryPlace) : null,
            ChildNumber = immunization.ChildNumber,
            NeonatalVisit1 = immunization.NeonatalVisit1,
            NeonatalVisit2 = immunization.NeonatalVisit2,
            NeonatalVisit3 = immunization.NeonatalVisit3,
            NeonatalVisit4 = immunization.NeonatalVisit4,
            BreastfeedingStatus = (int?)immunization.BreastfeedingStatus,
            BreastfeedingStatusLabel = immunization.BreastfeedingStatus.HasValue ? GetEnumDisplay(immunization.BreastfeedingStatus) : null,
            VitaminAStatusSixMonths = (int?)immunization.VitaminAStatusSixMonths,
            VitaminAStatusLabel = immunization.VitaminAStatusSixMonths.HasValue ? GetEnumDisplay(immunization.VitaminAStatusSixMonths) : null,
            VaccineType = (int?)immunization.VaccineType,
            VaccineTypeLabel = immunization.VaccineType.HasValue ? GetEnumDisplay(immunization.VaccineType) : null,
            VaccineName = immunization.VaccineName,
            VaccineDate = immunization.VaccineDate,
            DoseNumber = immunization.DoseNumber,
            Lot = immunization.Lot,
            Route = (int?)immunization.Route,
            RouteLabel = immunization.Route.HasValue ? GetEnumDisplay(immunization.Route) : null,
            Site = (int?)immunization.Site,
            SiteLabel = immunization.Site.HasValue ? GetEnumDisplay(immunization.Site) : null,
            Reactions = immunization.Reactions,
            ReactionSeverity = (int?)immunization.ReactionSeverity,
            ReactionSeverityLabel = immunization.ReactionSeverity.HasValue ? GetEnumDisplay(immunization.ReactionSeverity) : null,
            AgeCategory = (int?)immunization.AgeCategory,
            AgeCategoryLabel = immunization.AgeCategory.HasValue ? GetEnumDisplay(immunization.AgeCategory) : null,
            ServiceDate = immunization.ServiceDate,
            AgeYears = immunization.AgeYears,
            DoctorName = immunization.DoctorName,
            NurseName = immunization.NurseName,
            ProviderId = immunization.ProviderId,
            NurseId = immunization.NurseId,
            CreatedAt = immunization.CreatedAt,
            UpdatedAt = immunization.UpdatedAt
        };
    }

    private string GetEnumDisplay(object enumValue)
    {
        var field = enumValue?.GetType().GetField(enumValue?.ToString() ?? "");
        var attribute = field?.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() as DisplayAttribute;
        return attribute?.Name ?? enumValue?.ToString() ?? "";
    }

    private DateTime? ConvertToUtc(DateTimeOffset? dateTime)
    {
        if (dateTime == null)
            return null;

        // Convert to UTC DateTime
        return dateTime.Value.UtcDateTime;
    }
}
