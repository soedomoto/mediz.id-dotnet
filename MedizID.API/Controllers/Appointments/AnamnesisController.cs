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
public class AnamnesisController : ControllerBase
{
    private readonly MedizIDDbContext _context;
    private readonly ILogger<AnamnesisController> _logger;

    public AnamnesisController(MedizIDDbContext context, ILogger<AnamnesisController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Get all anamnesis records with pagination
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<AnamnesisResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAnamnesis(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] Guid? appointmentId = null)
    {
        try
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var query = _context.Anamnesis.AsQueryable();

            if (appointmentId.HasValue)
                query = query.Where(a => a.AppointmentId == appointmentId.Value);

            var total = await query.CountAsync();

            var records = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new AnamnesisResponse
                {
                    Id = a.Id,
                    AppointmentId = a.AppointmentId,
                    // Chief Complaint and Presenting Illness
                    ChiefComplaint = a.ChiefComplaint,
                    AdditionalComplaints = a.AdditionalComplaints,
                    DurationYears = a.DurationYears,
                    DurationMonths = a.DurationMonths,
                    DurationDays = a.DurationDays,
                    PresentingIllness = a.PresentIllnessHistory,
                    // Medical History
                    MedicalHistory = a.PastMedicalHistory,
                    FamilyHistory = a.FamilyHistory,
                    AllergiesHistory = a.DrugAllergies,
                    DrugAllergies = a.DrugAllergies,
                    FoodAllergies = a.FoodAllergies,
                    OtherAllergies = a.OtherAllergies,
                    // Medications and Social History
                    CurrentMedications = a.CurrentMedications,
                    SocialHistory = a.SocialHistory,
                    SmokingStatus = a.SmokingStatus,
                    AlcoholConsumption = a.AlcoholConsumption,
                    InsufficientVegetableFruitIntake = a.InsufficientVegetableFruitIntake,
                    // Vital Signs
                    GeneralAppearance = a.GeneralAppearance,
                    ConsciousnessLevel = a.ConsciousnessLevel,
                    BodyTemperature = a.BodyTemperature.HasValue ? (double)a.BodyTemperature : null,
                    RespiratoryRate = a.RespiratoryRate,
                    PulseRate = a.PulseRate,
                    HeartRhythm = a.HeartRhythm,
                    SystolicBloodPressure = a.SystolicBloodPressure,
                    DiastolicBloodPressure = a.DiastolicBloodPressure,
                    BodyWeight = a.BodyWeight.HasValue ? (double)a.BodyWeight : null,
                    Height = a.Height.HasValue ? (double)a.Height : null,
                    HeightMeasurementMethod = a.HeightMeasurementMethod,
                    WaistCircumference = a.WaistCircumference.HasValue ? (double)a.WaistCircumference : null,
                    BodyMassIndex = a.BodyMassIndex.HasValue ? (double)a.BodyMassIndex : null,
                    BMIClassification = a.BMIClassification,
                    OxygenSaturation = a.OxygenSaturation.HasValue ? (double)a.OxygenSaturation : null,
                    PainScale = a.PainScale,
                    PhysicalActivity = a.PhysicalActivity,
                    // Detailed System Examination
                    HeadExamination = a.HeadExamination,
                    EyeExamination = a.EyeExamination,
                    EarExamination = a.EarExamination,
                    NoseExamination = a.NoseExamination,
                    MouthExamination = a.MouthExamination,
                    ThroatExamination = a.ThroatExamination,
                    LungExamination = a.LungExamination,
                    CardiovascularExamination = a.CardiovascularExamination,
                    ChestAxillaExamination = a.ChestAxillaExamination,
                    AbdominalExamination = a.AbdominalExamination,
                    UpperExtremityExamination = a.UpperExtremityExamination,
                    LowerExtremityExamination = a.LowerExtremityExamination,
                    GenitaliaExamination = a.GenitaliaExamination,
                    SkinExamination = a.SkinExamination,
                    NailExamination = a.NailExamination,
                    NeckExamination = a.NeckExamination,
                    NeurologicalExamination = a.NeurologicalExamination,
                    // Assessment and Triage
                    TriageLevel = a.TriageLevel,
                    // Clinical Documentation
                    BodyAnatomyFindings = a.BodyAnatomyFindings,
                    ClinicalNotes = a.ClinicalNotes,
                    AdditionalFindings = a.AdditionalFindings,
                    ObservationNotes = a.ObservationNotes,
                    AdditionalRemarks = a.AdditionalRemarks,
                    BiopsychosocialAssessment = a.BiopsychosocialAssessment,
                    // Assessment and Plan
                    SubjectiveAssessment = a.SubjectiveAssessment,
                    ObjectiveFindings = a.ObjectiveFindings,
                    Assessment = a.Assessment,
                    PlanOfCare = a.PlanOfCare,
                    NursingCareType = a.NursingCareType,
                    NursingCareDescription = a.NursingCareDescription,
                    NursingInterventions = a.NursingInterventions,
                    PatientEducation = a.PatientEducation,
                    Treatment = a.Treatment,
                    CreatedAt = a.CreatedAt
                })
                .ToListAsync();

