using MedizID.API.Common.Exceptions;
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
public class HIVCounselingController : ControllerBase
{
    private readonly MedizIDDbContext _context;
    private readonly ILogger<HIVCounselingController> _logger;

    public HIVCounselingController(MedizIDDbContext context, ILogger<HIVCounselingController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Get HIV counseling record by appointment ID
    /// </summary>
    [HttpGet("appointment/{appointmentId}")]
    [ProducesResponseType(typeof(HIVCounselingResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByAppointmentId(Guid appointmentId)
    {
        try
        {
            var record = await _context.HIVCounselings
                .Include(h => h.Appointment)
                .FirstOrDefaultAsync(h => h.AppointmentId == appointmentId);

            if (record == null)
            {
                _logger.LogInformation($"No HIV counseling record found for appointment: {appointmentId}");
                return NotFound(new ErrorResponse
                {
                    ErrorCode = "NOT_FOUND",
                    Message = $"HIV counseling record for appointment {appointmentId} not found"
                });
            }

            var response = MapToResponse(record);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching HIV counseling record for appointment {appointmentId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching the HIV counseling record"
            });
        }
    }

    /// <summary>
    /// Get HIV counseling record by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(HIVCounselingResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var record = await _context.HIVCounselings
                .Include(h => h.Appointment)
                .FirstOrDefaultAsync(h => h.Id == id);

            if (record == null)
            {
                _logger.LogWarning($"HIV counseling record not found: {id}");
                return NotFound(new ErrorResponse
                {
                    ErrorCode = "NOT_FOUND",
                    Message = $"HIV counseling record with ID {id} not found"
                });
            }

            var response = MapToResponse(record);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching HIV counseling record {id}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching the HIV counseling record"
            });
        }
    }

    /// <summary>
    /// Create a new HIV counseling record
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(HIVCounselingResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateHIVCounselingRequest request)
    {
        try
        {
            if (request == null || request.AppointmentId == Guid.Empty)
            {
                return BadRequest(new ErrorResponse
                {
                    ErrorCode = "INVALID_REQUEST",
                    Message = "Appointment ID is required"
                });
            }

            // Check if appointment exists
            var appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.Id == request.AppointmentId);
            if (appointment == null)
            {
                return BadRequest(new ErrorResponse
                {
                    ErrorCode = "INVALID_APPOINTMENT",
                    Message = $"Appointment with ID {request.AppointmentId} not found"
                });
            }

            // Check if record already exists
            var existing = await _context.HIVCounselings
                .FirstOrDefaultAsync(h => h.AppointmentId == request.AppointmentId);
            if (existing != null)
            {
                return BadRequest(new ErrorResponse
                {
                    ErrorCode = "RECORD_EXISTS",
                    Message = "HIV counseling record already exists for this appointment"
                });
            }

            var record = new HIVCounseling
            {
                Id = Guid.NewGuid(),
                AppointmentId = request.AppointmentId,
                VisitStatus = request.VisitStatus,
                MotherName = request.MotherName,
                PregnancyStatus = request.PregnancyStatus,
                LastChildAge = request.LastChildAge,
                NumberOfChildren = request.NumberOfChildren,
                ReferralStatus = request.ReferralStatus,
                RiskGroup = request.RiskGroup,
                RiskGroupStartDate = request.RiskGroupStartDate,
                HasRegularPartner = request.HasRegularPartner,
                HasFemalePartner = request.HasFemalePartner,
                PartnerPregnant = request.PartnerPregnant,
                PartnerName = request.PartnerName,
                PartnerDateOfBirth = request.PartnerDateOfBirth,
                PartnerHIVStatus = request.PartnerHIVStatus,
                PartnerLastTestDate = request.PartnerLastTestDate,
                IsIncarcerated = request.IsIncarcerated,
                PreTestCounselingDate = request.PreTestCounselingDate,
                ClientStatus = request.ClientStatus,
                TestReasons = request.TestReasons != null ? string.Join(",", request.TestReasons) : null,
                TestKnowledgeSource = request.TestKnowledgeSource,
                VaginalSexRisk = request.VaginalSexRisk,
                VaginalSexRiskDate = request.VaginalSexRiskDate,
                AnalSexRisk = request.AnalSexRisk,
                AnalSexRiskDate = request.AnalSexRiskDate,
                SharedNeedlesRisk = request.SharedNeedlesRisk,
                SharedNeedlesRiskDate = request.SharedNeedlesRiskDate,
                BloodTransfusionRisk = request.BloodTransfusionRisk,
                BloodTransfusionRiskDate = request.BloodTransfusionRiskDate,
                MotherToChildRisk = request.MotherToChildRisk,
                MotherToChildRiskDate = request.MotherToChildRiskDate,
                OtherRiskDescription = request.OtherRiskDescription,
                OtherRiskDate = request.OtherRiskDate,
                WindowPeriodRisk = request.WindowPeriodRisk,
                WindowPeriodRiskDate = request.WindowPeriodRiskDate,
                WillingToTest = request.WillingToTest,
                PreviouslyTested = request.PreviouslyTested,
                PreviousTestLocation = request.PreviousTestLocation,
                PreviousTestDate = request.PreviousTestDate,
                PreviousTestResult = request.PreviousTestResult,
                ObservationNotes = request.ObservationNotes,
                CreatedAt = DateTime.UtcNow
            };

            _context.HIVCounselings.Add(record);
            await _context.SaveChangesAsync();

            var response = MapToResponse(record);
            return CreatedAtAction(nameof(GetById), new { id = record.Id }, response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating HIV counseling record");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while creating the HIV counseling record"
            });
        }
    }

    /// <summary>
    /// Update an existing HIV counseling record
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(HIVCounselingResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateHIVCounselingRequest request)
    {
        try
        {
            var record = await _context.HIVCounselings.FirstOrDefaultAsync(h => h.Id == id);
            if (record == null)
            {
                return NotFound(new ErrorResponse
                {
                    ErrorCode = "NOT_FOUND",
                    Message = $"HIV counseling record with ID {id} not found"
                });
            }

            // Update properties
            if (request.VisitStatus != null) record.VisitStatus = request.VisitStatus;
            if (request.MotherName != null) record.MotherName = request.MotherName;
            if (request.PregnancyStatus != null) record.PregnancyStatus = request.PregnancyStatus;
            if (request.LastChildAge.HasValue) record.LastChildAge = request.LastChildAge;
            if (request.NumberOfChildren.HasValue) record.NumberOfChildren = request.NumberOfChildren;
            if (request.ReferralStatus != null) record.ReferralStatus = request.ReferralStatus;
            if (request.RiskGroup != null) record.RiskGroup = request.RiskGroup;
            if (request.RiskGroupStartDate.HasValue) record.RiskGroupStartDate = request.RiskGroupStartDate;
            if (request.HasRegularPartner.HasValue) record.HasRegularPartner = request.HasRegularPartner;
            if (request.HasFemalePartner.HasValue) record.HasFemalePartner = request.HasFemalePartner;
            if (request.PartnerPregnant.HasValue) record.PartnerPregnant = request.PartnerPregnant;
            if (request.PartnerName != null) record.PartnerName = request.PartnerName;
            if (request.PartnerDateOfBirth.HasValue) record.PartnerDateOfBirth = request.PartnerDateOfBirth;
            if (request.PartnerHIVStatus != null) record.PartnerHIVStatus = request.PartnerHIVStatus;
            if (request.PartnerLastTestDate.HasValue) record.PartnerLastTestDate = request.PartnerLastTestDate;
            if (request.IsIncarcerated.HasValue) record.IsIncarcerated = request.IsIncarcerated;
            if (request.PreTestCounselingDate.HasValue) record.PreTestCounselingDate = request.PreTestCounselingDate;
            if (request.ClientStatus != null) record.ClientStatus = request.ClientStatus;
            if (request.TestReasons != null) record.TestReasons = string.Join(",", request.TestReasons);
            if (request.TestKnowledgeSource != null) record.TestKnowledgeSource = request.TestKnowledgeSource;
            if (request.VaginalSexRisk.HasValue) record.VaginalSexRisk = request.VaginalSexRisk;
            if (request.VaginalSexRiskDate.HasValue) record.VaginalSexRiskDate = request.VaginalSexRiskDate;
            if (request.AnalSexRisk.HasValue) record.AnalSexRisk = request.AnalSexRisk;
            if (request.AnalSexRiskDate.HasValue) record.AnalSexRiskDate = request.AnalSexRiskDate;
            if (request.SharedNeedlesRisk.HasValue) record.SharedNeedlesRisk = request.SharedNeedlesRisk;
            if (request.SharedNeedlesRiskDate.HasValue) record.SharedNeedlesRiskDate = request.SharedNeedlesRiskDate;
            if (request.BloodTransfusionRisk.HasValue) record.BloodTransfusionRisk = request.BloodTransfusionRisk;
            if (request.BloodTransfusionRiskDate.HasValue) record.BloodTransfusionRiskDate = request.BloodTransfusionRiskDate;
            if (request.MotherToChildRisk.HasValue) record.MotherToChildRisk = request.MotherToChildRisk;
            if (request.MotherToChildRiskDate.HasValue) record.MotherToChildRiskDate = request.MotherToChildRiskDate;
            if (request.OtherRiskDescription != null) record.OtherRiskDescription = request.OtherRiskDescription;
            if (request.OtherRiskDate.HasValue) record.OtherRiskDate = request.OtherRiskDate;
            if (request.WindowPeriodRisk.HasValue) record.WindowPeriodRisk = request.WindowPeriodRisk;
            if (request.WindowPeriodRiskDate.HasValue) record.WindowPeriodRiskDate = request.WindowPeriodRiskDate;
            if (request.WillingToTest.HasValue) record.WillingToTest = request.WillingToTest;
            if (request.PreviouslyTested.HasValue) record.PreviouslyTested = request.PreviouslyTested;
            if (request.PreviousTestLocation != null) record.PreviousTestLocation = request.PreviousTestLocation;
            if (request.PreviousTestDate.HasValue) record.PreviousTestDate = request.PreviousTestDate;
            if (request.PreviousTestResult != null) record.PreviousTestResult = request.PreviousTestResult;
            if (request.ObservationNotes != null) record.ObservationNotes = request.ObservationNotes;

            _context.HIVCounselings.Update(record);
            await _context.SaveChangesAsync();

            var response = MapToResponse(record);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating HIV counseling record {id}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while updating the HIV counseling record"
            });
        }
    }

    private HIVCounselingResponse MapToResponse(HIVCounseling record)
    {
        var testReasons = new List<string>();
        if (!string.IsNullOrEmpty(record.TestReasons))
        {
            testReasons = record.TestReasons.Split(",").ToList();
        }

        return new HIVCounselingResponse
        {
            Id = record.Id,
            AppointmentId = record.AppointmentId,
            VisitStatus = record.VisitStatus,
            MotherName = record.MotherName,
            PregnancyStatus = record.PregnancyStatus,
            LastChildAge = record.LastChildAge,
            NumberOfChildren = record.NumberOfChildren,
            ReferralStatus = record.ReferralStatus,
            RiskGroup = record.RiskGroup,
            RiskGroupStartDate = record.RiskGroupStartDate,
            HasRegularPartner = record.HasRegularPartner,
            HasFemalePartner = record.HasFemalePartner,
            PartnerPregnant = record.PartnerPregnant,
            PartnerName = record.PartnerName,
            PartnerDateOfBirth = record.PartnerDateOfBirth,
            PartnerHIVStatus = record.PartnerHIVStatus,
            PartnerLastTestDate = record.PartnerLastTestDate,
            IsIncarcerated = record.IsIncarcerated,
            PreTestCounselingDate = record.PreTestCounselingDate,
            ClientStatus = record.ClientStatus,
            TestReasons = testReasons,
            TestKnowledgeSource = record.TestKnowledgeSource,
            VaginalSexRisk = record.VaginalSexRisk,
            VaginalSexRiskDate = record.VaginalSexRiskDate,
            AnalSexRisk = record.AnalSexRisk,
            AnalSexRiskDate = record.AnalSexRiskDate,
            SharedNeedlesRisk = record.SharedNeedlesRisk,
            SharedNeedlesRiskDate = record.SharedNeedlesRiskDate,
            BloodTransfusionRisk = record.BloodTransfusionRisk,
            BloodTransfusionRiskDate = record.BloodTransfusionRiskDate,
            MotherToChildRisk = record.MotherToChildRisk,
            MotherToChildRiskDate = record.MotherToChildRiskDate,
            OtherRiskDescription = record.OtherRiskDescription,
            OtherRiskDate = record.OtherRiskDate,
            WindowPeriodRisk = record.WindowPeriodRisk,
            WindowPeriodRiskDate = record.WindowPeriodRiskDate,
            WillingToTest = record.WillingToTest,
            PreviouslyTested = record.PreviouslyTested,
            PreviousTestLocation = record.PreviousTestLocation,
            PreviousTestDate = record.PreviousTestDate,
            PreviousTestResult = record.PreviousTestResult,
            ObservationNotes = record.ObservationNotes,
            CreatedAt = record.CreatedAt
        };
    }
}
