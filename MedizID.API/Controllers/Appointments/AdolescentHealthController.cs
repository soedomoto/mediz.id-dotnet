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
public class AdolescentHealthController : ControllerBase
{
    private readonly MedizIDDbContext _context;
    private readonly ILogger<AdolescentHealthController> _logger;

    public AdolescentHealthController(MedizIDDbContext context, ILogger<AdolescentHealthController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Create adolescent health record for an appointment
    /// </summary>
    [HttpPost("appointments/{appointmentId}")]
    [ProducesResponseType(typeof(AdolescentHealthResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAdolescentHealthRecord(Guid appointmentId, [FromBody] CreateAdolescentHealthRequest request)
    {
        try
        {
            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.Id == appointmentId);

            if (appointment == null)
            {
                throw new NotFoundException($"Appointment with ID {appointmentId} not found");
            }

            var adolescentHealth = new AdolescentHealth
            {
                Id = Guid.NewGuid(),
                AppointmentId = appointmentId,
                Citizenship = request.Citizenship,
                Residence = request.Residence,
                SchoolName = request.SchoolName,
                Grade = request.Grade,
                ChildOrder = request.ChildOrder,
                TotalSiblings = request.TotalSiblings,
                Occupation = request.Occupation,
                FatherEducation = request.FatherEducation,
                FatherOccupation = request.FatherOccupation,
                MotherEducation = request.MotherEducation,
                MotherOccupation = request.MotherOccupation,
                MaritalStatus = request.MaritalStatus,
                ParentStatus = request.ParentStatus,
                Weight = request.Weight,
                Height = request.Height,
                MainComplaint = request.MainComplaint,
                MedicalHistory = request.MedicalHistory,
                Diagnosis = request.Diagnosis,
                MenstrualDisorder = request.MenstrualDisorder,
                MenstrualDisorderNotes = request.MenstrualDisorderNotes,
                PremaritalSex = request.PremaritalSex,
                PremaritalSexNotes = request.PremaritalSexNotes,
                Pregnancy = request.Pregnancy,
                PregnancyNotes = request.PregnancyNotes,
                DesiredPregnancy = request.DesiredPregnancy,
                DesiredPregnancyNotes = request.DesiredPregnancyNotes,
                UnwantedPregnancy = request.UnwantedPregnancy,
                UnwantedPregnancyNotes = request.UnwantedPregnancyNotes,
                TeenageDelivery = request.TeenageDelivery,
                TeenageDeliveryNotes = request.TeenageDeliveryNotes,
                Abortion = request.Abortion,
                AbortionNotes = request.AbortionNotes,
                NutritionDisorder = request.NutritionDisorder,
                NutritionDisorderNotes = request.NutritionDisorderNotes,
                Anemia = request.Anemia,
                AnemiaNotes = request.AnemiaNotes,
                ChronicEnergyDeficiency = request.ChronicEnergyDeficiency,
                ChronicEnergyDeficiencyNotes = request.ChronicEnergyDeficiencyNotes,
                Obesity = request.Obesity,
                ObesityNotes = request.ObesityNotes,
                DrugAbuse = request.DrugAbuse,
                DrugAbuseNotes = request.DrugAbuseNotes,
                Smoking = request.Smoking,
                SmokingNotes = request.SmokingNotes,
                AlcoholUse = request.AlcoholUse,
                AlcoholUseNotes = request.AlcoholUseNotes,
                OtherSubstanceUse = request.OtherSubstanceUse,
                OtherSubstanceUseNotes = request.OtherSubstanceUseNotes,
                SexuallyTransmittedInfection = request.SexuallyTransmittedInfection,
                SexuallyTransmittedInfectionNotes = request.SexuallyTransmittedInfectionNotes,
                ReproductiveInfection = request.ReproductiveInfection,
                ReproductiveInfectionNotes = request.ReproductiveInfectionNotes,
                HIV = request.HIV,
                HIVNotes = request.HIVNotes,
                AIDS = request.AIDS,
                AIDSNotes = request.AIDSNotes,
                PsychologicalIssues = request.PsychologicalIssues,
                PsychologicalIssuesNotes = request.PsychologicalIssuesNotes,
                GadgetAddiction = request.GadgetAddiction,
                GadgetAddictionNotes = request.GadgetAddictionNotes,
                SexualOrientation = request.SexualOrientation,
                SexualOrientationNotes = request.SexualOrientationNotes,
                MentalDisability = request.MentalDisability,
                MentalDisabilityNotes = request.MentalDisabilityNotes,
                EarlyMarriage = request.EarlyMarriage,
                EarlyMarriageNotes = request.EarlyMarriageNotes,
                ChildAbuse = request.ChildAbuse,
                ChildAbuseNotes = request.ChildAbuseNotes,
                PhysicalDisability = request.PhysicalDisability,
                PhysicalDisabilityNotes = request.PhysicalDisabilityNotes,
                LearningDifficulty = request.LearningDifficulty,
                LearningDifficultyNotes = request.LearningDifficultyNotes,
                OtherProblems = request.OtherProblems,
                OtherProblemsNotes = request.OtherProblemsNotes,
                MainProblem = request.MainProblem,
                ProblemBackground = request.ProblemBackground,
                SolutionAlternatives = request.SolutionAlternatives,
                ClientDecision = request.ClientDecision,
                Observations = request.Observations,
                Counselor = request.Counselor,
                HomeAssessment = request.HomeAssessment,
                EmploymentEducationAssessment = request.EmploymentEducationAssessment,
                EatingAssessment = request.EatingAssessment,
                ActivityAssessment = request.ActivityAssessment,
                DrugsAssessment = request.DrugsAssessment,
                SexualityAssessment = request.SexualityAssessment,
                SafetyAssessment = request.SafetyAssessment,
                SuicideDepressionAssessment = request.SuicideDepressionAssessment,
                CreatedAt = DateTime.UtcNow
            };

            _context.AdolescentHealths.Add(adolescentHealth);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Adolescent health record created: {adolescentHealth.Id}");

            var response = MapToResponse(adolescentHealth);
            return CreatedAtAction(nameof(GetAdolescentHealthRecord), new { recordId = adolescentHealth.Id }, response);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ErrorResponse
            {
                ErrorCode = ex.ErrorCode,
                Message = ex.Message
            });
        }
        catch (ApiException ex)
        {
            return BadRequest(new ErrorResponse
            {
                ErrorCode = ex.ErrorCode,
                Message = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating adolescent health record");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while creating adolescent health record"
            });
        }
    }

    /// <summary>
    /// Get adolescent health record by appointment
    /// </summary>
    [HttpGet("appointments/{appointmentId}")]
    [ProducesResponseType(typeof(AdolescentHealthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAdolescentHealthRecordByAppointment(Guid appointmentId)
    {
        try
        {
            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.Id == appointmentId);

            if (appointment == null)
            {
                throw new NotFoundException($"Appointment with ID {appointmentId} not found");
            }

            var adolescentHealth = await _context.AdolescentHealths
                .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);

            if (adolescentHealth == null)
            {
                throw new NotFoundException("No adolescent health record found for this appointment");
            }

            var response = MapToResponse(adolescentHealth);
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
            _logger.LogError(ex, $"Error fetching adolescent health record for appointment {appointmentId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching adolescent health record"
            });
        }
    }

    /// <summary>
    /// Get adolescent health record by ID
    /// </summary>
    [HttpGet("{recordId}")]
    [ProducesResponseType(typeof(AdolescentHealthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAdolescentHealthRecord(Guid recordId)
    {
        try
        {
            var adolescentHealth = await _context.AdolescentHealths.FirstOrDefaultAsync(a => a.Id == recordId);

            if (adolescentHealth == null)
            {
                throw new NotFoundException($"Adolescent health record with ID {recordId} not found");
            }

            var response = MapToResponse(adolescentHealth);
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
            _logger.LogError(ex, $"Error fetching adolescent health record {recordId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching adolescent health record"
            });
        }
    }

    /// <summary>
    /// Update adolescent health record for an appointment
    /// </summary>
    [HttpPut("appointments/{appointmentId}")]
    [ProducesResponseType(typeof(AdolescentHealthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAdolescentHealthRecordByAppointment(Guid appointmentId, [FromBody] UpdateAdolescentHealthRequest request)
    {
        try
        {
            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.Id == appointmentId);

            if (appointment == null)
            {
                throw new NotFoundException($"Appointment with ID {appointmentId} not found");
            }

            var adolescentHealth = await _context.AdolescentHealths
                .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);

            if (adolescentHealth == null)
            {
                throw new NotFoundException("No adolescent health record found for this appointment");
            }

            UpdateRecord(adolescentHealth, request);
            adolescentHealth.UpdatedAt = DateTime.UtcNow;

            _context.AdolescentHealths.Update(adolescentHealth);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Adolescent health record updated: {adolescentHealth.Id}");

            var response = MapToResponse(adolescentHealth);
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
            _logger.LogError(ex, $"Error updating adolescent health record for appointment {appointmentId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while updating adolescent health record"
            });
        }
    }

    /// <summary>
    /// Update adolescent health record by ID
    /// </summary>
    [HttpPut("{recordId}")]
    [ProducesResponseType(typeof(AdolescentHealthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAdolescentHealthRecord(Guid recordId, [FromBody] UpdateAdolescentHealthRequest request)
    {
        try
        {
            var adolescentHealth = await _context.AdolescentHealths.FirstOrDefaultAsync(a => a.Id == recordId);

            if (adolescentHealth == null)
            {
                throw new NotFoundException($"Adolescent health record with ID {recordId} not found");
            }

            UpdateRecord(adolescentHealth, request);
            adolescentHealth.UpdatedAt = DateTime.UtcNow;

            _context.AdolescentHealths.Update(adolescentHealth);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Adolescent health record updated: {adolescentHealth.Id}");

            var response = MapToResponse(adolescentHealth);
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
            _logger.LogError(ex, $"Error updating adolescent health record {recordId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while updating adolescent health record"
            });
        }
    }

    /// <summary>
    /// Delete adolescent health record for an appointment
    /// </summary>
    [HttpDelete("appointments/{appointmentId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAdolescentHealthRecordByAppointment(Guid appointmentId)
    {
        try
        {
            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.Id == appointmentId);

            if (appointment == null)
            {
                throw new NotFoundException($"Appointment with ID {appointmentId} not found");
            }

            var adolescentHealth = await _context.AdolescentHealths
                .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);

            if (adolescentHealth == null)
            {
                throw new NotFoundException("No adolescent health record found for this appointment");
            }

            _context.AdolescentHealths.Remove(adolescentHealth);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Adolescent health record deleted: {adolescentHealth.Id}");

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
            _logger.LogError(ex, $"Error deleting adolescent health record for appointment {appointmentId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while deleting adolescent health record"
            });
        }
    }

    /// <summary>
    /// Delete adolescent health record by ID
    /// </summary>
    [HttpDelete("{recordId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAdolescentHealthRecord(Guid recordId)
    {
        try
        {
            var adolescentHealth = await _context.AdolescentHealths.FirstOrDefaultAsync(a => a.Id == recordId);

            if (adolescentHealth == null)
            {
                throw new NotFoundException($"Adolescent health record with ID {recordId} not found");
            }

            _context.AdolescentHealths.Remove(adolescentHealth);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Adolescent health record deleted: {recordId}");

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
            _logger.LogError(ex, $"Error deleting adolescent health record {recordId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while deleting adolescent health record"
            });
        }
    }

    private AdolescentHealthResponse MapToResponse(AdolescentHealth health)
    {
        return new AdolescentHealthResponse
        {
            Id = health.Id,
            AppointmentId = health.AppointmentId,
            Citizenship = health.Citizenship,
            Residence = health.Residence,
            SchoolName = health.SchoolName,
            Grade = health.Grade,
            ChildOrder = health.ChildOrder,
            TotalSiblings = health.TotalSiblings,
            Occupation = health.Occupation,
            FatherEducation = health.FatherEducation,
            FatherOccupation = health.FatherOccupation,
            MotherEducation = health.MotherEducation,
            MotherOccupation = health.MotherOccupation,
            MaritalStatus = health.MaritalStatus,
            ParentStatus = health.ParentStatus,
            Weight = health.Weight,
            Height = health.Height,
            MainComplaint = health.MainComplaint,
            MedicalHistory = health.MedicalHistory,
            Diagnosis = health.Diagnosis,
            MenstrualDisorder = health.MenstrualDisorder,
            MenstrualDisorderNotes = health.MenstrualDisorderNotes,
            PremaritalSex = health.PremaritalSex,
            PremaritalSexNotes = health.PremaritalSexNotes,
            Pregnancy = health.Pregnancy,
            PregnancyNotes = health.PregnancyNotes,
            DesiredPregnancy = health.DesiredPregnancy,
            DesiredPregnancyNotes = health.DesiredPregnancyNotes,
            UnwantedPregnancy = health.UnwantedPregnancy,
            UnwantedPregnancyNotes = health.UnwantedPregnancyNotes,
            TeenageDelivery = health.TeenageDelivery,
            TeenageDeliveryNotes = health.TeenageDeliveryNotes,
            Abortion = health.Abortion,
            AbortionNotes = health.AbortionNotes,
            NutritionDisorder = health.NutritionDisorder,
            NutritionDisorderNotes = health.NutritionDisorderNotes,
            Anemia = health.Anemia,
            AnemiaNotes = health.AnemiaNotes,
            ChronicEnergyDeficiency = health.ChronicEnergyDeficiency,
            ChronicEnergyDeficiencyNotes = health.ChronicEnergyDeficiencyNotes,
            Obesity = health.Obesity,
            ObesityNotes = health.ObesityNotes,
            DrugAbuse = health.DrugAbuse,
            DrugAbuseNotes = health.DrugAbuseNotes,
            Smoking = health.Smoking,
            SmokingNotes = health.SmokingNotes,
            AlcoholUse = health.AlcoholUse,
            AlcoholUseNotes = health.AlcoholUseNotes,
            OtherSubstanceUse = health.OtherSubstanceUse,
            OtherSubstanceUseNotes = health.OtherSubstanceUseNotes,
            SexuallyTransmittedInfection = health.SexuallyTransmittedInfection,
            SexuallyTransmittedInfectionNotes = health.SexuallyTransmittedInfectionNotes,
            ReproductiveInfection = health.ReproductiveInfection,
            ReproductiveInfectionNotes = health.ReproductiveInfectionNotes,
            HIV = health.HIV,
            HIVNotes = health.HIVNotes,
            AIDS = health.AIDS,
            AIDSNotes = health.AIDSNotes,
            PsychologicalIssues = health.PsychologicalIssues,
            PsychologicalIssuesNotes = health.PsychologicalIssuesNotes,
            GadgetAddiction = health.GadgetAddiction,
            GadgetAddictionNotes = health.GadgetAddictionNotes,
            SexualOrientation = health.SexualOrientation,
            SexualOrientationNotes = health.SexualOrientationNotes,
            MentalDisability = health.MentalDisability,
            MentalDisabilityNotes = health.MentalDisabilityNotes,
            EarlyMarriage = health.EarlyMarriage,
            EarlyMarriageNotes = health.EarlyMarriageNotes,
            ChildAbuse = health.ChildAbuse,
            ChildAbuseNotes = health.ChildAbuseNotes,
            PhysicalDisability = health.PhysicalDisability,
            PhysicalDisabilityNotes = health.PhysicalDisabilityNotes,
            LearningDifficulty = health.LearningDifficulty,
            LearningDifficultyNotes = health.LearningDifficultyNotes,
            OtherProblems = health.OtherProblems,
            OtherProblemsNotes = health.OtherProblemsNotes,
            MainProblem = health.MainProblem,
            ProblemBackground = health.ProblemBackground,
            SolutionAlternatives = health.SolutionAlternatives,
            ClientDecision = health.ClientDecision,
            Observations = health.Observations,
            Counselor = health.Counselor,
            HomeAssessment = health.HomeAssessment,
            EmploymentEducationAssessment = health.EmploymentEducationAssessment,
            EatingAssessment = health.EatingAssessment,
            ActivityAssessment = health.ActivityAssessment,
            DrugsAssessment = health.DrugsAssessment,
            SexualityAssessment = health.SexualityAssessment,
            SafetyAssessment = health.SafetyAssessment,
            SuicideDepressionAssessment = health.SuicideDepressionAssessment,
            CreatedAt = health.CreatedAt,
            UpdatedAt = health.UpdatedAt
        };
    }

    private void UpdateRecord(AdolescentHealth health, UpdateAdolescentHealthRequest request)
    {
        if (!string.IsNullOrEmpty(request.Citizenship))
            health.Citizenship = request.Citizenship;
        if (!string.IsNullOrEmpty(request.Residence))
            health.Residence = request.Residence;
        if (!string.IsNullOrEmpty(request.SchoolName))
            health.SchoolName = request.SchoolName;
        if (!string.IsNullOrEmpty(request.Grade))
            health.Grade = request.Grade;
        if (request.ChildOrder.HasValue)
            health.ChildOrder = request.ChildOrder;
        if (request.TotalSiblings.HasValue)
            health.TotalSiblings = request.TotalSiblings;
        if (!string.IsNullOrEmpty(request.Occupation))
            health.Occupation = request.Occupation;
        if (!string.IsNullOrEmpty(request.FatherEducation))
            health.FatherEducation = request.FatherEducation;
        if (!string.IsNullOrEmpty(request.FatherOccupation))
            health.FatherOccupation = request.FatherOccupation;
        if (!string.IsNullOrEmpty(request.MotherEducation))
            health.MotherEducation = request.MotherEducation;
        if (!string.IsNullOrEmpty(request.MotherOccupation))
            health.MotherOccupation = request.MotherOccupation;
        if (request.MaritalStatus.HasValue)
            health.MaritalStatus = request.MaritalStatus;
        if (request.ParentStatus.HasValue)
            health.ParentStatus = request.ParentStatus;
        if (request.Weight.HasValue)
            health.Weight = request.Weight;
        if (request.Height.HasValue)
            health.Height = request.Height;
        if (!string.IsNullOrEmpty(request.MainComplaint))
            health.MainComplaint = request.MainComplaint;
        if (!string.IsNullOrEmpty(request.MedicalHistory))
            health.MedicalHistory = request.MedicalHistory;
        if (!string.IsNullOrEmpty(request.Diagnosis))
            health.Diagnosis = request.Diagnosis;
        if (request.MenstrualDisorder.HasValue)
            health.MenstrualDisorder = request.MenstrualDisorder;
        if (!string.IsNullOrEmpty(request.MenstrualDisorderNotes))
            health.MenstrualDisorderNotes = request.MenstrualDisorderNotes;
        if (request.PremaritalSex.HasValue)
            health.PremaritalSex = request.PremaritalSex;
        if (!string.IsNullOrEmpty(request.PremaritalSexNotes))
            health.PremaritalSexNotes = request.PremaritalSexNotes;
        if (request.Pregnancy.HasValue)
            health.Pregnancy = request.Pregnancy;
        if (!string.IsNullOrEmpty(request.PregnancyNotes))
            health.PregnancyNotes = request.PregnancyNotes;
        if (request.DesiredPregnancy.HasValue)
            health.DesiredPregnancy = request.DesiredPregnancy;
        if (!string.IsNullOrEmpty(request.DesiredPregnancyNotes))
            health.DesiredPregnancyNotes = request.DesiredPregnancyNotes;
        if (request.UnwantedPregnancy.HasValue)
            health.UnwantedPregnancy = request.UnwantedPregnancy;
        if (!string.IsNullOrEmpty(request.UnwantedPregnancyNotes))
            health.UnwantedPregnancyNotes = request.UnwantedPregnancyNotes;
        if (request.TeenageDelivery.HasValue)
            health.TeenageDelivery = request.TeenageDelivery;
        if (!string.IsNullOrEmpty(request.TeenageDeliveryNotes))
            health.TeenageDeliveryNotes = request.TeenageDeliveryNotes;
        if (request.Abortion.HasValue)
            health.Abortion = request.Abortion;
        if (!string.IsNullOrEmpty(request.AbortionNotes))
            health.AbortionNotes = request.AbortionNotes;
        if (request.NutritionDisorder.HasValue)
            health.NutritionDisorder = request.NutritionDisorder;
        if (!string.IsNullOrEmpty(request.NutritionDisorderNotes))
            health.NutritionDisorderNotes = request.NutritionDisorderNotes;
        if (request.Anemia.HasValue)
            health.Anemia = request.Anemia;
        if (!string.IsNullOrEmpty(request.AnemiaNotes))
            health.AnemiaNotes = request.AnemiaNotes;
        if (request.ChronicEnergyDeficiency.HasValue)
            health.ChronicEnergyDeficiency = request.ChronicEnergyDeficiency;
        if (!string.IsNullOrEmpty(request.ChronicEnergyDeficiencyNotes))
            health.ChronicEnergyDeficiencyNotes = request.ChronicEnergyDeficiencyNotes;
        if (request.Obesity.HasValue)
            health.Obesity = request.Obesity;
        if (!string.IsNullOrEmpty(request.ObesityNotes))
            health.ObesityNotes = request.ObesityNotes;
        if (request.DrugAbuse.HasValue)
            health.DrugAbuse = request.DrugAbuse;
        if (!string.IsNullOrEmpty(request.DrugAbuseNotes))
            health.DrugAbuseNotes = request.DrugAbuseNotes;
        if (request.Smoking.HasValue)
            health.Smoking = request.Smoking;
        if (!string.IsNullOrEmpty(request.SmokingNotes))
            health.SmokingNotes = request.SmokingNotes;
        if (request.AlcoholUse.HasValue)
            health.AlcoholUse = request.AlcoholUse;
        if (!string.IsNullOrEmpty(request.AlcoholUseNotes))
            health.AlcoholUseNotes = request.AlcoholUseNotes;
        if (request.OtherSubstanceUse.HasValue)
            health.OtherSubstanceUse = request.OtherSubstanceUse;
        if (!string.IsNullOrEmpty(request.OtherSubstanceUseNotes))
            health.OtherSubstanceUseNotes = request.OtherSubstanceUseNotes;
        if (request.SexuallyTransmittedInfection.HasValue)
            health.SexuallyTransmittedInfection = request.SexuallyTransmittedInfection;
        if (!string.IsNullOrEmpty(request.SexuallyTransmittedInfectionNotes))
            health.SexuallyTransmittedInfectionNotes = request.SexuallyTransmittedInfectionNotes;
        if (request.ReproductiveInfection.HasValue)
            health.ReproductiveInfection = request.ReproductiveInfection;
        if (!string.IsNullOrEmpty(request.ReproductiveInfectionNotes))
            health.ReproductiveInfectionNotes = request.ReproductiveInfectionNotes;
        if (request.HIV.HasValue)
            health.HIV = request.HIV;
        if (!string.IsNullOrEmpty(request.HIVNotes))
            health.HIVNotes = request.HIVNotes;
        if (request.AIDS.HasValue)
            health.AIDS = request.AIDS;
        if (!string.IsNullOrEmpty(request.AIDSNotes))
            health.AIDSNotes = request.AIDSNotes;
        if (request.PsychologicalIssues.HasValue)
            health.PsychologicalIssues = request.PsychologicalIssues;
        if (!string.IsNullOrEmpty(request.PsychologicalIssuesNotes))
            health.PsychologicalIssuesNotes = request.PsychologicalIssuesNotes;
        if (request.GadgetAddiction.HasValue)
            health.GadgetAddiction = request.GadgetAddiction;
        if (!string.IsNullOrEmpty(request.GadgetAddictionNotes))
            health.GadgetAddictionNotes = request.GadgetAddictionNotes;
        if (request.SexualOrientation.HasValue)
            health.SexualOrientation = request.SexualOrientation;
        if (!string.IsNullOrEmpty(request.SexualOrientationNotes))
            health.SexualOrientationNotes = request.SexualOrientationNotes;
        if (request.MentalDisability.HasValue)
            health.MentalDisability = request.MentalDisability;
        if (!string.IsNullOrEmpty(request.MentalDisabilityNotes))
            health.MentalDisabilityNotes = request.MentalDisabilityNotes;
        if (request.EarlyMarriage.HasValue)
            health.EarlyMarriage = request.EarlyMarriage;
        if (!string.IsNullOrEmpty(request.EarlyMarriageNotes))
            health.EarlyMarriageNotes = request.EarlyMarriageNotes;
        if (request.ChildAbuse.HasValue)
            health.ChildAbuse = request.ChildAbuse;
        if (!string.IsNullOrEmpty(request.ChildAbuseNotes))
            health.ChildAbuseNotes = request.ChildAbuseNotes;
        if (request.PhysicalDisability.HasValue)
            health.PhysicalDisability = request.PhysicalDisability;
        if (!string.IsNullOrEmpty(request.PhysicalDisabilityNotes))
            health.PhysicalDisabilityNotes = request.PhysicalDisabilityNotes;
        if (request.LearningDifficulty.HasValue)
            health.LearningDifficulty = request.LearningDifficulty;
        if (!string.IsNullOrEmpty(request.LearningDifficultyNotes))
            health.LearningDifficultyNotes = request.LearningDifficultyNotes;
        if (request.OtherProblems.HasValue)
            health.OtherProblems = request.OtherProblems;
        if (!string.IsNullOrEmpty(request.OtherProblemsNotes))
            health.OtherProblemsNotes = request.OtherProblemsNotes;
        if (!string.IsNullOrEmpty(request.MainProblem))
            health.MainProblem = request.MainProblem;
        if (!string.IsNullOrEmpty(request.ProblemBackground))
            health.ProblemBackground = request.ProblemBackground;
        if (!string.IsNullOrEmpty(request.SolutionAlternatives))
            health.SolutionAlternatives = request.SolutionAlternatives;
        if (!string.IsNullOrEmpty(request.ClientDecision))
            health.ClientDecision = request.ClientDecision;
        if (!string.IsNullOrEmpty(request.Observations))
            health.Observations = request.Observations;
        if (!string.IsNullOrEmpty(request.Counselor))
            health.Counselor = request.Counselor;
        if (!string.IsNullOrEmpty(request.HomeAssessment))
            health.HomeAssessment = request.HomeAssessment;
        if (!string.IsNullOrEmpty(request.EmploymentEducationAssessment))
            health.EmploymentEducationAssessment = request.EmploymentEducationAssessment;
        if (!string.IsNullOrEmpty(request.EatingAssessment))
            health.EatingAssessment = request.EatingAssessment;
        if (!string.IsNullOrEmpty(request.ActivityAssessment))
            health.ActivityAssessment = request.ActivityAssessment;
        if (!string.IsNullOrEmpty(request.DrugsAssessment))
            health.DrugsAssessment = request.DrugsAssessment;
        if (!string.IsNullOrEmpty(request.SexualityAssessment))
            health.SexualityAssessment = request.SexualityAssessment;
        if (!string.IsNullOrEmpty(request.SafetyAssessment))
            health.SafetyAssessment = request.SafetyAssessment;
        if (!string.IsNullOrEmpty(request.SuicideDepressionAssessment))
            health.SuicideDepressionAssessment = request.SuicideDepressionAssessment;
    }
}