            return Ok(new PagedResult<AnamnesisResponse>
            {
                Items = records,
                Total = total,
                Page = page,
                PageSize = pageSize
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching anamnesis records");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching anamnesis records"
            });
        }
    }

    /// <summary>
    /// Get anamnesis record by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(AnamnesisResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAnamnesisRecord(Guid id)
    {
        try
        {
            var record = await _context.Anamnesis
                .Include(a => a.Appointment)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (record == null)
            {
                _logger.LogWarning($"Anamnesis record not found: {id}");
                throw new NotFoundException($"Anamnesis record with ID {id} not found");
            }

            var response = new AnamnesisResponse
            {
                Id = record.Id,
                AppointmentId = record.AppointmentId,
                // Chief Complaint and Presenting Illness
                ChiefComplaint = record.ChiefComplaint,
                AdditionalComplaints = record.AdditionalComplaints,
                DurationYears = record.DurationYears,
                DurationMonths = record.DurationMonths,
                DurationDays = record.DurationDays,
                PresentingIllness = record.PresentIllnessHistory,
                // Medical History
                MedicalHistory = record.PastMedicalHistory,
                FamilyHistory = record.FamilyHistory,
                AllergiesHistory = record.DrugAllergies,
                DrugAllergies = record.DrugAllergies,
                FoodAllergies = record.FoodAllergies,
                OtherAllergies = record.OtherAllergies,
                // Medications and Social History
                CurrentMedications = record.CurrentMedications,
                SocialHistory = record.SocialHistory,
                SmokingStatus = record.SmokingStatus,
                AlcoholConsumption = record.AlcoholConsumption,
                InsufficientVegetableFruitIntake = record.InsufficientVegetableFruitIntake,
                // Vital Signs
                GeneralAppearance = record.GeneralAppearance,
                ConsciousnessLevel = record.ConsciousnessLevel,
                BodyTemperature = record.BodyTemperature.HasValue ? (double)record.BodyTemperature : null,
                RespiratoryRate = record.RespiratoryRate,
                PulseRate = record.PulseRate,
                HeartRhythm = record.HeartRhythm,
                SystolicBloodPressure = record.SystolicBloodPressure,
                DiastolicBloodPressure = record.DiastolicBloodPressure,
                BodyWeight = record.BodyWeight.HasValue ? (double)record.BodyWeight : null,
                Height = record.Height.HasValue ? (double)record.Height : null,
                HeightMeasurementMethod = record.HeightMeasurementMethod,
                WaistCircumference = record.WaistCircumference.HasValue ? (double)record.WaistCircumference : null,
                BodyMassIndex = record.BodyMassIndex.HasValue ? (double)record.BodyMassIndex : null,
                BMIClassification = record.BMIClassification,
                OxygenSaturation = record.OxygenSaturation.HasValue ? (double)record.OxygenSaturation : null,
                PainScale = record.PainScale,
                PhysicalActivity = record.PhysicalActivity,
                // Detailed System Examination
                HeadExamination = record.HeadExamination,
                EyeExamination = record.EyeExamination,
                EarExamination = record.EarExamination,
                NoseExamination = record.NoseExamination,
                MouthExamination = record.MouthExamination,
                ThroatExamination = record.ThroatExamination,
                LungExamination = record.LungExamination,
                CardiovascularExamination = record.CardiovascularExamination,
                ChestAxillaExamination = record.ChestAxillaExamination,
                AbdominalExamination = record.AbdominalExamination,
                UpperExtremityExamination = record.UpperExtremityExamination,
                LowerExtremityExamination = record.LowerExtremityExamination,
                GenitaliaExamination = record.GenitaliaExamination,
                SkinExamination = record.SkinExamination,
                NailExamination = record.NailExamination,
                NeckExamination = record.NeckExamination,
                NeurologicalExamination = record.NeurologicalExamination,
                // Assessment and Triage
                TriageLevel = record.TriageLevel,
                // Clinical Documentation
                BodyAnatomyFindings = record.BodyAnatomyFindings,
                ClinicalNotes = record.ClinicalNotes,
                AdditionalFindings = record.AdditionalFindings,
                ObservationNotes = record.ObservationNotes,
                AdditionalRemarks = record.AdditionalRemarks,
                BiopsychosocialAssessment = record.BiopsychosocialAssessment,
                // Assessment and Plan
                SubjectiveAssessment = record.SubjectiveAssessment,
                ObjectiveFindings = record.ObjectiveFindings,
                Assessment = record.Assessment,
                PlanOfCare = record.PlanOfCare,
                NursingCareType = record.NursingCareType,
                NursingCareDescription = record.NursingCareDescription,
                NursingInterventions = record.NursingInterventions,
                PatientEducation = record.PatientEducation,
                Treatment = record.Treatment,
                CreatedAt = record.CreatedAt
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
            _logger.LogError(ex, $"Error fetching anamnesis record {id}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching the anamnesis record"
            });
        }
    }

    /// <summary>
    /// Get anamnesis record by appointment ID
    /// </summary>
    [HttpGet("appointment/{appointmentId}")]
    [ProducesResponseType(typeof(AnamnesisResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAnamnesisRecordByAppointmentId(Guid appointmentId)
    {
        try
        {
            var record = await _context.Anamnesis
                .Include(a => a.Appointment)
                .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);

            if (record == null)
            {
                _logger.LogWarning($"Anamnesis record not found for appointment: {appointmentId}");
                throw new NotFoundException($"Anamnesis record for appointment {appointmentId} not found");
            }

            var response = new AnamnesisResponse
            {
                Id = record.Id,
                AppointmentId = record.AppointmentId,
                // Chief Complaint and Presenting Illness
                ChiefComplaint = record.ChiefComplaint,
                AdditionalComplaints = record.AdditionalComplaints,
                DurationYears = record.DurationYears,
                DurationMonths = record.DurationMonths,
                DurationDays = record.DurationDays,
                PresentingIllness = record.PresentIllnessHistory,
                // Medical History
                MedicalHistory = record.PastMedicalHistory,
                FamilyHistory = record.FamilyHistory,
                AllergiesHistory = record.DrugAllergies,
                DrugAllergies = record.DrugAllergies,
                FoodAllergies = record.FoodAllergies,
                OtherAllergies = record.OtherAllergies,
                // Medications and Social History
                CurrentMedications = record.CurrentMedications,
                SocialHistory = record.SocialHistory,
                SmokingStatus = record.SmokingStatus,
                AlcoholConsumption = record.AlcoholConsumption,
                InsufficientVegetableFruitIntake = record.InsufficientVegetableFruitIntake,
                // Vital Signs
                GeneralAppearance = record.GeneralAppearance,
                ConsciousnessLevel = record.ConsciousnessLevel,
                BodyTemperature = record.BodyTemperature.HasValue ? (double)record.BodyTemperature : null,
                RespiratoryRate = record.RespiratoryRate,
                PulseRate = record.PulseRate,
                HeartRhythm = record.HeartRhythm,
                SystolicBloodPressure = record.SystolicBloodPressure,
                DiastolicBloodPressure = record.DiastolicBloodPressure,
                BodyWeight = record.BodyWeight.HasValue ? (double)record.BodyWeight : null,
                Height = record.Height.HasValue ? (double)record.Height : null,
                HeightMeasurementMethod = record.HeightMeasurementMethod,
                WaistCircumference = record.WaistCircumference.HasValue ? (double)record.WaistCircumference : null,
                BodyMassIndex = record.BodyMassIndex.HasValue ? (double)record.BodyMassIndex : null,
                BMIClassification = record.BMIClassification,
                OxygenSaturation = record.OxygenSaturation.HasValue ? (double)record.OxygenSaturation : null,
                PainScale = record.PainScale,
                PhysicalActivity = record.PhysicalActivity,
                // Detailed System Examination
                HeadExamination = record.HeadExamination,
                EyeExamination = record.EyeExamination,
                EarExamination = record.EarExamination,
                NoseExamination = record.NoseExamination,
                MouthExamination = record.MouthExamination,
                ThroatExamination = record.ThroatExamination,
                LungExamination = record.LungExamination,
                CardiovascularExamination = record.CardiovascularExamination,
                ChestAxillaExamination = record.ChestAxillaExamination,
                AbdominalExamination = record.AbdominalExamination,
                UpperExtremityExamination = record.UpperExtremityExamination,
                LowerExtremityExamination = record.LowerExtremityExamination,
                GenitaliaExamination = record.GenitaliaExamination,
                SkinExamination = record.SkinExamination,
                NailExamination = record.NailExamination,
                NeckExamination = record.NeckExamination,
                NeurologicalExamination = record.NeurologicalExamination,
                // Assessment and Triage
                TriageLevel = record.TriageLevel,
                // Clinical Documentation
                BodyAnatomyFindings = record.BodyAnatomyFindings,
                ClinicalNotes = record.ClinicalNotes,
                AdditionalFindings = record.AdditionalFindings,
                ObservationNotes = record.ObservationNotes,
                AdditionalRemarks = record.AdditionalRemarks,
                BiopsychosocialAssessment = record.BiopsychosocialAssessment,
                // Assessment and Plan
                SubjectiveAssessment = record.SubjectiveAssessment,
                ObjectiveFindings = record.ObjectiveFindings,
                Assessment = record.Assessment,
                PlanOfCare = record.PlanOfCare,
                NursingCareType = record.NursingCareType,
                NursingCareDescription = record.NursingCareDescription,
                NursingInterventions = record.NursingInterventions,
                PatientEducation = record.PatientEducation,
                Treatment = record.Treatment,
                CreatedAt = record.CreatedAt
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
            _logger.LogError(ex, $"Error fetching anamnesis record for appointment {appointmentId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching the anamnesis record"
            });
        }
    }

    /// <summary>
    /// Create a new anamnesis record
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(AnamnesisResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAnamnesis([FromBody] CreateAnamnesisRequest request)
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

            var anamnesis = new Models.Anamnesis
            {
                Id = Guid.NewGuid(),
                AppointmentId = request.AppointmentId,
                // Chief Complaint and Presenting Illness
                ChiefComplaint = request.ChiefComplaint,
                AdditionalComplaints = request.AdditionalComplaints,
                DurationYears = request.DurationYears,
                DurationMonths = request.DurationMonths,
                DurationDays = request.DurationDays,
                PresentIllnessHistory = request.PresentIllness,
                // Medical History
                PastMedicalHistory = request.MedicalHistory,
                FamilyHistory = request.FamilyHistory,
                // Allergies
                DrugAllergies = request.DrugAllergies ?? request.AllergiesHistory,
                FoodAllergies = request.FoodAllergies,
                OtherAllergies = request.OtherAllergies,
                // Medications and Social History
                CurrentMedications = request.MedicationHistory,
                SocialHistory = request.SocialHistory,
                SmokingStatus = request.SmokingStatus,
                AlcoholConsumption = request.AlcoholConsumption,
                InsufficientVegetableFruitIntake = request.InsufficientVegetableFruitIntake,
                // Physical Examination - Vital Signs
                GeneralAppearance = request.GeneralAppearance,
                ConsciousnessLevel = request.ConsciousnessLevel,
                BodyTemperature = request.BodyTemperature.HasValue ? (decimal)request.BodyTemperature.Value : null,
                RespiratoryRate = request.RespiratoryRate,
                PulseRate = request.PulseRate,
                HeartRhythm = request.HeartRhythm,
                SystolicBloodPressure = request.SystolicBloodPressure,
                DiastolicBloodPressure = request.DiastolicBloodPressure,
                BodyWeight = request.BodyWeight.HasValue ? (decimal)request.BodyWeight.Value : null,
                Height = request.Height.HasValue ? (decimal)request.Height.Value : null,
                HeightMeasurementMethod = request.HeightMeasurementMethod,
                WaistCircumference = request.WaistCircumference.HasValue ? (decimal)request.WaistCircumference.Value : null,
                BodyMassIndex = request.BodyMassIndex.HasValue ? (decimal)request.BodyMassIndex.Value : null,
                BMIClassification = request.BMIClassification,
                OxygenSaturation = request.OxygenSaturation.HasValue ? (decimal)request.OxygenSaturation.Value : null,
                PainScale = request.PainScale,
                PhysicalActivity = request.PhysicalActivity,
                // Detailed System Examination
                HeadExamination = request.HeadExamination,
                EyeExamination = request.EyeExamination,
                EarExamination = request.EarExamination,
                NoseExamination = request.NoseExamination,
                MouthExamination = request.MouthExamination,
                ThroatExamination = request.ThroatExamination,
                LungExamination = request.LungExamination,
                CardiovascularExamination = request.CardiovascularExamination,
                ChestAxillaExamination = request.ChestAxillaExamination,
                AbdominalExamination = request.AbdominalExamination,
                UpperExtremityExamination = request.UpperExtremityExamination,
                LowerExtremityExamination = request.LowerExtremityExamination,
                GenitaliaExamination = request.GenitaliaExamination,
                SkinExamination = request.SkinExamination,
                NailExamination = request.NailExamination,
                NeckExamination = request.NeckExamination,
                NeurologicalExamination = request.NeurologicalExamination,
                // Assessment and Triage
                TriageLevel = request.TriageLevel,
                // Clinical Documentation
                BodyAnatomyFindings = request.BodyAnatomyFindings,
                ClinicalNotes = request.ClinicalNotes,
                AdditionalFindings = request.AdditionalFindings,
                ObservationNotes = request.ObservationNotes,
                AdditionalRemarks = request.AdditionalRemarks,
                BiopsychosocialAssessment = request.BiopsychosocialAssessment,
                // Assessment and Plan
                SubjectiveAssessment = request.SubjectiveAssessment,
                ObjectiveFindings = request.ObjectiveFindings,
                Assessment = request.Assessment,
                PlanOfCare = request.PlanOfCare,
                NursingCareType = request.NursingCareType,
                NursingCareDescription = request.NursingCareDescription,
                NursingInterventions = request.NursingInterventions,
                PatientEducation = request.PatientEducation,
                Treatment = request.Treatment,
                CreatedAt = DateTime.UtcNow
            };

            _context.Anamnesis.Add(anamnesis);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Anamnesis record created: {anamnesis.Id}");

            var response = new AnamnesisResponse
            {
                Id = anamnesis.Id,
                AppointmentId = anamnesis.AppointmentId,
                // Chief Complaint and Presenting Illness
                ChiefComplaint = anamnesis.ChiefComplaint,
                AdditionalComplaints = anamnesis.AdditionalComplaints,
                DurationYears = anamnesis.DurationYears,
                DurationMonths = anamnesis.DurationMonths,
                DurationDays = anamnesis.DurationDays,
                PresentingIllness = anamnesis.PresentIllnessHistory,
                // Medical History
                MedicalHistory = anamnesis.PastMedicalHistory,
                FamilyHistory = anamnesis.FamilyHistory,
                AllergiesHistory = anamnesis.DrugAllergies,
                DrugAllergies = anamnesis.DrugAllergies,
                FoodAllergies = anamnesis.FoodAllergies,
                OtherAllergies = anamnesis.OtherAllergies,
                // Medications and Social History
                CurrentMedications = anamnesis.CurrentMedications,
                SocialHistory = anamnesis.SocialHistory,
                SmokingStatus = anamnesis.SmokingStatus,
                AlcoholConsumption = anamnesis.AlcoholConsumption,
                InsufficientVegetableFruitIntake = anamnesis.InsufficientVegetableFruitIntake,
                // Vital Signs
                GeneralAppearance = anamnesis.GeneralAppearance,
                ConsciousnessLevel = anamnesis.ConsciousnessLevel,
                BodyTemperature = anamnesis.BodyTemperature.HasValue ? (double)anamnesis.BodyTemperature : null,
                RespiratoryRate = anamnesis.RespiratoryRate,
                PulseRate = anamnesis.PulseRate,
                HeartRhythm = anamnesis.HeartRhythm,
                SystolicBloodPressure = anamnesis.SystolicBloodPressure,
                DiastolicBloodPressure = anamnesis.DiastolicBloodPressure,
                BodyWeight = anamnesis.BodyWeight.HasValue ? (double)anamnesis.BodyWeight : null,
                Height = anamnesis.Height.HasValue ? (double)anamnesis.Height : null,
                HeightMeasurementMethod = anamnesis.HeightMeasurementMethod,
                WaistCircumference = anamnesis.WaistCircumference.HasValue ? (double)anamnesis.WaistCircumference : null,
                BodyMassIndex = anamnesis.BodyMassIndex.HasValue ? (double)anamnesis.BodyMassIndex : null,
                BMIClassification = anamnesis.BMIClassification,
                OxygenSaturation = anamnesis.OxygenSaturation.HasValue ? (double)anamnesis.OxygenSaturation : null,
                PainScale = anamnesis.PainScale,
                PhysicalActivity = anamnesis.PhysicalActivity,
                // Detailed System Examination
                HeadExamination = anamnesis.HeadExamination,
                EyeExamination = anamnesis.EyeExamination,
                EarExamination = anamnesis.EarExamination,
                NoseExamination = anamnesis.NoseExamination,
                MouthExamination = anamnesis.MouthExamination,
                ThroatExamination = anamnesis.ThroatExamination,
                LungExamination = anamnesis.LungExamination,
                CardiovascularExamination = anamnesis.CardiovascularExamination,
                ChestAxillaExamination = anamnesis.ChestAxillaExamination,
                AbdominalExamination = anamnesis.AbdominalExamination,
                UpperExtremityExamination = anamnesis.UpperExtremityExamination,
                LowerExtremityExamination = anamnesis.LowerExtremityExamination,
                GenitaliaExamination = anamnesis.GenitaliaExamination,
                SkinExamination = anamnesis.SkinExamination,
                NailExamination = anamnesis.NailExamination,
                NeckExamination = anamnesis.NeckExamination,
                NeurologicalExamination = anamnesis.NeurologicalExamination,
                // Assessment and Triage
                TriageLevel = anamnesis.TriageLevel,
                // Clinical Documentation
                BodyAnatomyFindings = anamnesis.BodyAnatomyFindings,
                ClinicalNotes = anamnesis.ClinicalNotes,
                AdditionalFindings = anamnesis.AdditionalFindings,
                ObservationNotes = anamnesis.ObservationNotes,
                AdditionalRemarks = anamnesis.AdditionalRemarks,
                BiopsychosocialAssessment = anamnesis.BiopsychosocialAssessment,
                // Assessment and Plan
                SubjectiveAssessment = anamnesis.SubjectiveAssessment,
                ObjectiveFindings = anamnesis.ObjectiveFindings,
                Assessment = anamnesis.Assessment,
                PlanOfCare = anamnesis.PlanOfCare,
                NursingCareType = anamnesis.NursingCareType,
                NursingCareDescription = anamnesis.NursingCareDescription,
                NursingInterventions = anamnesis.NursingInterventions,
                PatientEducation = anamnesis.PatientEducation,
                Treatment = anamnesis.Treatment,
                CreatedAt = anamnesis.CreatedAt
            };

            return CreatedAtAction(nameof(GetAnamnesisRecord), new { id = anamnesis.Id }, response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating anamnesis record");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while creating the anamnesis record"
            });
        }
    }

    /// <summary>
    /// Update anamnesis record
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(AnamnesisResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAnamnesis(Guid id, [FromBody] CreateAnamnesisRequest request)
    {
        try
        {
            var anamnesis = await _context.Anamnesis.FirstOrDefaultAsync(a => a.Id == id);

            if (anamnesis == null)
            {
                throw new NotFoundException($"Anamnesis record with ID {id} not found");
            }

            if (!string.IsNullOrEmpty(request.ChiefComplaint))
                anamnesis.ChiefComplaint = request.ChiefComplaint;

            if (!string.IsNullOrEmpty(request.AdditionalComplaints))
                anamnesis.AdditionalComplaints = request.AdditionalComplaints;

            if (request.DurationYears.HasValue)
                anamnesis.DurationYears = request.DurationYears;

            if (request.DurationMonths.HasValue)
                anamnesis.DurationMonths = request.DurationMonths;

            if (request.DurationDays.HasValue)
                anamnesis.DurationDays = request.DurationDays;

            if (!string.IsNullOrEmpty(request.PresentIllness))
                anamnesis.PresentIllnessHistory = request.PresentIllness;

            if (!string.IsNullOrEmpty(request.MedicalHistory))
                anamnesis.PastMedicalHistory = request.MedicalHistory;

            if (!string.IsNullOrEmpty(request.FamilyHistory))
                anamnesis.FamilyHistory = request.FamilyHistory;

            if (!string.IsNullOrEmpty(request.AllergiesHistory))
                anamnesis.DrugAllergies = request.AllergiesHistory;

            if (!string.IsNullOrEmpty(request.DrugAllergies))
                anamnesis.DrugAllergies = request.DrugAllergies;

            if (!string.IsNullOrEmpty(request.FoodAllergies))
                anamnesis.FoodAllergies = request.FoodAllergies;

            if (!string.IsNullOrEmpty(request.OtherAllergies))
                anamnesis.OtherAllergies = request.OtherAllergies;

            if (!string.IsNullOrEmpty(request.MedicationHistory))
                anamnesis.CurrentMedications = request.MedicationHistory;

            if (!string.IsNullOrEmpty(request.SocialHistory))
                anamnesis.SocialHistory = request.SocialHistory;

            if (request.SmokingStatus.HasValue)
                anamnesis.SmokingStatus = request.SmokingStatus;

            if (request.AlcoholConsumption.HasValue)
                anamnesis.AlcoholConsumption = request.AlcoholConsumption;

            if (request.InsufficientVegetableFruitIntake.HasValue)
                anamnesis.InsufficientVegetableFruitIntake = request.InsufficientVegetableFruitIntake;

            if (!string.IsNullOrEmpty(request.GeneralAppearance))
                anamnesis.GeneralAppearance = request.GeneralAppearance;

            if (request.ConsciousnessLevel.HasValue)
                anamnesis.ConsciousnessLevel = request.ConsciousnessLevel;

            if (request.BodyTemperature.HasValue)
                anamnesis.BodyTemperature = (decimal?)request.BodyTemperature;

            if (request.RespiratoryRate.HasValue)
                anamnesis.RespiratoryRate = request.RespiratoryRate;

            if (request.PulseRate.HasValue)
                anamnesis.PulseRate = request.PulseRate;

            if (request.HeartRhythm.HasValue)
                anamnesis.HeartRhythm = request.HeartRhythm;

            if (request.SystolicBloodPressure.HasValue)
                anamnesis.SystolicBloodPressure = request.SystolicBloodPressure;

            if (request.DiastolicBloodPressure.HasValue)
                anamnesis.DiastolicBloodPressure = request.DiastolicBloodPressure;

            if (request.BodyWeight.HasValue)
                anamnesis.BodyWeight = (decimal?)request.BodyWeight;

            if (request.Height.HasValue)
                anamnesis.Height = (decimal?)request.Height;

            if (request.HeightMeasurementMethod.HasValue)
                anamnesis.HeightMeasurementMethod = request.HeightMeasurementMethod;

            if (request.WaistCircumference.HasValue)
                anamnesis.WaistCircumference = (decimal?)request.WaistCircumference;

            if (request.BodyMassIndex.HasValue)
                anamnesis.BodyMassIndex = (decimal?)request.BodyMassIndex;

            if (!string.IsNullOrEmpty(request.BMIClassification))
                anamnesis.BMIClassification = request.BMIClassification;

            if (request.OxygenSaturation.HasValue)
                anamnesis.OxygenSaturation = (decimal?)request.OxygenSaturation;

            if (request.PainScale.HasValue)
                anamnesis.PainScale = request.PainScale;

            if (!string.IsNullOrEmpty(request.PhysicalActivity))
                anamnesis.PhysicalActivity = request.PhysicalActivity;

            if (!string.IsNullOrEmpty(request.HeadExamination))
                anamnesis.HeadExamination = request.HeadExamination;

            if (!string.IsNullOrEmpty(request.EyeExamination))
                anamnesis.EyeExamination = request.EyeExamination;

            if (!string.IsNullOrEmpty(request.EarExamination))
                anamnesis.EarExamination = request.EarExamination;

            if (!string.IsNullOrEmpty(request.NoseExamination))
                anamnesis.NoseExamination = request.NoseExamination;

            if (!string.IsNullOrEmpty(request.MouthExamination))
                anamnesis.MouthExamination = request.MouthExamination;

            if (!string.IsNullOrEmpty(request.ThroatExamination))
                anamnesis.ThroatExamination = request.ThroatExamination;

            if (!string.IsNullOrEmpty(request.LungExamination))
                anamnesis.LungExamination = request.LungExamination;

            if (!string.IsNullOrEmpty(request.CardiovascularExamination))
                anamnesis.CardiovascularExamination = request.CardiovascularExamination;

            if (!string.IsNullOrEmpty(request.ChestAxillaExamination))
                anamnesis.ChestAxillaExamination = request.ChestAxillaExamination;

            if (!string.IsNullOrEmpty(request.AbdominalExamination))
                anamnesis.AbdominalExamination = request.AbdominalExamination;

            if (!string.IsNullOrEmpty(request.UpperExtremityExamination))
                anamnesis.UpperExtremityExamination = request.UpperExtremityExamination;

            if (!string.IsNullOrEmpty(request.LowerExtremityExamination))
                anamnesis.LowerExtremityExamination = request.LowerExtremityExamination;

            if (!string.IsNullOrEmpty(request.GenitaliaExamination))
                anamnesis.GenitaliaExamination = request.GenitaliaExamination;

            if (!string.IsNullOrEmpty(request.SkinExamination))
                anamnesis.SkinExamination = request.SkinExamination;

            if (!string.IsNullOrEmpty(request.NailExamination))
                anamnesis.NailExamination = request.NailExamination;

            if (!string.IsNullOrEmpty(request.NeckExamination))
                anamnesis.NeckExamination = request.NeckExamination;

            if (!string.IsNullOrEmpty(request.NeurologicalExamination))
                anamnesis.NeurologicalExamination = request.NeurologicalExamination;

            if (request.TriageLevel.HasValue)
                anamnesis.TriageLevel = request.TriageLevel;

            if (!string.IsNullOrEmpty(request.BodyAnatomyFindings))
                anamnesis.BodyAnatomyFindings = request.BodyAnatomyFindings;

            if (!string.IsNullOrEmpty(request.ClinicalNotes))
                anamnesis.ClinicalNotes = request.ClinicalNotes;

            if (!string.IsNullOrEmpty(request.AdditionalFindings))
                anamnesis.AdditionalFindings = request.AdditionalFindings;

            if (!string.IsNullOrEmpty(request.ObservationNotes))
                anamnesis.ObservationNotes = request.ObservationNotes;

            if (!string.IsNullOrEmpty(request.AdditionalRemarks))
                anamnesis.AdditionalRemarks = request.AdditionalRemarks;

            if (!string.IsNullOrEmpty(request.BiopsychosocialAssessment))
                anamnesis.BiopsychosocialAssessment = request.BiopsychosocialAssessment;

            if (!string.IsNullOrEmpty(request.SubjectiveAssessment))
                anamnesis.SubjectiveAssessment = request.SubjectiveAssessment;

            if (!string.IsNullOrEmpty(request.ObjectiveFindings))
                anamnesis.ObjectiveFindings = request.ObjectiveFindings;

            if (!string.IsNullOrEmpty(request.Assessment))
                anamnesis.Assessment = request.Assessment;

            if (!string.IsNullOrEmpty(request.PlanOfCare))
                anamnesis.PlanOfCare = request.PlanOfCare;

            if (request.NursingCareType.HasValue)
                anamnesis.NursingCareType = request.NursingCareType;

            if (!string.IsNullOrEmpty(request.NursingCareDescription))
                anamnesis.NursingCareDescription = request.NursingCareDescription;

            if (!string.IsNullOrEmpty(request.NursingInterventions))
                anamnesis.NursingInterventions = request.NursingInterventions;

            if (!string.IsNullOrEmpty(request.PatientEducation))
                anamnesis.PatientEducation = request.PatientEducation;

            if (!string.IsNullOrEmpty(request.Treatment))
                anamnesis.Treatment = request.Treatment;

            anamnesis.UpdatedAt = DateTime.UtcNow;
            _context.Anamnesis.Update(anamnesis);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Anamnesis record updated: {anamnesis.Id}");

            var response = new AnamnesisResponse
            {
                Id = anamnesis.Id,
                AppointmentId = anamnesis.AppointmentId,
                // Chief Complaint and Presenting Illness
                ChiefComplaint = anamnesis.ChiefComplaint,
                AdditionalComplaints = anamnesis.AdditionalComplaints,
                DurationYears = anamnesis.DurationYears,
                DurationMonths = anamnesis.DurationMonths,
                DurationDays = anamnesis.DurationDays,
                PresentingIllness = anamnesis.PresentIllnessHistory,
                // Medical History
                MedicalHistory = anamnesis.PastMedicalHistory,
                FamilyHistory = anamnesis.FamilyHistory,
                AllergiesHistory = anamnesis.DrugAllergies,
                DrugAllergies = anamnesis.DrugAllergies,
                FoodAllergies = anamnesis.FoodAllergies,
                OtherAllergies = anamnesis.OtherAllergies,
                // Medications and Social History
                CurrentMedications = anamnesis.CurrentMedications,
                SocialHistory = anamnesis.SocialHistory,
                SmokingStatus = anamnesis.SmokingStatus,
                AlcoholConsumption = anamnesis.AlcoholConsumption,
                InsufficientVegetableFruitIntake = anamnesis.InsufficientVegetableFruitIntake,
                // Vital Signs
                GeneralAppearance = anamnesis.GeneralAppearance,
                ConsciousnessLevel = anamnesis.ConsciousnessLevel,
                BodyTemperature = anamnesis.BodyTemperature.HasValue ? (double)anamnesis.BodyTemperature : null,
                RespiratoryRate = anamnesis.RespiratoryRate,
                PulseRate = anamnesis.PulseRate,
                HeartRhythm = anamnesis.HeartRhythm,
                SystolicBloodPressure = anamnesis.SystolicBloodPressure,
                DiastolicBloodPressure = anamnesis.DiastolicBloodPressure,
                BodyWeight = anamnesis.BodyWeight.HasValue ? (double)anamnesis.BodyWeight : null,
                Height = anamnesis.Height.HasValue ? (double)anamnesis.Height : null,
                HeightMeasurementMethod = anamnesis.HeightMeasurementMethod,
                WaistCircumference = anamnesis.WaistCircumference.HasValue ? (double)anamnesis.WaistCircumference : null,
                BodyMassIndex = anamnesis.BodyMassIndex.HasValue ? (double)anamnesis.BodyMassIndex : null,
                BMIClassification = anamnesis.BMIClassification,
                OxygenSaturation = anamnesis.OxygenSaturation.HasValue ? (double)anamnesis.OxygenSaturation : null,
                PainScale = anamnesis.PainScale,
                PhysicalActivity = anamnesis.PhysicalActivity,
                // Detailed System Examination
                HeadExamination = anamnesis.HeadExamination,
                EyeExamination = anamnesis.EyeExamination,
                EarExamination = anamnesis.EarExamination,
                NoseExamination = anamnesis.NoseExamination,
                MouthExamination = anamnesis.MouthExamination,
                ThroatExamination = anamnesis.ThroatExamination,
                LungExamination = anamnesis.LungExamination,
                CardiovascularExamination = anamnesis.CardiovascularExamination,
                ChestAxillaExamination = anamnesis.ChestAxillaExamination,
                AbdominalExamination = anamnesis.AbdominalExamination,
                UpperExtremityExamination = anamnesis.UpperExtremityExamination,
                LowerExtremityExamination = anamnesis.LowerExtremityExamination,
                GenitaliaExamination = anamnesis.GenitaliaExamination,
                SkinExamination = anamnesis.SkinExamination,
                NailExamination = anamnesis.NailExamination,
                NeckExamination = anamnesis.NeckExamination,
                NeurologicalExamination = anamnesis.NeurologicalExamination,
                // Assessment and Triage
                TriageLevel = anamnesis.TriageLevel,
                // Clinical Documentation
                BodyAnatomyFindings = anamnesis.BodyAnatomyFindings,
                ClinicalNotes = anamnesis.ClinicalNotes,
                AdditionalFindings = anamnesis.AdditionalFindings,
                ObservationNotes = anamnesis.ObservationNotes,
                AdditionalRemarks = anamnesis.AdditionalRemarks,
                BiopsychosocialAssessment = anamnesis.BiopsychosocialAssessment,
                // Assessment and Plan
                SubjectiveAssessment = anamnesis.SubjectiveAssessment,
                ObjectiveFindings = anamnesis.ObjectiveFindings,
                Assessment = anamnesis.Assessment,
                PlanOfCare = anamnesis.PlanOfCare,
                NursingCareType = anamnesis.NursingCareType,
                NursingCareDescription = anamnesis.NursingCareDescription,
                NursingInterventions = anamnesis.NursingInterventions,
                PatientEducation = anamnesis.PatientEducation,
                Treatment = anamnesis.Treatment,
                CreatedAt = anamnesis.CreatedAt
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
            _logger.LogError(ex, $"Error updating anamnesis record {id}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while updating the anamnesis record"
            });
        }
    }

    /// <summary>
    /// Delete anamnesis record
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAnamnesis(Guid id)
    {
        try
        {
            var anamnesis = await _context.Anamnesis.FirstOrDefaultAsync(a => a.Id == id);

            if (anamnesis == null)
            {
                throw new NotFoundException($"Anamnesis record with ID {id} not found");
            }

            _context.Anamnesis.Remove(anamnesis);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Anamnesis record deleted: {id}");

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
            _logger.LogError(ex, $"Error deleting anamnesis record {id}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while deleting the anamnesis record"
            });
        }
    }
}
