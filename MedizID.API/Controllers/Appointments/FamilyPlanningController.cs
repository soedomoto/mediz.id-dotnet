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
public class FamilyPlanningController : ControllerBase
{
    private readonly MedizIDDbContext _context;
    private readonly ILogger<FamilyPlanningController> _logger;

    public FamilyPlanningController(MedizIDDbContext context, ILogger<FamilyPlanningController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Get all family planning records with pagination
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<FamilyPlanningResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFamilyPlanning(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] Guid? appointmentId = null)
    {
        try
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var query = _context.FamilyPlannings.AsQueryable();

            if (appointmentId.HasValue)
                query = query.Where(fp => fp.AppointmentId == appointmentId.Value);

            var total = await query.CountAsync();

            var records = await query
                .OrderByDescending(fp => fp.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(fp => fp.ContraceptiveMethods)
                .Include(fp => fp.LaboratoryResults)
                .Include(fp => fp.Procedures)
                .Select(fp => new FamilyPlanningResponse
                {
                    Id = fp.Id,
                    AppointmentId = fp.AppointmentId,
                    SpouseName = fp.SpouseName,
                    HusbandEducation = fp.HusbandEducation,
                    WifeEducation = fp.WifeEducation,
                    HusbandOccupation = fp.HusbandOccupation,
                    WifeOccupation = fp.WifeOccupation,
                    FamilyPlanningStage = fp.FamilyPlanningStage,
                    NumberOfLivingChildren = fp.NumberOfLivingChildren,
                    YoungestChildYears = fp.YoungestChildYears,
                    YoungestChildMonths = fp.YoungestChildMonths,
                    KBParticipantStatus = fp.KBParticipantStatus,
                    LastContraceptiveMethod = fp.LastContraceptiveMethod,
                    PregnancySigns = fp.PregnancySigns,
                    AbnormalVaginalDischarge = fp.AbnormalVaginalDischarge,
                    AbdominalPain = fp.AbdominalPain,
                    EctopicPregnancyHistory = fp.EctopicPregnancyHistory,
                    AbnormalUterinebleeding = fp.AbnormalUterinebleeding,
                    IUDStillInPlace = fp.IUDStillInPlace,
                    PelvicPain = fp.PelvicPain,
                    Dysmenorrhea = fp.Dysmenorrhea,
                    InflammationSigns = fp.InflammationSigns,
                    TumorOrMalignancy = fp.TumorOrMalignancy,
                    UterinePosition = fp.UterinePosition,
                    DiabetesSigns = fp.DiabetesSigns,
                    BloodClottingDisorder = fp.BloodClottingDisorder,
                    OrchitisEpididymitis = fp.OrchitisEpididymitis,
                    TumorOrMalignancyMOP = fp.TumorOrMalignancyMOP,
                    AllowedContraceptiveMethods = fp.AllowedContraceptiveMethods,
                    SelectedContraceptiveMethod = fp.SelectedContraceptiveMethod,
                    ServiceDate = fp.ServiceDate,
                    FollowUpDate = fp.FollowUpDate,
                    RemovalDate = fp.RemovalDate,
                    ObservationNotes = fp.ObservationNotes,
                    CreatedAt = fp.CreatedAt,
                    UpdatedAt = fp.UpdatedAt,
                    ContraceptiveMethods = fp.ContraceptiveMethods.Select(cm => new FamilyPlanningContraceptiveMethodDto
                    {
                        Id = cm.Id,
                        FamilyPlanningId = cm.FamilyPlanningId,
                        MethodName = cm.MethodName,
                        ServiceDate = cm.ServiceDate,
                        Quantity = cm.Quantity,
                        Notes = cm.Notes,
                        CreatedAt = cm.CreatedAt,
                        UpdatedAt = cm.UpdatedAt
                    }).ToList(),
                    LaboratoryResults = fp.LaboratoryResults.Select(lr => new FamilyPlanningLaboratoryResultDto
                    {
                        Id = lr.Id,
                        FamilyPlanningId = lr.FamilyPlanningId,
                        TestName = lr.TestName,
                        Result = lr.Result,
                        ReferenceValue = lr.ReferenceValue,
                        TestDate = lr.TestDate,
                        Notes = lr.Notes,
                        CreatedAt = lr.CreatedAt,
                        UpdatedAt = lr.UpdatedAt
                    }).ToList(),
                    Procedures = fp.Procedures.Select(p => new FamilyPlanningProcedureDto
                    {
                        Id = p.Id,
                        FamilyPlanningId = p.FamilyPlanningId,
                        ProcedureName = p.ProcedureName,
                        ProcedureDate = p.ProcedureDate,
                        PerformedBy = p.PerformedBy,
                        Outcome = p.Outcome,
                        Complications = p.Complications,
                        Notes = p.Notes,
                        CreatedAt = p.CreatedAt,
                        UpdatedAt = p.UpdatedAt
                    }).ToList()
                })
                .ToListAsync();

            return Ok(new PagedResult<FamilyPlanningResponse>
            {
                Items = records,
                Total = total,
                Page = page,
                PageSize = pageSize
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching family planning records");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching family planning records"
            });
        }
    }

