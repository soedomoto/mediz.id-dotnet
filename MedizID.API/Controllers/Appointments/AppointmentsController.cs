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
public class AppointmentsController : ControllerBase
{
    private readonly MedizIDDbContext _context;
    private readonly ILogger<AppointmentsController> _logger;

    public AppointmentsController(MedizIDDbContext context, ILogger<AppointmentsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Get all appointments with pagination
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<AppointmentResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAppointments(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] Guid? patientId = null)
    {
        try
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var query = _context.Appointments.AsQueryable();

            var total = await query.CountAsync();

            var appointments = await query
                .Include(a => a.FacilityPatient)
                .ThenInclude(fp => fp.Patient)
                .Include(a => a.FacilityDoctor)
                .ThenInclude(fs => fs.Staff)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new AppointmentResponse
                {
                    Id = a.Id,
                    FacilityPatientId = a.FacilityPatientId,
                    PatientName = a.FacilityPatient != null ? $"{a.FacilityPatient.Patient.FirstName} {a.FacilityPatient.Patient.LastName}" : "",
                    FacilityDoctorId = a.FacilityDoctorId,
                    DoctorName = a.FacilityDoctor != null ? $"{a.FacilityDoctor.Staff.FirstName} {a.FacilityDoctor.Staff.LastName}" : null,
                    AppointmentDate = a.AppointmentDate,
                    AppointmentTime = a.AppointmentTime,
                    Status = a.Status.ToString(),
                    Reason = a.Reason,
                    CreatedAt = a.CreatedAt
                })
                .ToListAsync();

