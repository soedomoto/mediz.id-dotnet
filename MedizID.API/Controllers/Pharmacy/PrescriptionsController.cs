using MedizID.API.Common.Exceptions;
using MedizID.API.Data;
using MedizID.API.DTOs;
using MedizID.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedizID.API.Controllers.Pharmacy;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
[Authorize]
public class PrescriptionsController : ControllerBase
{
    private readonly MedizIDDbContext _context;
    private readonly ILogger<PrescriptionsController> _logger;

    public PrescriptionsController(MedizIDDbContext context, ILogger<PrescriptionsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Get all prescriptions with pagination
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<PrescriptionResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPrescriptions(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] Guid? appointmentId = null)
    {
        try
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var query = _context.Prescriptions.AsQueryable();

            if (appointmentId.HasValue)
                query = query.Where(p => p.AppointmentId == appointmentId.Value);

            var total = await query.CountAsync();

            var prescriptions = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new PrescriptionResponse
                {
                    Id = p.Id,
                    MedicationName = p.MedicationName,
                    Dosage = p.Dosage,
                    Frequency = p.Frequency,
                    Duration = p.Duration,
                    CreatedAt = p.CreatedAt
                })
                .ToListAsync();

            return Ok(new PagedResult<PrescriptionResponse>
            {
                Items = prescriptions,
                Total = total,
                Page = page,
                PageSize = pageSize
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching prescriptions");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching prescriptions"
            });
        }
    }

    /// <summary>
    /// Get prescription by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PrescriptionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPrescription(Guid id)
    {
        try
        {
            var prescription = await _context.Prescriptions.FirstOrDefaultAsync(p => p.Id == id);

            if (prescription == null)
            {
                _logger.LogWarning($"Prescription not found: {id}");
                throw new NotFoundException($"Prescription with ID {id} not found");
            }

            var response = new PrescriptionResponse
            {
                Id = prescription.Id,
                MedicationName = prescription.MedicationName,
                Dosage = prescription.Dosage,
                Frequency = prescription.Frequency,
                Duration = prescription.Duration,
                CreatedAt = prescription.CreatedAt
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
            _logger.LogError(ex, $"Error fetching prescription {id}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching the prescription"
            });
        }
    }

    /// <summary>
    /// Create a new prescription
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(PrescriptionResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreatePrescription([FromBody] CreatePrescriptionRequest request)
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

            var prescription = new Prescription
            {
                Id = Guid.NewGuid(),
                AppointmentId = request.AppointmentId,
                MedicationName = request.MedicationName,
                Dosage = request.Dosage,
                Frequency = request.Frequency,
                Duration = request.Duration,
                Instructions = request.Instructions,
                CreatedAt = DateTime.UtcNow
            };

            _context.Prescriptions.Add(prescription);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Prescription created: {prescription.Id}");

            var response = new PrescriptionResponse
            {
                Id = prescription.Id,
                MedicationName = prescription.MedicationName,
                Dosage = prescription.Dosage,
                Frequency = prescription.Frequency,
                Duration = prescription.Duration,
                CreatedAt = prescription.CreatedAt
            };

            return CreatedAtAction(nameof(GetPrescription), new { id = prescription.Id }, response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating prescription");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while creating the prescription"
            });
        }
    }

    /// <summary>
    /// Update prescription
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(PrescriptionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdatePrescription(Guid id, [FromBody] CreatePrescriptionRequest request)
    {
        try
        {
            var prescription = await _context.Prescriptions.FirstOrDefaultAsync(p => p.Id == id);

            if (prescription == null)
            {
                throw new NotFoundException($"Prescription with ID {id} not found");
            }

            prescription.MedicationName = request.MedicationName;
            prescription.Dosage = request.Dosage;
            prescription.Frequency = request.Frequency;
            prescription.Duration = request.Duration;
            prescription.Instructions = request.Instructions;

            _context.Prescriptions.Update(prescription);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Prescription updated: {prescription.Id}");

            var response = new PrescriptionResponse
            {
                Id = prescription.Id,
                MedicationName = prescription.MedicationName,
                Dosage = prescription.Dosage,
                Frequency = prescription.Frequency,
                Duration = prescription.Duration,
                CreatedAt = prescription.CreatedAt
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
            _logger.LogError(ex, $"Error updating prescription {id}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while updating the prescription"
            });
        }
    }

    /// <summary>
    /// Delete prescription
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeletePrescription(Guid id)
    {
        try
        {
            var prescription = await _context.Prescriptions.FirstOrDefaultAsync(p => p.Id == id);

            if (prescription == null)
            {
                throw new NotFoundException($"Prescription with ID {id} not found");
            }

            _context.Prescriptions.Remove(prescription);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Prescription deleted: {id}");

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
            _logger.LogError(ex, $"Error deleting prescription {id}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while deleting the prescription"
            });
        }
    }
}