    /// <summary>
    /// Get family planning record by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(FamilyPlanningResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetFamilyPlanningRecord(Guid id)
    {
        try
        {
            var record = await _context.FamilyPlannings
                .Include(fp => fp.Appointment)
                .FirstOrDefaultAsync(fp => fp.Id == id);

            if (record == null)
            {
                _logger.LogWarning($"Family planning record not found: {id}");
                throw new NotFoundException($"Family planning record with ID {id} not found");
            }

            var response = new FamilyPlanningResponse
            {
                Id = record.Id,
                AppointmentId = record.AppointmentId,
                SpouseName = record.SpouseName,
                HusbandEducation = record.HusbandEducation,
                WifeEducation = record.WifeEducation,
                HusbandOccupation = record.HusbandOccupation,
                WifeOccupation = record.WifeOccupation,
                FamilyPlanningStage = record.FamilyPlanningStage,
                NumberOfLivingChildren = record.NumberOfLivingChildren,
                YoungestChildYears = record.YoungestChildYears,
                YoungestChildMonths = record.YoungestChildMonths,
                KBParticipantStatus = record.KBParticipantStatus,
                LastContraceptiveMethod = record.LastContraceptiveMethod,
                PregnancySigns = record.PregnancySigns,
                AbnormalVaginalDischarge = record.AbnormalVaginalDischarge,
                AbdominalPain = record.AbdominalPain,
                EctopicPregnancyHistory = record.EctopicPregnancyHistory,
                AbnormalUterinebleeding = record.AbnormalUterinebleeding,
                IUDStillInPlace = record.IUDStillInPlace,
                PelvicPain = record.PelvicPain,
                Dysmenorrhea = record.Dysmenorrhea,
                InflammationSigns = record.InflammationSigns,
                TumorOrMalignancy = record.TumorOrMalignancy,
                UterinePosition = record.UterinePosition,
                DiabetesSigns = record.DiabetesSigns,
                BloodClottingDisorder = record.BloodClottingDisorder,
                OrchitisEpididymitis = record.OrchitisEpididymitis,
                TumorOrMalignancyMOP = record.TumorOrMalignancyMOP,
                AllowedContraceptiveMethods = record.AllowedContraceptiveMethods,
                SelectedContraceptiveMethod = record.SelectedContraceptiveMethod,
                ServiceDate = record.ServiceDate,
                FollowUpDate = record.FollowUpDate,
                RemovalDate = record.RemovalDate,
                ObservationNotes = record.ObservationNotes,
                CreatedAt = record.CreatedAt,
                UpdatedAt = record.UpdatedAt
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
            _logger.LogError(ex, $"Error fetching family planning record {id}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching the family planning record"
            });
        }
    }