            return Ok(new PagedResult<AppointmentResponse>
            {
                Items = appointments,
                Total = total,
                Page = page,
                PageSize = pageSize
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching appointments");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching appointments"
            });
        }
    }

    /// <summary>
    /// Get appointment by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(AppointmentDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAppointment(Guid id)
    {
        try
        {
            var appointment = await _context.Appointments
                .Include(a => a.FacilityPatient)
                .ThenInclude(fp => fp.Patient)
                .Include(a => a.FacilityDoctor)
                .ThenInclude(fs => fs.Staff)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null)
            {
                _logger.LogWarning($"Appointment not found: {id}");
                throw new NotFoundException($"Appointment with ID {id} not found");
            }

            var response = new AppointmentDetailResponse
            {
                Id = appointment.Id,
                FacilityPatientId = appointment.FacilityPatientId,
                PatientName = appointment.FacilityPatient != null ? $"{appointment.FacilityPatient.Patient.FirstName} {appointment.FacilityPatient.Patient.LastName}" : "",
                PatientIdNumber = appointment.FacilityPatient?.MedicalRecordNumber,
                FacilityDoctorId = appointment.FacilityDoctorId,
                DoctorName = appointment.FacilityDoctor != null ? $"{appointment.FacilityDoctor.Staff.FirstName} {appointment.FacilityDoctor.Staff.LastName}" : null,
                AppointmentDate = appointment.AppointmentDate,
                AppointmentTime = appointment.AppointmentTime,
                Status = appointment.Status.ToString(),
                Reason = appointment.Reason,
                Notes = appointment.Notes,
                Insurance = "Tidak ada data",
                CreatedAt = appointment.CreatedAt
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
            _logger.LogError(ex, $"Error fetching appointment {id}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching the appointment"
            });
        }
    }

    /// <summary>
    /// Create a new appointment
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(AppointmentResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentRequest request)
    {
        try
        {
            // Validate FacilityPatient exists
            var facilityPatient = await _context.FacilityPatients
                .Include(fp => fp.Patient)
                .FirstOrDefaultAsync(fp => fp.Id == request.FacilityPatientId);
            if (facilityPatient == null)
            {
                return BadRequest(new ErrorResponse
                {
                    ErrorCode = "FACILITY_PATIENT_NOT_FOUND",
                    Message = "Facility patient not found"
                });
            }

            // Validate FacilityStaff exists and has Doctor role
            var facilityStaff = await _context.FacilityStaffs
                .Include(fs => fs.Staff)
                .FirstOrDefaultAsync(fs => fs.Id == request.FacilityDoctorId && fs.Role == UserRoleEnum.Doctor);
            if (facilityStaff == null)
            {
                return BadRequest(new ErrorResponse
                {
                    ErrorCode = "FACILITY_STAFF_NOT_FOUND",
                    Message = "Doctor not found or does not have doctor role"
                });
            }

            // Check for appointment conflicts
            var existingAppointment = await _context.Appointments
                .FirstOrDefaultAsync(a =>
                    a.FacilityDoctorId == request.FacilityDoctorId &&
                    a.AppointmentDate == request.AppointmentDate &&
                    a.AppointmentTime == request.AppointmentTime &&
                    a.Status != AppointmentStatusEnum.Cancelled &&
                    a.Status != AppointmentStatusEnum.Completed);

            if (existingAppointment != null)
            {
                return BadRequest(new ErrorResponse
                {
                    ErrorCode = "APPOINTMENT_CONFLICT",
                    Message = "Doctor already has an appointment at this time"
                });
            }

            var appointment = new Appointment
            {
                Id = Guid.NewGuid(),
                FacilityPatientId = request.FacilityPatientId,
                FacilityDoctorId = request.FacilityDoctorId,
                FacilityId = facilityPatient.FacilityId,
                AppointmentDate = request.AppointmentDate.Kind == DateTimeKind.Local 
                    ? request.AppointmentDate.ToUniversalTime() 
                    : request.AppointmentDate,
                AppointmentTime = request.AppointmentTime,
                Reason = request.Reason,
                Notes = request.Notes,
                Status = AppointmentStatusEnum.Scheduled,
                CreatedAt = DateTime.UtcNow
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Appointment created: {appointment.Id}");

            var response = new AppointmentResponse
            {
                Id = appointment.Id,
                FacilityPatientId = appointment.FacilityPatientId,
                PatientName = $"{facilityPatient.Patient.FirstName} {facilityPatient.Patient.LastName}",
                FacilityDoctorId = appointment.FacilityDoctorId,
                DoctorName = $"{facilityStaff.Staff.FirstName} {facilityStaff.Staff.LastName}",
                AppointmentDate = appointment.AppointmentDate,
                AppointmentTime = appointment.AppointmentTime,
                Status = appointment.Status.ToString(),
                Reason = appointment.Reason,
                CreatedAt = appointment.CreatedAt
            };

            return CreatedAtAction(nameof(GetAppointment), new { id = appointment.Id }, response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating appointment");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while creating the appointment"
            });
        }
    }

    /// <summary>
    /// Update appointment
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(AppointmentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAppointment(Guid id, [FromBody] UpdateAppointmentRequest request)
    {
        try
        {
            var appointment = await _context.Appointments
                .Include(a => a.FacilityPatient)
                .ThenInclude(fp => fp.Patient)
                .Include(a => a.FacilityDoctor)
                .ThenInclude(fs => fs.Staff)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null)
            {
                throw new NotFoundException($"Appointment with ID {id} not found");
            }

            if (request.AppointmentDate.HasValue)
                appointment.AppointmentDate = request.AppointmentDate.Value.Kind == DateTimeKind.Local 
                    ? request.AppointmentDate.Value.ToUniversalTime() 
                    : request.AppointmentDate.Value;

            if (request.AppointmentTime.HasValue)
                appointment.AppointmentTime = request.AppointmentTime.Value;

            if (!string.IsNullOrEmpty(request.Status))
            {
                if (Enum.TryParse<AppointmentStatusEnum>(request.Status, true, out var status))
                    appointment.Status = status;
            }

            if (!string.IsNullOrEmpty(request.Reason))
                appointment.Reason = request.Reason;

            if (!string.IsNullOrEmpty(request.Notes))
                appointment.Notes = request.Notes;

            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Appointment updated: {appointment.Id}");

            var response = new AppointmentResponse
            {
                Id = appointment.Id,
                FacilityPatientId = appointment.FacilityPatientId,
                PatientName = appointment.FacilityPatient != null ? $"{appointment.FacilityPatient.Patient.FirstName} {appointment.FacilityPatient.Patient.LastName}" : "",
                FacilityDoctorId = appointment.FacilityDoctorId,
                DoctorName = appointment.FacilityDoctor != null ? $"{appointment.FacilityDoctor.Staff.FirstName} {appointment.FacilityDoctor.Staff.LastName}" : null,
                AppointmentDate = appointment.AppointmentDate,
                AppointmentTime = appointment.AppointmentTime,
                Status = appointment.Status.ToString(),
                Reason = appointment.Reason,
                CreatedAt = appointment.CreatedAt
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
            _logger.LogError(ex, $"Error updating appointment {id}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while updating the appointment"
            });
        }
    }

    /// <summary>
    /// Delete appointment
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAppointment(Guid id)
    {
        try
        {
            var appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null)
            {
                throw new NotFoundException($"Appointment with ID {id} not found");
            }

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Appointment deleted: {id}");

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
            _logger.LogError(ex, $"Error deleting appointment {id}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while deleting the appointment"
            });
        }
    }

    /// <summary>
    /// Get appointment medical history
    /// </summary>
    [HttpGet("{appointmentId}/medical-history")]
    [ProducesResponseType(typeof(List<AppointmentMedicalHistoryResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAppointmentMedicalHistory(Guid appointmentId)
    {
        try
        {
            var appointment = await _context.Appointments
                .Include(a => a.FacilityPatient)
                .FirstOrDefaultAsync(a => a.Id == appointmentId);

            if (appointment == null)
            {
                return NotFound(new ErrorResponse
                {
                    ErrorCode = "APPOINTMENT_NOT_FOUND",
                    Message = "Appointment not found"
                });
            }

            // Get the patient ID from FacilityPatient
            var patientId = appointment.FacilityPatient?.PatientId;
            if (!patientId.HasValue)
            {
                return BadRequest(new ErrorResponse
                {
                    ErrorCode = "PATIENT_NOT_FOUND",
                    Message = "Patient not associated with appointment"
                });
            }

            // Get medical records for the patient associated with this appointment
            var medicalRecords = await _context.MedicalRecords
                .Where(mr => mr.PatientId == patientId.Value)
                .OrderByDescending(mr => mr.CreatedAt)
                .Take(10)
                .ToListAsync();

            var history = new List<AppointmentMedicalHistoryResponse>();
            int index = 0;

            foreach (var record in medicalRecords)
            {
                history.Add(new AppointmentMedicalHistoryResponse
                {
                    Waktu = record.CreatedAt.ToString("dd MMM\nyyyy HH:mm"),
                    KodeICD10 = "J10.1",
                    NamaICD10 = "Influenza with other respiratory manifestations",
                    Diagnosa = record.Diagnosis ?? "No diagnosis recorded",
                    ObatObatan = "No data",
                    Status = index == 0 ? "Current" : (index + 1).ToString()
                });
                index++;
            }

            return Ok(history);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching appointment medical history");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching medical history"
            });
        }
    }
}
