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
                    HusbandEducation = fp.HusbandEducation.HasValue ? (int)fp.HusbandEducation : null,
                    WifeEducation = fp.WifeEducation.HasValue ? (int)fp.WifeEducation : null,
                    HusbandOccupation = fp.HusbandOccupation,
                    WifeOccupation = fp.WifeOccupation,
                    FamilyPlanningStage = fp.FamilyPlanningStage.HasValue ? (int)fp.FamilyPlanningStage : null,
                    NumberOfLivingChildren = fp.NumberOfLivingChildren,
                    YoungestChildYears = fp.YoungestChildYears,
                    YoungestChildMonths = fp.YoungestChildMonths,
                    KBParticipantStatus = fp.KBParticipantStatus.HasValue ? (int)fp.KBParticipantStatus : null,
                    LastContraceptiveMethod = fp.LastContraceptiveMethod.HasValue ? (int)fp.LastContraceptiveMethod : null,
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
                    UterinePosition = fp.UterinePosition.HasValue ? (int)fp.UterinePosition : null,
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
                        MethodType = cm.MethodType.HasValue ? (int)cm.MethodType : null,
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
                .Include(fp => fp.ContraceptiveMethods)
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
                HusbandEducation = record.HusbandEducation.HasValue ? (int)record.HusbandEducation : null,
                WifeEducation = record.WifeEducation.HasValue ? (int)record.WifeEducation : null,
                HusbandOccupation = record.HusbandOccupation,
                WifeOccupation = record.WifeOccupation,
                FamilyPlanningStage = record.FamilyPlanningStage.HasValue ? (int)record.FamilyPlanningStage : null,
                NumberOfLivingChildren = record.NumberOfLivingChildren,
                YoungestChildYears = record.YoungestChildYears,
                YoungestChildMonths = record.YoungestChildMonths,
                KBParticipantStatus = record.KBParticipantStatus.HasValue ? (int)record.KBParticipantStatus : null,
                LastContraceptiveMethod = record.LastContraceptiveMethod.HasValue ? (int)record.LastContraceptiveMethod : null,
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
                UterinePosition = record.UterinePosition.HasValue ? (int)record.UterinePosition : null,
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
                UpdatedAt = record.UpdatedAt,
                ContraceptiveMethods = record.ContraceptiveMethods.Select(cm => new FamilyPlanningContraceptiveMethodDto
                {
                    Id = cm.Id,
                    FamilyPlanningId = cm.FamilyPlanningId,
                    MethodType = cm.MethodType.HasValue ? (int)cm.MethodType : null,
                    ServiceDate = cm.ServiceDate,
                    Quantity = cm.Quantity,
                    Notes = cm.Notes,
                    CreatedAt = cm.CreatedAt,
                    UpdatedAt = cm.UpdatedAt
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
                .Include(fp => fp.ContraceptiveMethods)
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
                HusbandEducation = record.HusbandEducation.HasValue ? (int)record.HusbandEducation : null,
                WifeEducation = record.WifeEducation.HasValue ? (int)record.WifeEducation : null,
                HusbandOccupation = record.HusbandOccupation,
                WifeOccupation = record.WifeOccupation,
                FamilyPlanningStage = record.FamilyPlanningStage.HasValue ? (int)record.FamilyPlanningStage : null,
                NumberOfLivingChildren = record.NumberOfLivingChildren,
                YoungestChildYears = record.YoungestChildYears,
                YoungestChildMonths = record.YoungestChildMonths,
                KBParticipantStatus = record.KBParticipantStatus.HasValue ? (int)record.KBParticipantStatus : null,
                LastContraceptiveMethod = record.LastContraceptiveMethod.HasValue ? (int)record.LastContraceptiveMethod : null,
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
                UterinePosition = record.UterinePosition.HasValue ? (int)record.UterinePosition : null,
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
                UpdatedAt = record.UpdatedAt,
                ContraceptiveMethods = record.ContraceptiveMethods.Select(cm => new FamilyPlanningContraceptiveMethodDto
                {
                    Id = cm.Id,
                    FamilyPlanningId = cm.FamilyPlanningId,
                    MethodType = cm.MethodType.HasValue ? (int)cm.MethodType : null,
                    ServiceDate = cm.ServiceDate,
                    Quantity = cm.Quantity,
                    Notes = cm.Notes,
                    CreatedAt = cm.CreatedAt,
                    UpdatedAt = cm.UpdatedAt
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
                HusbandEducation = request.HusbandEducation.HasValue ? (EducationLevel)request.HusbandEducation : null,
                WifeEducation = request.WifeEducation.HasValue ? (EducationLevel)request.WifeEducation : null,
                HusbandOccupation = request.HusbandOccupation,
                WifeOccupation = request.WifeOccupation,
                FamilyPlanningStage = request.FamilyPlanningStage.HasValue ? (FamilyPlanningStage)request.FamilyPlanningStage : null,
                NumberOfLivingChildren = request.NumberOfLivingChildren,
                YoungestChildYears = request.YoungestChildYears,
                YoungestChildMonths = request.YoungestChildMonths,
                KBParticipantStatus = request.KBParticipantStatus.HasValue ? (KBParticipantStatus)request.KBParticipantStatus : null,
                LastContraceptiveMethod = request.LastContraceptiveMethod.HasValue ? (ContraceptiveMethod)request.LastContraceptiveMethod : null,
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
                UterinePosition = request.UterinePosition.HasValue ? (UterinePosition)request.UterinePosition : null,
                DiabetesSigns = request.DiabetesSigns,
                BloodClottingDisorder = request.BloodClottingDisorder,
                OrchitisEpididymitis = request.OrchitisEpididymitis,
                TumorOrMalignancyMOP = request.TumorOrMalignancyMOP,
                AllowedContraceptiveMethods = request.AllowedContraceptiveMethods,
                SelectedContraceptiveMethod = request.SelectedContraceptiveMethod,
                ServiceDate = request.ServiceDate.HasValue ? (request.ServiceDate.Value.Kind == DateTimeKind.Utc ? request.ServiceDate.Value : request.ServiceDate.Value.ToUniversalTime()) : (DateTime?)null,
                FollowUpDate = request.FollowUpDate.HasValue ? (request.FollowUpDate.Value.Kind == DateTimeKind.Utc ? request.FollowUpDate.Value : request.FollowUpDate.Value.ToUniversalTime()) : (DateTime?)null,
                RemovalDate = request.RemovalDate.HasValue ? (request.RemovalDate.Value.Kind == DateTimeKind.Utc ? request.RemovalDate.Value : request.RemovalDate.Value.ToUniversalTime()) : (DateTime?)null,
                ObservationNotes = request.ObservationNotes,
                CreatedAt = DateTime.UtcNow
            };

            _context.FamilyPlannings.Add(familyPlanning);
            await _context.SaveChangesAsync();

            // Add contraceptive methods if provided
            if (request.ContraceptiveMethods != null && request.ContraceptiveMethods.Count > 0)
            {
                var contraceptiveMethods = request.ContraceptiveMethods.Select(cm => new FamilyPlanningContraceptiveMethod
                {
                    Id = Guid.NewGuid(),
                    FamilyPlanningId = familyPlanning.Id,
                    MethodType = cm.MethodType.HasValue ? (ContraceptiveMethod)cm.MethodType : null,
                    ServiceDate = cm.ServiceDate.HasValue ? (cm.ServiceDate.Value.Kind == DateTimeKind.Utc ? cm.ServiceDate.Value : cm.ServiceDate.Value.ToUniversalTime()) : (DateTime?)null,
                    Quantity = cm.Quantity,
                    Notes = cm.Notes,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }).ToList();

                _context.FamilyPlanningContraceptiveMethods.AddRange(contraceptiveMethods);
                await _context.SaveChangesAsync();
            }

            _logger.LogInformation($"Family planning record created: {familyPlanning.Id}");

            var response = new FamilyPlanningResponse
            {
                Id = familyPlanning.Id,
                AppointmentId = familyPlanning.AppointmentId,
                SpouseName = familyPlanning.SpouseName,
                HusbandEducation = familyPlanning.HusbandEducation.HasValue ? (int)familyPlanning.HusbandEducation : null,
                WifeEducation = familyPlanning.WifeEducation.HasValue ? (int)familyPlanning.WifeEducation : null,
                HusbandOccupation = familyPlanning.HusbandOccupation,
                WifeOccupation = familyPlanning.WifeOccupation,
                FamilyPlanningStage = familyPlanning.FamilyPlanningStage.HasValue ? (int)familyPlanning.FamilyPlanningStage : null,
                NumberOfLivingChildren = familyPlanning.NumberOfLivingChildren,
                YoungestChildYears = familyPlanning.YoungestChildYears,
                YoungestChildMonths = familyPlanning.YoungestChildMonths,
                KBParticipantStatus = familyPlanning.KBParticipantStatus.HasValue ? (int)familyPlanning.KBParticipantStatus : null,
                LastContraceptiveMethod = familyPlanning.LastContraceptiveMethod.HasValue ? (int)familyPlanning.LastContraceptiveMethod : null,
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
                UterinePosition = familyPlanning.UterinePosition.HasValue ? (int)familyPlanning.UterinePosition : null,
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
                UpdatedAt = familyPlanning.UpdatedAt,
                ContraceptiveMethods = request.ContraceptiveMethods?.Select(cm => new FamilyPlanningContraceptiveMethodDto
                {
                    MethodType = cm.MethodType,
                    ServiceDate = cm.ServiceDate,
                    Quantity = cm.Quantity,
                    Notes = cm.Notes
                }).ToList() ?? new()
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

            if (request.HusbandEducation.HasValue)
                familyPlanning.HusbandEducation = (EducationLevel)request.HusbandEducation;

            if (request.WifeEducation.HasValue)
                familyPlanning.WifeEducation = (EducationLevel)request.WifeEducation;

            if (!string.IsNullOrEmpty(request.HusbandOccupation))
                familyPlanning.HusbandOccupation = request.HusbandOccupation;

            if (!string.IsNullOrEmpty(request.WifeOccupation))
                familyPlanning.WifeOccupation = request.WifeOccupation;

            if (request.FamilyPlanningStage.HasValue)
                familyPlanning.FamilyPlanningStage = (FamilyPlanningStage)request.FamilyPlanningStage;

            if (request.NumberOfLivingChildren.HasValue)
                familyPlanning.NumberOfLivingChildren = request.NumberOfLivingChildren;

            if (request.YoungestChildYears.HasValue)
                familyPlanning.YoungestChildYears = request.YoungestChildYears;

            if (request.YoungestChildMonths.HasValue)
                familyPlanning.YoungestChildMonths = request.YoungestChildMonths;

            if (request.KBParticipantStatus.HasValue)
                familyPlanning.KBParticipantStatus = (KBParticipantStatus)request.KBParticipantStatus;

            if (request.LastContraceptiveMethod.HasValue)
                familyPlanning.LastContraceptiveMethod = (ContraceptiveMethod)request.LastContraceptiveMethod;

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

            if (request.UterinePosition.HasValue)
                familyPlanning.UterinePosition = (UterinePosition)request.UterinePosition;

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
                familyPlanning.ServiceDate = request.ServiceDate.Value.Kind == DateTimeKind.Utc ? request.ServiceDate.Value : request.ServiceDate.Value.ToUniversalTime();

            if (request.FollowUpDate.HasValue)
                familyPlanning.FollowUpDate = request.FollowUpDate.Value.Kind == DateTimeKind.Utc ? request.FollowUpDate.Value : request.FollowUpDate.Value.ToUniversalTime();

            if (request.RemovalDate.HasValue)
                familyPlanning.RemovalDate = request.RemovalDate.Value.Kind == DateTimeKind.Utc ? request.RemovalDate.Value : request.RemovalDate.Value.ToUniversalTime();

            if (!string.IsNullOrEmpty(request.ObservationNotes))
                familyPlanning.ObservationNotes = request.ObservationNotes;

            // Handle contraceptive methods update
            if (request.ContraceptiveMethods != null)
            {
                // Remove existing contraceptive methods
                var existingMethods = _context.FamilyPlanningContraceptiveMethods
                    .Where(cm => cm.FamilyPlanningId == familyPlanning.Id);
                _context.FamilyPlanningContraceptiveMethods.RemoveRange(existingMethods);

                // Add new contraceptive methods
                if (request.ContraceptiveMethods.Count > 0)
                {
                    var contraceptiveMethods = request.ContraceptiveMethods.Select(cm => new FamilyPlanningContraceptiveMethod
                    {
                        Id = Guid.NewGuid(),
                        FamilyPlanningId = familyPlanning.Id,
                        MethodType = cm.MethodType.HasValue ? (ContraceptiveMethod)cm.MethodType : null,
                        ServiceDate = cm.ServiceDate.HasValue ? (cm.ServiceDate.Value.Kind == DateTimeKind.Utc ? cm.ServiceDate.Value : cm.ServiceDate.Value.ToUniversalTime()) : (DateTime?)null,
                        Quantity = cm.Quantity,
                        Notes = cm.Notes,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    }).ToList();

                    _context.FamilyPlanningContraceptiveMethods.AddRange(contraceptiveMethods);
                }
            }

            familyPlanning.UpdatedAt = DateTime.UtcNow;

            _context.FamilyPlannings.Update(familyPlanning);
            await _context.SaveChangesAsync();

            // Reload contraceptive methods after save
            var savedContraceptiveMethods = await _context.FamilyPlanningContraceptiveMethods
                .Where(cm => cm.FamilyPlanningId == familyPlanning.Id)
                .ToListAsync();

            _logger.LogInformation($"Family planning record updated: {familyPlanning.Id}");

            var response = new FamilyPlanningResponse
            {
                Id = familyPlanning.Id,
                AppointmentId = familyPlanning.AppointmentId,
                SpouseName = familyPlanning.SpouseName,
                HusbandEducation = familyPlanning.HusbandEducation.HasValue ? (int)familyPlanning.HusbandEducation : null,
                WifeEducation = familyPlanning.WifeEducation.HasValue ? (int)familyPlanning.WifeEducation : null,
                HusbandOccupation = familyPlanning.HusbandOccupation,
                WifeOccupation = familyPlanning.WifeOccupation,
                FamilyPlanningStage = familyPlanning.FamilyPlanningStage.HasValue ? (int)familyPlanning.FamilyPlanningStage : null,
                NumberOfLivingChildren = familyPlanning.NumberOfLivingChildren,
                YoungestChildYears = familyPlanning.YoungestChildYears,
                YoungestChildMonths = familyPlanning.YoungestChildMonths,
                KBParticipantStatus = familyPlanning.KBParticipantStatus.HasValue ? (int)familyPlanning.KBParticipantStatus : null,
                LastContraceptiveMethod = familyPlanning.LastContraceptiveMethod.HasValue ? (int)familyPlanning.LastContraceptiveMethod : null,
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
                UterinePosition = familyPlanning.UterinePosition.HasValue ? (int)familyPlanning.UterinePosition : null,
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
                UpdatedAt = familyPlanning.UpdatedAt,
                ContraceptiveMethods = savedContraceptiveMethods.Select(cm => new FamilyPlanningContraceptiveMethodDto
                {
                    Id = cm.Id,
                    FamilyPlanningId = cm.FamilyPlanningId,
                    MethodType = cm.MethodType.HasValue ? (int)cm.MethodType : null,
                    ServiceDate = cm.ServiceDate,
                    Quantity = cm.Quantity,
                    Notes = cm.Notes,
                    CreatedAt = cm.CreatedAt,
                    UpdatedAt = cm.UpdatedAt
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
