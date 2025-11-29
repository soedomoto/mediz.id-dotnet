using MedizID.API.Common.Exceptions;
using MedizID.API.Common.Enums;
using MedizID.API.Data;
using MedizID.API.DTOs;
using MedizID.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedizID.API.Controllers.FacilityManagement;

[ApiController]
[Route("api/v1/facilities/{facilityId}/appointments")]
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
    /// Get facility appointments
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<AppointmentResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetFacilityAppointments(Guid facilityId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var facility = await _context.Facilities.FirstOrDefaultAsync(f => f.Id == facilityId);
            if (facility == null)
                throw new NotFoundException($"Facility with ID {facilityId} not found");

            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var query = _context.Appointments
                .Where(a => a.FacilityId == facilityId)
                .AsQueryable();

            var total = await query.CountAsync();

            var appointments = await query
                .Include(a => a.FacilityPatient)
                .ThenInclude(fp => fp.Patient)
                .Include(a => a.FacilityDoctor)
                .ThenInclude(fs => fs.Staff)
                .OrderByDescending(a => a.AppointmentDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new AppointmentResponse
                {
                    Id = a.Id,
                    FacilityPatientId = a.FacilityPatientId,
                    PatientName = $"{a.FacilityPatient.Patient.FirstName} {a.FacilityPatient.Patient.LastName}",
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
            _logger.LogError(ex, $"Error fetching facility appointments {facilityId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching appointments"
            });
        }
    }

    /// <summary>
    /// Create facility appointment
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(AppointmentResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateFacilityAppointment(Guid facilityId, [FromBody] CreateFacilityAppointmentRequest request)
    {
        try
        {
            var facility = await _context.Facilities.FirstOrDefaultAsync(f => f.Id == facilityId);
            if (facility == null)
                throw new NotFoundException($"Facility with ID {facilityId} not found");

            var facilityPatient = await _context.FacilityPatients
                .Include(fp => fp.Patient)
                .FirstOrDefaultAsync(fp => fp.Id == request.FacilityPatientId && fp.FacilityId == facilityId);
            if (facilityPatient == null)
                throw new NotFoundException($"Facility patient with ID {request.FacilityPatientId} not found in this facility");

            FacilityStaff? facilityDoctor = null;
            if (request.FacilityDoctorId.HasValue)
            {
                facilityDoctor = await _context.FacilityStaffs
                    .Include(fs => fs.Staff)
                    .FirstOrDefaultAsync(fs => fs.Id == request.FacilityDoctorId && fs.FacilityId == facilityId && fs.Role == UserRoleEnum.Doctor);
                if (facilityDoctor == null)
                    throw new NotFoundException($"Doctor staff with ID {request.FacilityDoctorId} not found in this facility");
            }

            var appointment = new Appointment
            {
                Id = Guid.NewGuid(),
                FacilityId = facilityId,
                FacilityPatientId = request.FacilityPatientId,
                FacilityDoctorId = request.FacilityDoctorId,
                AppointmentDate = request.AppointmentDate.Kind == DateTimeKind.Local 
                    ? request.AppointmentDate.ToUniversalTime() 
                    : request.AppointmentDate,
                AppointmentTime = request.AppointmentTime,
                Reason = request.Reason,
                Notes = request.Notes,
                CreatedAt = DateTime.UtcNow
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Facility appointment created: {appointment.Id}");

            var response = new AppointmentResponse
            {
                Id = appointment.Id,
                FacilityPatientId = appointment.FacilityPatientId,
                PatientName = $"{facilityPatient.Patient.FirstName} {facilityPatient.Patient.LastName}",
                FacilityDoctorId = appointment.FacilityDoctorId,
                DoctorName = facilityDoctor != null ? $"{facilityDoctor.Staff.FirstName} {facilityDoctor.Staff.LastName}" : null,
                AppointmentDate = appointment.AppointmentDate,
                AppointmentTime = appointment.AppointmentTime,
                Status = appointment.Status.ToString(),
                Reason = appointment.Reason,
                CreatedAt = appointment.CreatedAt
            };

            return CreatedAtAction(nameof(GetFacilityAppointments), new { facilityId }, response);
        }
        catch (NotFoundException ex)
        {
            return BadRequest(new ErrorResponse
            {
                ErrorCode = ex.ErrorCode,
                Message = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating facility appointment");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while creating the appointment"
            });
        }
    }
}
