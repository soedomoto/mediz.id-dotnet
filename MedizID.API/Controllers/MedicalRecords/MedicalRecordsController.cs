using MedizID.API.Common.Exceptions;
using MedizID.API.Data;
using MedizID.API.DTOs;
using MedizID.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedizID.API.Controllers.MedicalRecords;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
[Authorize]
public class MedicalRecordsController : ControllerBase
{
    private readonly MedizIDDbContext _context;
    private readonly ILogger<MedicalRecordsController> _logger;

    public MedicalRecordsController(MedizIDDbContext context, ILogger<MedicalRecordsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Get all medical records with pagination
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<MedicalRecordResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMedicalRecords(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] Guid? patientId = null)
    {
        try
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var query = _context.MedicalRecords.AsQueryable();

            if (patientId.HasValue)
                query = query.Where(mr => mr.PatientId == patientId.Value);

            var total = await query.CountAsync();

            var records = await query
                .Include(mr => mr.Patient)
                .Include(mr => mr.Doctor)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(mr => new MedicalRecordResponse
                {
                    Id = mr.Id,
                    PatientId = mr.PatientId,
                    PatientName = mr.Patient != null ? $"{mr.Patient.FirstName} {mr.Patient.LastName}" : "",
                    VisitDate = mr.VisitDate,
                    ChiefComplaint = mr.ChiefComplaint,
                    Diagnosis = mr.Diagnosis,
                    Treatment = mr.Treatment,
                    CreatedAt = mr.CreatedAt
                })
                .ToListAsync();

            return Ok(new PagedResult<MedicalRecordResponse>
            {
                Items = records,
                Total = total,
                Page = page,
                PageSize = pageSize
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching medical records");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching medical records"
            });
        }
    }

    /// <summary>
    /// Get medical record by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(MedicalRecordResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMedicalRecord(Guid id)
    {
        try
        {
            var record = await _context.MedicalRecords
                .Include(mr => mr.Patient)
                .Include(mr => mr.Doctor)
                .Include(mr => mr.Diagnoses)
                .Include(mr => mr.Prescriptions)
                .Include(mr => mr.LaboratoriumTests)
                .FirstOrDefaultAsync(mr => mr.Id == id);

            if (record == null)
            {
                _logger.LogWarning($"Medical record not found: {id}");
                throw new NotFoundException($"Medical record with ID {id} not found");
            }

            var response = new MedicalRecordResponse
            {
                Id = record.Id,
                PatientId = record.PatientId,
                PatientName = record.Patient != null ? $"{record.Patient.FirstName} {record.Patient.LastName}" : "",
                VisitDate = record.VisitDate,
                ChiefComplaint = record.ChiefComplaint,
                Diagnosis = record.Diagnosis,
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
            _logger.LogError(ex, $"Error fetching medical record {id}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching the medical record"
            });
        }
    }

    /// <summary>
    /// Create a new medical record
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(MedicalRecordResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateMedicalRecord([FromBody] CreateMedicalRecordRequest request)
    {
        try
        {
            // Validate patient exists
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Id == request.PatientId);
            if (patient == null)
            {
                return BadRequest(new ErrorResponse
                {
                    ErrorCode = "PATIENT_NOT_FOUND",
                    Message = "Patient not found"
                });
            }

            var record = new MedicalRecord
            {
                Id = Guid.NewGuid(),
                PatientId = request.PatientId,
                AppointmentId = request.AppointmentId,
                VisitDate = request.VisitDate,
                ChiefComplaint = request.ChiefComplaint,
                Diagnosis = request.Diagnosis,
                Treatment = request.Treatment,
                Notes = request.Notes,
                CreatedAt = DateTime.UtcNow
            };

            _context.MedicalRecords.Add(record);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Medical record created: {record.Id}");

            var response = new MedicalRecordResponse
            {
                Id = record.Id,
                PatientId = record.PatientId,
                PatientName = $"{patient.FirstName} {patient.LastName}",
                VisitDate = record.VisitDate,
                ChiefComplaint = record.ChiefComplaint,
                Diagnosis = record.Diagnosis,
                Treatment = record.Treatment,
                CreatedAt = record.CreatedAt
            };

            return CreatedAtAction(nameof(GetMedicalRecord), new { id = record.Id }, response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating medical record");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while creating the medical record"
            });
        }
    }

    /// <summary>
    /// Update medical record
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(MedicalRecordResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateMedicalRecord(Guid id, [FromBody] UpdateMedicalRecordRequest request)
    {
        try
        {
            var record = await _context.MedicalRecords
                .Include(mr => mr.Patient)
                .FirstOrDefaultAsync(mr => mr.Id == id);

            if (record == null)
            {
                throw new NotFoundException($"Medical record with ID {id} not found");
            }

            if (!string.IsNullOrEmpty(request.ChiefComplaint))
                record.ChiefComplaint = request.ChiefComplaint;

            if (!string.IsNullOrEmpty(request.Diagnosis))
                record.Diagnosis = request.Diagnosis;

            if (!string.IsNullOrEmpty(request.Treatment))
                record.Treatment = request.Treatment;

            if (!string.IsNullOrEmpty(request.Notes))
                record.Notes = request.Notes;

            record.UpdatedAt = DateTime.UtcNow;

            _context.MedicalRecords.Update(record);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Medical record updated: {record.Id}");

            var response = new MedicalRecordResponse
            {
                Id = record.Id,
                PatientId = record.PatientId,
                PatientName = record.Patient != null ? $"{record.Patient.FirstName} {record.Patient.LastName}" : "",
                VisitDate = record.VisitDate,
                ChiefComplaint = record.ChiefComplaint,
                Diagnosis = record.Diagnosis,
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
            _logger.LogError(ex, $"Error updating medical record {id}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while updating the medical record"
            });
        }
    }

    /// <summary>
    /// Delete medical record
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteMedicalRecord(Guid id)
    {
        try
        {
            var record = await _context.MedicalRecords.FirstOrDefaultAsync(mr => mr.Id == id);

            if (record == null)
            {
                throw new NotFoundException($"Medical record with ID {id} not found");
            }

            _context.MedicalRecords.Remove(record);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Medical record deleted: {id}");

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
            _logger.LogError(ex, $"Error deleting medical record {id}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while deleting the medical record"
            });
        }
    }
}
