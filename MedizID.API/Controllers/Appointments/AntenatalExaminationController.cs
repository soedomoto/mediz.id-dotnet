using MedizID.API.Common.Exceptions;
using MedizID.API.Data;
using MedizID.API.DTOs;
using MedizID.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedizID.API.Controllers.Appointments;

[ApiController]
[Route("api/v1/maternal-child-health/antenatal-examination")]
[Produces("application/json")]
[Authorize]
public class AntenatalExaminationController : ControllerBase
{
    private readonly MedizIDDbContext _context;
    private readonly ILogger<AntenatalExaminationController> _logger;

    public AntenatalExaminationController(MedizIDDbContext context, ILogger<AntenatalExaminationController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Create Antenatal Examination record
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(AntenatalExaminationResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateExamination([FromBody] CreateAntenatalExaminationRequest request)
    {
        try
        {
            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.Id == request.AppointmentId);

            if (appointment == null)
            {
                throw new NotFoundException($"Appointment with ID {request.AppointmentId} not found");
            }

            var examination = new AntenatalExamination
            {
                Id = Guid.NewGuid(),
                AppointmentId = request.AppointmentId,
                PregnancyWeeks = request.PregnancyWeeks,
                Trimester = request.Trimester,
                FetalHeartRate = request.FetalHeartRate,
                HeadPosition = request.HeadPosition,
                EstimatedFetalWeight = request.EstimatedFetalWeight,
                Presentation = request.Presentation,
                FetalCount = request.FetalCount,
                ClinicalHistory = request.ClinicalHistory,
                Height = request.Height,
                Weight = request.Weight,
                BloodPressure = request.BloodPressure,
                UpperArmCircumference = request.UpperArmCircumference,
                NutritionStatus = request.NutritionStatus,
                FundusHeight = request.FundusHeight,
                PatellaReflex = request.PatellaReflex,
                Hemoglobin = request.Hemoglobin,
                Anemia = request.Anemia,
                ProteinUrine = request.ProteinUrine,
                UrineReducingSubstances = request.UrineReducingSubstances,
                BloodGlucose = request.BloodGlucose,
                CreatedAt = DateTime.UtcNow
            };

            _context.AntenatalExaminations.Add(examination);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Antenatal Examination record created: {examination.Id}");

            var response = new AntenatalExaminationResponse
            {
                Id = examination.Id,
                AppointmentId = examination.AppointmentId,
                PregnancyWeeks = examination.PregnancyWeeks,
                Trimester = examination.Trimester,
                FetalHeartRate = examination.FetalHeartRate,
                HeadPosition = examination.HeadPosition,
                EstimatedFetalWeight = examination.EstimatedFetalWeight,
                Presentation = examination.Presentation,
                FetalCount = examination.FetalCount,
                ClinicalHistory = examination.ClinicalHistory,
                Height = examination.Height,
                Weight = examination.Weight,
                BloodPressure = examination.BloodPressure,
                UpperArmCircumference = examination.UpperArmCircumference,
                NutritionStatus = examination.NutritionStatus,
                FundusHeight = examination.FundusHeight,
                PatellaReflex = examination.PatellaReflex,
                Hemoglobin = examination.Hemoglobin,
                Anemia = examination.Anemia,
                ProteinUrine = examination.ProteinUrine,
                UrineReducingSubstances = examination.UrineReducingSubstances,
                BloodGlucose = examination.BloodGlucose,
                CreatedAt = examination.CreatedAt
            };

            return CreatedAtAction(nameof(GetExaminationById), new { examinationId = examination.Id }, response);
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
            _logger.LogError(ex, "Error creating Antenatal Examination record");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while creating Antenatal Examination record"
            });
        }
    }

    /// <summary>
    /// Get all Antenatal Examination records with pagination
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<AntenatalExaminationResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetExaminationList([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var query = _context.AntenatalExaminations.AsQueryable();
            var total = await query.CountAsync();

            var examinationList = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new AntenatalExaminationResponse
                {
                    Id = e.Id,
                    AppointmentId = e.AppointmentId,
                    PregnancyWeeks = e.PregnancyWeeks,
                    Trimester = e.Trimester,
                    FetalHeartRate = e.FetalHeartRate,
                    HeadPosition = e.HeadPosition,
                    EstimatedFetalWeight = e.EstimatedFetalWeight,
                    Presentation = e.Presentation,
                    FetalCount = e.FetalCount,
                    ClinicalHistory = e.ClinicalHistory,
                    Height = e.Height,
                    Weight = e.Weight,
                    BloodPressure = e.BloodPressure,
                    UpperArmCircumference = e.UpperArmCircumference,
                    NutritionStatus = e.NutritionStatus,
                    FundusHeight = e.FundusHeight,
                    PatellaReflex = e.PatellaReflex,
                    Hemoglobin = e.Hemoglobin,
                    Anemia = e.Anemia,
                    ProteinUrine = e.ProteinUrine,
                    UrineReducingSubstances = e.UrineReducingSubstances,
                    BloodGlucose = e.BloodGlucose,
                    CreatedAt = e.CreatedAt,
                    UpdatedAt = e.UpdatedAt
                })
                .ToListAsync();

            return Ok(new PagedResult<AntenatalExaminationResponse>
            {
                Items = examinationList,
                Total = total,
                Page = page,
                PageSize = pageSize
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching Antenatal Examination records");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching Antenatal Examination records"
            });
        }
    }

    /// <summary>
    /// Get Antenatal Examination record by ID
    /// </summary>
    [HttpGet("{examinationId}")]
    [ProducesResponseType(typeof(AntenatalExaminationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetExaminationById(Guid examinationId)
    {
        try
        {
            var examination = await _context.AntenatalExaminations
                .FirstOrDefaultAsync(e => e.Id == examinationId);

            if (examination == null)
            {
                throw new NotFoundException($"Antenatal Examination record with ID {examinationId} not found");
            }

            var response = new AntenatalExaminationResponse
            {
                Id = examination.Id,
                AppointmentId = examination.AppointmentId,
                PregnancyWeeks = examination.PregnancyWeeks,
                Trimester = examination.Trimester,
                FetalHeartRate = examination.FetalHeartRate,
                HeadPosition = examination.HeadPosition,
                EstimatedFetalWeight = examination.EstimatedFetalWeight,
                Presentation = examination.Presentation,
                FetalCount = examination.FetalCount,
                ClinicalHistory = examination.ClinicalHistory,
                Height = examination.Height,
                Weight = examination.Weight,
                BloodPressure = examination.BloodPressure,
                UpperArmCircumference = examination.UpperArmCircumference,
                NutritionStatus = examination.NutritionStatus,
                FundusHeight = examination.FundusHeight,
                PatellaReflex = examination.PatellaReflex,
                Hemoglobin = examination.Hemoglobin,
                Anemia = examination.Anemia,
                ProteinUrine = examination.ProteinUrine,
                UrineReducingSubstances = examination.UrineReducingSubstances,
                BloodGlucose = examination.BloodGlucose,
                CreatedAt = examination.CreatedAt,
                UpdatedAt = examination.UpdatedAt
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
            _logger.LogError(ex, $"Error fetching Antenatal Examination record {examinationId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching Antenatal Examination record"
            });
        }
    }

    /// <summary>
    /// Get Antenatal Examination by appointment
    /// </summary>
    [HttpGet("appointment/{appointmentId}")]
    [ProducesResponseType(typeof(AntenatalExaminationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetExaminationByAppointment(Guid appointmentId)
    {
        try
        {
            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.Id == appointmentId);

            if (appointment == null)
            {
                throw new NotFoundException($"Appointment with ID {appointmentId} not found");
            }

            var examination = await _context.AntenatalExaminations
                .FirstOrDefaultAsync(e => e.AppointmentId == appointmentId);

            if (examination == null)
            {
                throw new NotFoundException("No Antenatal Examination record found for this appointment");
            }

            var response = new AntenatalExaminationResponse
            {
                Id = examination.Id,
                AppointmentId = examination.AppointmentId,
                PregnancyWeeks = examination.PregnancyWeeks,
                Trimester = examination.Trimester,
                FetalHeartRate = examination.FetalHeartRate,
                HeadPosition = examination.HeadPosition,
                EstimatedFetalWeight = examination.EstimatedFetalWeight,
                Presentation = examination.Presentation,
                FetalCount = examination.FetalCount,
                ClinicalHistory = examination.ClinicalHistory,
                Height = examination.Height,
                Weight = examination.Weight,
                BloodPressure = examination.BloodPressure,
                UpperArmCircumference = examination.UpperArmCircumference,
                NutritionStatus = examination.NutritionStatus,
                FundusHeight = examination.FundusHeight,
                PatellaReflex = examination.PatellaReflex,
                Hemoglobin = examination.Hemoglobin,
                Anemia = examination.Anemia,
                ProteinUrine = examination.ProteinUrine,
                UrineReducingSubstances = examination.UrineReducingSubstances,
                BloodGlucose = examination.BloodGlucose,
                CreatedAt = examination.CreatedAt,
                UpdatedAt = examination.UpdatedAt
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
            _logger.LogError(ex, $"Error fetching Antenatal Examination record for appointment {appointmentId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching Antenatal Examination record"
            });
        }
    }

    /// <summary>
    /// Update Antenatal Examination record
    /// </summary>
    [HttpPut("{examinationId}")]
    [ProducesResponseType(typeof(AntenatalExaminationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateExamination(Guid examinationId, [FromBody] UpdateAntenatalExaminationRequest request)
    {
        try
        {
            var examination = await _context.AntenatalExaminations
                .FirstOrDefaultAsync(e => e.Id == examinationId);

            if (examination == null)
            {
                throw new NotFoundException($"Antenatal Examination record with ID {examinationId} not found");
            }

            examination.PregnancyWeeks = request.PregnancyWeeks ?? examination.PregnancyWeeks;
            examination.Trimester = request.Trimester ?? examination.Trimester;
            examination.FetalHeartRate = request.FetalHeartRate ?? examination.FetalHeartRate;
            examination.HeadPosition = request.HeadPosition ?? examination.HeadPosition;
            examination.EstimatedFetalWeight = request.EstimatedFetalWeight ?? examination.EstimatedFetalWeight;
            examination.Presentation = request.Presentation ?? examination.Presentation;
            examination.FetalCount = request.FetalCount ?? examination.FetalCount;
            examination.ClinicalHistory = request.ClinicalHistory ?? examination.ClinicalHistory;
            examination.Height = request.Height ?? examination.Height;
            examination.Weight = request.Weight ?? examination.Weight;
            examination.BloodPressure = request.BloodPressure ?? examination.BloodPressure;
            examination.UpperArmCircumference = request.UpperArmCircumference ?? examination.UpperArmCircumference;
            examination.NutritionStatus = request.NutritionStatus ?? examination.NutritionStatus;
            examination.FundusHeight = request.FundusHeight ?? examination.FundusHeight;
            examination.PatellaReflex = request.PatellaReflex ?? examination.PatellaReflex;
            examination.Hemoglobin = request.Hemoglobin ?? examination.Hemoglobin;
            examination.Anemia = request.Anemia ?? examination.Anemia;
            examination.ProteinUrine = request.ProteinUrine ?? examination.ProteinUrine;
            examination.UrineReducingSubstances = request.UrineReducingSubstances ?? examination.UrineReducingSubstances;
            examination.BloodGlucose = request.BloodGlucose ?? examination.BloodGlucose;
            examination.UpdatedAt = DateTime.UtcNow;

            _context.AntenatalExaminations.Update(examination);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Antenatal Examination record updated: {examination.Id}");

            var response = new AntenatalExaminationResponse
            {
                Id = examination.Id,
                AppointmentId = examination.AppointmentId,
                PregnancyWeeks = examination.PregnancyWeeks,
                Trimester = examination.Trimester,
                FetalHeartRate = examination.FetalHeartRate,
                HeadPosition = examination.HeadPosition,
                EstimatedFetalWeight = examination.EstimatedFetalWeight,
                Presentation = examination.Presentation,
                FetalCount = examination.FetalCount,
                ClinicalHistory = examination.ClinicalHistory,
                Height = examination.Height,
                Weight = examination.Weight,
                BloodPressure = examination.BloodPressure,
                UpperArmCircumference = examination.UpperArmCircumference,
                NutritionStatus = examination.NutritionStatus,
                FundusHeight = examination.FundusHeight,
                PatellaReflex = examination.PatellaReflex,
                Hemoglobin = examination.Hemoglobin,
                Anemia = examination.Anemia,
                ProteinUrine = examination.ProteinUrine,
                UrineReducingSubstances = examination.UrineReducingSubstances,
                BloodGlucose = examination.BloodGlucose,
                CreatedAt = examination.CreatedAt,
                UpdatedAt = examination.UpdatedAt
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
            _logger.LogError(ex, $"Error updating Antenatal Examination record {examinationId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while updating Antenatal Examination record"
            });
        }
    }

    /// <summary>
    /// Delete Antenatal Examination record
    /// </summary>
    [HttpDelete("{examinationId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteExamination(Guid examinationId)
    {
        try
        {
            var examination = await _context.AntenatalExaminations
                .FirstOrDefaultAsync(e => e.Id == examinationId);

            if (examination == null)
            {
                throw new NotFoundException($"Antenatal Examination record with ID {examinationId} not found");
            }

            _context.AntenatalExaminations.Remove(examination);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Antenatal Examination record deleted: {examinationId}");

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
            _logger.LogError(ex, $"Error deleting Antenatal Examination record {examinationId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while deleting Antenatal Examination record"
            });
        }
    }
}
