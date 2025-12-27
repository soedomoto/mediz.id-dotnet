using MedizID.API.Common.Exceptions;
using MedizID.API.Common.Enums;
using MedizID.API.Data;
using MedizID.API.DTOs;
using MedizID.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedizID.API.Controllers.Appointments;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
[Authorize]
public class STIController : ControllerBase
{
    private readonly MedizIDDbContext _context;
    private readonly ILogger<STIController> _logger;

    public STIController(MedizIDDbContext context, ILogger<STIController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Get STI record by appointment
    /// </summary>
    [HttpGet("appointment/{appointmentId}")]
    [ProducesResponseType(typeof(STIResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSTIByAppointment(Guid appointmentId)
    {
        try
        {
            var sti = await _context.STIs
                .FirstOrDefaultAsync(s => s.AppointmentId == appointmentId);

            if (sti == null)
            {
                // Return empty record for new appointments
                return Ok(new STIResponse
                {
                    AppointmentId = appointmentId,
                    Id = Guid.Empty
                });
            }

            return Ok(MapToResponse(sti));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching STI record for appointment {appointmentId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching STI record"
            });
        }
    }

    /// <summary>
    /// Create STI record
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(STIResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateSTIRecord([FromBody] CreateSTIRequest request)
    {
        try
        {
            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(m => m.Id == request.AppointmentId);

            if (appointment == null)
            {
                throw new NotFoundException($"Appointment with ID {request.AppointmentId} not found");
            }

            var sti = new STI
            {
                Id = Guid.NewGuid(),
                AppointmentId = request.AppointmentId,
                MotherName = request.MotherName,
                VisitStatus = !string.IsNullOrEmpty(request.VisitStatus) ? Enum.Parse<STIVisitStatusEnum>(request.VisitStatus) : null,
                ReferralStatus = request.ReferralStatus,
                RiskGroup = !string.IsNullOrEmpty(request.RiskGroup) ? Enum.Parse<STIRiskGroupEnum>(request.RiskGroup) : null,
                VisitNumber = request.VisitNumber,
                VisitReason = !string.IsNullOrEmpty(request.VisitReason) ? Enum.Parse<STIVisitReasonEnum>(request.VisitReason) : null,
                PregnancyStatus = !string.IsNullOrEmpty(request.PregnancyStatus) ? Enum.Parse<STIPregnancyStatusEnum>(request.PregnancyStatus) : null,
                LastSexualContactDaysAgo = request.LastSexualContactDaysAgo,
                CondomLastContact = request.CondomLastContact,
                SexPartnerCountLastMonth = request.SexPartnerCountLastMonth,
                CondomLastMonthContact = request.CondomLastMonthContact,
                CondomWithPartner = request.CondomWithPartner,
                VaginalDouching = request.VaginalDouching,
                OtherAnamnesisNotes = request.OtherAnamnesisNotes,
                ClinicalSigns = request.ClinicalSigns,
                Diagnosis = request.Diagnosis,
                LaboratoryReferral = request.LaboratoryReferral,
                LaboratoryTests = request.LaboratoryTests,
                LaboratoryResults = request.LaboratoryResults,
                Treatment = request.Treatment,
                Partner = request.Partner,
                Notes = request.Notes,
                CreatedAt = DateTime.UtcNow
            };

            _context.STIs.Add(sti);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"STI record created: {sti.Id}");

            return CreatedAtAction(nameof(GetSTIRecord), new { stiId = sti.Id }, MapToResponse(sti));
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
            _logger.LogError(ex, "Error creating STI record");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while creating STI record"
            });
        }
    }

    /// <summary>
    /// Get STI record by ID
    /// </summary>
    [HttpGet("{stiId}")]
    [ProducesResponseType(typeof(STIResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSTIRecord(Guid stiId)
    {
        try
        {
            var sti = await _context.STIs.FirstOrDefaultAsync(s => s.Id == stiId);

            if (sti == null)
            {
                throw new NotFoundException($"STI record with ID {stiId} not found");
            }

            return Ok(MapToResponse(sti));
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
            _logger.LogError(ex, $"Error fetching STI record {stiId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching STI record"
            });
        }
    }

    /// <summary>
    /// Update STI record
    /// </summary>
    [HttpPut("{stiId}")]
    [ProducesResponseType(typeof(STIResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateSTIRecord(Guid stiId, [FromBody] UpdateSTIRequest request)
    {
        try
        {
            var sti = await _context.STIs.FirstOrDefaultAsync(s => s.Id == stiId);

            if (sti == null)
            {
                throw new NotFoundException($"STI record with ID {stiId} not found");
            }

            // Update Data Kunjungan
            if (request.MotherName != null) sti.MotherName = request.MotherName;
            if (!string.IsNullOrEmpty(request.VisitStatus)) sti.VisitStatus = Enum.Parse<STIVisitStatusEnum>(request.VisitStatus);
            if (request.ReferralStatus != null) sti.ReferralStatus = request.ReferralStatus;
            if (!string.IsNullOrEmpty(request.RiskGroup)) sti.RiskGroup = Enum.Parse<STIRiskGroupEnum>(request.RiskGroup);
            if (request.VisitNumber.HasValue) sti.VisitNumber = request.VisitNumber;
            if (!string.IsNullOrEmpty(request.VisitReason)) sti.VisitReason = Enum.Parse<STIVisitReasonEnum>(request.VisitReason);

            // Update Anamnesa
            if (!string.IsNullOrEmpty(request.PregnancyStatus)) sti.PregnancyStatus = Enum.Parse<STIPregnancyStatusEnum>(request.PregnancyStatus);
            if (request.LastSexualContactDaysAgo.HasValue) sti.LastSexualContactDaysAgo = request.LastSexualContactDaysAgo;
            if (request.CondomLastContact.HasValue) sti.CondomLastContact = request.CondomLastContact;
            if (request.SexPartnerCountLastMonth.HasValue) sti.SexPartnerCountLastMonth = request.SexPartnerCountLastMonth;
            if (request.CondomLastMonthContact.HasValue) sti.CondomLastMonthContact = request.CondomLastMonthContact;
            if (request.CondomWithPartner.HasValue) sti.CondomWithPartner = request.CondomWithPartner;
            if (request.VaginalDouching.HasValue) sti.VaginalDouching = request.VaginalDouching;
            if (request.OtherAnamnesisNotes != null) sti.OtherAnamnesisNotes = request.OtherAnamnesisNotes;

            // Update Pemeriksaan Fisik & Diagnosis
            if (request.ClinicalSigns != null) sti.ClinicalSigns = request.ClinicalSigns;
            if (request.Diagnosis != null) sti.Diagnosis = request.Diagnosis;

            // Update Laboratorium
            if (request.LaboratoryReferral.HasValue) sti.LaboratoryReferral = request.LaboratoryReferral;
            if (request.LaboratoryTests != null) sti.LaboratoryTests = request.LaboratoryTests;
            if (request.LaboratoryResults != null) sti.LaboratoryResults = request.LaboratoryResults;

            // Update Treatment & Follow-up
            if (request.Treatment != null) sti.Treatment = request.Treatment;
            if (request.Partner != null) sti.Partner = request.Partner;
            if (request.Notes != null) sti.Notes = request.Notes;

            sti.UpdatedAt = DateTime.UtcNow;

            _context.STIs.Update(sti);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"STI record updated: {sti.Id}");

            return Ok(MapToResponse(sti));
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
            _logger.LogError(ex, $"Error updating STI record {stiId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while updating STI record"
            });
        }
    }

    /// <summary>
    /// Delete STI record
    /// </summary>
    [HttpDelete("{stiId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSTIRecord(Guid stiId)
    {
        try
        {
            var sti = await _context.STIs.FirstOrDefaultAsync(s => s.Id == stiId);

            if (sti == null)
            {
                throw new NotFoundException($"STI record with ID {stiId} not found");
            }

            _context.STIs.Remove(sti);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"STI record deleted: {stiId}");

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
            _logger.LogError(ex, $"Error deleting STI record {stiId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while deleting STI record"
            });
        }
    }

    private STIResponse MapToResponse(STI sti)
    {
        return new STIResponse
        {
            Id = sti.Id,
            AppointmentId = sti.AppointmentId,
            MotherName = sti.MotherName,
            VisitStatus = sti.VisitStatus?.ToString(),
            ReferralStatus = sti.ReferralStatus,
            RiskGroup = sti.RiskGroup?.ToString(),
            VisitNumber = sti.VisitNumber,
            VisitReason = sti.VisitReason?.ToString(),
            PregnancyStatus = sti.PregnancyStatus?.ToString(),
            LastSexualContactDaysAgo = sti.LastSexualContactDaysAgo,
            CondomLastContact = sti.CondomLastContact,
            SexPartnerCountLastMonth = sti.SexPartnerCountLastMonth,
            CondomLastMonthContact = sti.CondomLastMonthContact,
            CondomWithPartner = sti.CondomWithPartner,
            VaginalDouching = sti.VaginalDouching,
            OtherAnamnesisNotes = sti.OtherAnamnesisNotes,
            ClinicalSigns = sti.ClinicalSigns,
            Diagnosis = sti.Diagnosis,
            LaboratoryReferral = sti.LaboratoryReferral,
            LaboratoryTests = sti.LaboratoryTests,
            LaboratoryResults = sti.LaboratoryResults,
            Treatment = sti.Treatment,
            Partner = sti.Partner,
            Notes = sti.Notes,
            CreatedAt = sti.CreatedAt,
            UpdatedAt = sti.UpdatedAt
        };
    }
}