    /// <summary>
    /// Get family planning record by appointment ID
    /// </summary>
    [HttpGet("appointment/{appointmentId}")]
    [ProducesResponseType(typeof(FamilyPlanningResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetFamilyPlanningByAppointmentId(Guid appointmentId)
    {
        try
        {
            var record = await _context.FamilyPlannings
                .Include(fp => fp.Appointment)
                .FirstOrDefaultAsync(fp => fp.AppointmentId == appointmentId);

            if (record == null)
            {
                _logger.LogWarning($"Family planning record not found for appointment: {appointmentId}");
                throw new NotFoundException($"Family planning record for appointment {appointmentId} not found");
            }

            var response = new FamilyPlanningResponse
            {
                Id = record.Id,
                AppointmentId = record.AppointmentId,
                SpouseName = record.SpouseName,
                HusbandEducation = record.HusbandEducation,
                WifeEducation = record.WifeEducation,
                HusbandOccupation = record.HusbandOccupation,
                WifeOccupation = record.WifeOccupation,
                FamilyPlanningStage = record.FamilyPlanningStage,
                NumberOfLivingChildren = record.NumberOfLivingChildren,
                YoungestChildYears = record.YoungestChildYears,
                YoungestChildMonths = record.YoungestChildMonths,
                KBParticipantStatus = record.KBParticipantStatus,
                LastContraceptiveMethod = record.LastContraceptiveMethod,
                PregnancySigns = record.PregnancySigns,
                AbnormalVaginalDischarge = record.AbnormalVaginalDischarge,
                AbdominalPain = record.AbdominalPain,
                EctopicPregnancyHistory = record.EctopicPregnancyHistory,
                AbnormalUterinebleeding = record.AbnormalUterinebleeding,
                IUDStillInPlace = record.IUDStillInPlace,
                PelvicPain = record.PelvicPain,
                Dysmenorrhea = record.Dysmenorrhea,
                InflammationSigns = record.InflammationSigns,
                TumorOrMalignancy = record.TumorOrMalignancy,
                UterinePosition = record.UterinePosition,
                DiabetesSigns = record.DiabetesSigns,
                BloodClottingDisorder = record.BloodClottingDisorder,
                OrchitisEpididymitis = record.OrchitisEpididymitis,
                TumorOrMalignancyMOP = record.TumorOrMalignancyMOP,
                AllowedContraceptiveMethods = record.AllowedContraceptiveMethods,
                SelectedContraceptiveMethod = record.SelectedContraceptiveMethod,
                ServiceDate = record.ServiceDate,
                FollowUpDate = record.FollowUpDate,
                RemovalDate = record.RemovalDate,
                ObservationNotes = record.ObservationNotes,
                CreatedAt = record.CreatedAt,
                UpdatedAt = record.UpdatedAt
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
            _logger.LogError(ex, $"Error fetching family planning record for appointment {appointmentId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching the family planning record"
            });
        }
    }

    /// <summary>
    /// Create a new family planning record
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(FamilyPlanningResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateFamilyPlanning([FromBody] CreateFamilyPlanningRequest request)
    {
        try
        {
            // Verify appointment exists
            var appointmentExists = await _context.Appointments
                .AnyAsync(a => a.Id == request.AppointmentId);

            if (!appointmentExists)
            {
                return BadRequest(new ErrorResponse
                {
                    ErrorCode = "INVALID_APPOINTMENT",
                    Message = $"Appointment with ID {request.AppointmentId} not found"
                });
            }

            var familyPlanning = new FamilyPlanning
            {
                Id = Guid.NewGuid(),
                AppointmentId = request.AppointmentId,
                SpouseName = request.SpouseName,
                HusbandEducation = request.HusbandEducation,
                WifeEducation = request.WifeEducation,
                HusbandOccupation = request.HusbandOccupation,
                WifeOccupation = request.WifeOccupation,
                FamilyPlanningStage = request.FamilyPlanningStage,
                NumberOfLivingChildren = request.NumberOfLivingChildren,
                YoungestChildYears = request.YoungestChildYears,
                YoungestChildMonths = request.YoungestChildMonths,
                KBParticipantStatus = request.KBParticipantStatus,
                LastContraceptiveMethod = request.LastContraceptiveMethod,
                PregnancySigns = request.PregnancySigns,
                AbnormalVaginalDischarge = request.AbnormalVaginalDischarge,
                AbdominalPain = request.AbdominalPain,
                EctopicPregnancyHistory = request.EctopicPregnancyHistory,
                AbnormalUterinebleeding = request.AbnormalUterinebleeding,
                IUDStillInPlace = request.IUDStillInPlace,
                PelvicPain = request.PelvicPain,
                Dysmenorrhea = request.Dysmenorrhea,
                InflammationSigns = request.InflammationSigns,
                TumorOrMalignancy = request.TumorOrMalignancy,
                UterinePosition = request.UterinePosition,
                DiabetesSigns = request.DiabetesSigns,
                BloodClottingDisorder = request.BloodClottingDisorder,
                OrchitisEpididymitis = request.OrchitisEpididymitis,
                TumorOrMalignancyMOP = request.TumorOrMalignancyMOP,
                AllowedContraceptiveMethods = request.AllowedContraceptiveMethods,
                SelectedContraceptiveMethod = request.SelectedContraceptiveMethod,
                ServiceDate = request.ServiceDate,
                FollowUpDate = request.FollowUpDate,
                RemovalDate = request.RemovalDate,
                ObservationNotes = request.ObservationNotes,
                CreatedAt = DateTime.UtcNow
            };

            _context.FamilyPlannings.Add(familyPlanning);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Family planning record created: {familyPlanning.Id}");

            var response = new FamilyPlanningResponse
            {
                Id = familyPlanning.Id,
                AppointmentId = familyPlanning.AppointmentId,
                SpouseName = familyPlanning.SpouseName,
                HusbandEducation = familyPlanning.HusbandEducation,
                WifeEducation = familyPlanning.WifeEducation,
                HusbandOccupation = familyPlanning.HusbandOccupation,
                WifeOccupation = familyPlanning.WifeOccupation,
                FamilyPlanningStage = familyPlanning.FamilyPlanningStage,
                NumberOfLivingChildren = familyPlanning.NumberOfLivingChildren,
                YoungestChildYears = familyPlanning.YoungestChildYears,
                YoungestChildMonths = familyPlanning.YoungestChildMonths,
                KBParticipantStatus = familyPlanning.KBParticipantStatus,
                LastContraceptiveMethod = familyPlanning.LastContraceptiveMethod,
                PregnancySigns = familyPlanning.PregnancySigns,
                AbnormalVaginalDischarge = familyPlanning.AbnormalVaginalDischarge,
                AbdominalPain = familyPlanning.AbdominalPain,
                EctopicPregnancyHistory = familyPlanning.EctopicPregnancyHistory,
                AbnormalUterinebleeding = familyPlanning.AbnormalUterinebleeding,
                IUDStillInPlace = familyPlanning.IUDStillInPlace,
                PelvicPain = familyPlanning.PelvicPain,
                Dysmenorrhea = familyPlanning.Dysmenorrhea,
                InflammationSigns = familyPlanning.InflammationSigns,
                TumorOrMalignancy = familyPlanning.TumorOrMalignancy,
                UterinePosition = familyPlanning.UterinePosition,
                DiabetesSigns = familyPlanning.DiabetesSigns,
                BloodClottingDisorder = familyPlanning.BloodClottingDisorder,
                OrchitisEpididymitis = familyPlanning.OrchitisEpididymitis,
                TumorOrMalignancyMOP = familyPlanning.TumorOrMalignancyMOP,
                AllowedContraceptiveMethods = familyPlanning.AllowedContraceptiveMethods,
                SelectedContraceptiveMethod = familyPlanning.SelectedContraceptiveMethod,
                ServiceDate = familyPlanning.ServiceDate,
                FollowUpDate = familyPlanning.FollowUpDate,
                RemovalDate = familyPlanning.RemovalDate,
                ObservationNotes = familyPlanning.ObservationNotes,
                CreatedAt = familyPlanning.CreatedAt,
                UpdatedAt = familyPlanning.UpdatedAt
            };

            return CreatedAtAction(nameof(GetFamilyPlanningRecord), new { id = familyPlanning.Id }, response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating family planning record");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while creating the family planning record"
            });
        }
    }

    /// <summary>
    /// Update family planning record
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(FamilyPlanningResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateFamilyPlanning(Guid id, [FromBody] CreateFamilyPlanningRequest request)
    {
        try
        {
            var familyPlanning = await _context.FamilyPlannings.FirstOrDefaultAsync(fp => fp.Id == id);

            if (familyPlanning == null)
            {
                throw new NotFoundException($"Family planning record with ID {id} not found");
            }

            // Update fields
            if (!string.IsNullOrEmpty(request.SpouseName))
                familyPlanning.SpouseName = request.SpouseName;

            if (!string.IsNullOrEmpty(request.HusbandEducation))
                familyPlanning.HusbandEducation = request.HusbandEducation;

            if (!string.IsNullOrEmpty(request.WifeEducation))
                familyPlanning.WifeEducation = request.WifeEducation;

            if (!string.IsNullOrEmpty(request.HusbandOccupation))
                familyPlanning.HusbandOccupation = request.HusbandOccupation;

            if (!string.IsNullOrEmpty(request.WifeOccupation))
                familyPlanning.WifeOccupation = request.WifeOccupation;

            if (!string.IsNullOrEmpty(request.FamilyPlanningStage))
                familyPlanning.FamilyPlanningStage = request.FamilyPlanningStage;

            if (request.NumberOfLivingChildren.HasValue)
                familyPlanning.NumberOfLivingChildren = request.NumberOfLivingChildren;

            if (request.YoungestChildYears.HasValue)
                familyPlanning.YoungestChildYears = request.YoungestChildYears;

            if (request.YoungestChildMonths.HasValue)
                familyPlanning.YoungestChildMonths = request.YoungestChildMonths;

            if (!string.IsNullOrEmpty(request.KBParticipantStatus))
                familyPlanning.KBParticipantStatus = request.KBParticipantStatus;

            if (!string.IsNullOrEmpty(request.LastContraceptiveMethod))
                familyPlanning.LastContraceptiveMethod = request.LastContraceptiveMethod;

            // Pre-insertion examination
            if (request.PregnancySigns.HasValue)
                familyPlanning.PregnancySigns = request.PregnancySigns;

            if (request.AbnormalVaginalDischarge.HasValue)
                familyPlanning.AbnormalVaginalDischarge = request.AbnormalVaginalDischarge;

            if (request.AbdominalPain.HasValue)
                familyPlanning.AbdominalPain = request.AbdominalPain;

            if (request.EctopicPregnancyHistory.HasValue)
                familyPlanning.EctopicPregnancyHistory = request.EctopicPregnancyHistory;

            if (request.AbnormalUterinebleeding.HasValue)
                familyPlanning.AbnormalUterinebleeding = request.AbnormalUterinebleeding;

            if (request.IUDStillInPlace.HasValue)
                familyPlanning.IUDStillInPlace = request.IUDStillInPlace;

            if (request.PelvicPain.HasValue)
                familyPlanning.PelvicPain = request.PelvicPain;

            if (request.Dysmenorrhea.HasValue)
                familyPlanning.Dysmenorrhea = request.Dysmenorrhea;

            // Internal examination findings
            if (request.InflammationSigns.HasValue)
                familyPlanning.InflammationSigns = request.InflammationSigns;

            if (request.TumorOrMalignancy.HasValue)
                familyPlanning.TumorOrMalignancy = request.TumorOrMalignancy;

            if (!string.IsNullOrEmpty(request.UterinePosition))
                familyPlanning.UterinePosition = request.UterinePosition;

            // Additional examination for MOP/MOW
            if (request.DiabetesSigns.HasValue)
                familyPlanning.DiabetesSigns = request.DiabetesSigns;

            if (request.BloodClottingDisorder.HasValue)
                familyPlanning.BloodClottingDisorder = request.BloodClottingDisorder;

            if (request.OrchitisEpididymitis.HasValue)
                familyPlanning.OrchitisEpididymitis = request.OrchitisEpididymitis;

            if (request.TumorOrMalignancyMOP.HasValue)
                familyPlanning.TumorOrMalignancyMOP = request.TumorOrMalignancyMOP;

            // Contraceptive selection and service
            if (!string.IsNullOrEmpty(request.AllowedContraceptiveMethods))
                familyPlanning.AllowedContraceptiveMethods = request.AllowedContraceptiveMethods;

            if (!string.IsNullOrEmpty(request.SelectedContraceptiveMethod))
                familyPlanning.SelectedContraceptiveMethod = request.SelectedContraceptiveMethod;

            if (request.ServiceDate.HasValue)
                familyPlanning.ServiceDate = request.ServiceDate;

            if (request.FollowUpDate.HasValue)
                familyPlanning.FollowUpDate = request.FollowUpDate;

            if (request.RemovalDate.HasValue)
                familyPlanning.RemovalDate = request.RemovalDate;

            if (!string.IsNullOrEmpty(request.ObservationNotes))
                familyPlanning.ObservationNotes = request.ObservationNotes;

            familyPlanning.UpdatedAt = DateTime.UtcNow;

            _context.FamilyPlannings.Update(familyPlanning);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Family planning record updated: {familyPlanning.Id}");

            var response = new FamilyPlanningResponse
            {
                Id = familyPlanning.Id,
                AppointmentId = familyPlanning.AppointmentId,
                SpouseName = familyPlanning.SpouseName,
                HusbandEducation = familyPlanning.HusbandEducation,
                WifeEducation = familyPlanning.WifeEducation,
                HusbandOccupation = familyPlanning.HusbandOccupation,
                WifeOccupation = familyPlanning.WifeOccupation,
                FamilyPlanningStage = familyPlanning.FamilyPlanningStage,
                NumberOfLivingChildren = familyPlanning.NumberOfLivingChildren,
                YoungestChildYears = familyPlanning.YoungestChildYears,
                YoungestChildMonths = familyPlanning.YoungestChildMonths,
                KBParticipantStatus = familyPlanning.KBParticipantStatus,
                LastContraceptiveMethod = familyPlanning.LastContraceptiveMethod,
                PregnancySigns = familyPlanning.PregnancySigns,
                AbnormalVaginalDischarge = familyPlanning.AbnormalVaginalDischarge,
                AbdominalPain = familyPlanning.AbdominalPain,
                EctopicPregnancyHistory = familyPlanning.EctopicPregnancyHistory,
                AbnormalUterinebleeding = familyPlanning.AbnormalUterinebleeding,
                IUDStillInPlace = familyPlanning.IUDStillInPlace,
                PelvicPain = familyPlanning.PelvicPain,
                Dysmenorrhea = familyPlanning.Dysmenorrhea,
                InflammationSigns = familyPlanning.InflammationSigns,
                TumorOrMalignancy = familyPlanning.TumorOrMalignancy,
                UterinePosition = familyPlanning.UterinePosition,
                DiabetesSigns = familyPlanning.DiabetesSigns,
                BloodClottingDisorder = familyPlanning.BloodClottingDisorder,
                OrchitisEpididymitis = familyPlanning.OrchitisEpididymitis,
                TumorOrMalignancyMOP = familyPlanning.TumorOrMalignancyMOP,
                AllowedContraceptiveMethods = familyPlanning.AllowedContraceptiveMethods,
                SelectedContraceptiveMethod = familyPlanning.SelectedContraceptiveMethod,
                ServiceDate = familyPlanning.ServiceDate,
                FollowUpDate = familyPlanning.FollowUpDate,
                RemovalDate = familyPlanning.RemovalDate,
                ObservationNotes = familyPlanning.ObservationNotes,
                CreatedAt = familyPlanning.CreatedAt,
                UpdatedAt = familyPlanning.UpdatedAt
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
            _logger.LogError(ex, $"Error updating family planning record {id}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while updating the family planning record"
            });
        }
    }

    /// <summary>
    /// Delete family planning record
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteFamilyPlanning(Guid id)
    {
        try
        {
            var familyPlanning = await _context.FamilyPlannings.FirstOrDefaultAsync(fp => fp.Id == id);

            if (familyPlanning == null)
            {
                throw new NotFoundException($"Family planning record with ID {id} not found");
            }

            _context.FamilyPlannings.Remove(familyPlanning);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Family planning record deleted: {id}");

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
            _logger.LogError(ex, $"Error deleting family planning record {id}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while deleting the family planning record"
            });
        }
    }
}
