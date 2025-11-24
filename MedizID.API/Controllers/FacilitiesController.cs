using MedizID.API.Common.Exceptions;
using MedizID.API.Common.Enums;
using MedizID.API.Data;
using MedizID.API.DTOs;
using MedizID.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedizID.API.Controllers;

[ApiController]
[Route("api/v1/facilities")]
[Produces("application/json")]
[Authorize]
public class FacilitiesController : ControllerBase
{
    private readonly MedizIDDbContext _context;
    private readonly ILogger<FacilitiesController> _logger;

    public FacilitiesController(MedizIDDbContext context, ILogger<FacilitiesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    #region Base Facility CRUD

    /// <summary>
    /// Create new facility
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(FacilityResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateFacility([FromBody] CreateFacilityRequest request)
    {
        try
        {
            var facility = new Facility
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Address = request.Address,
                City = request.City,
                Province = request.Province,
                PostalCode = request.PostalCode,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                Type = (FacilityTypeEnum)request.Type,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Facilities.Add(facility);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Facility created: {facility.Id}");

            var response = new FacilityResponse
            {
                Id = facility.Id,
                Name = facility.Name,
                Address = facility.Address,
                City = facility.City,
                Province = facility.Province,
                PostalCode = facility.PostalCode,
                PhoneNumber = facility.PhoneNumber,
                Email = facility.Email,
                Type = (int)facility.Type,
                IsActive = facility.IsActive,
                CreatedAt = facility.CreatedAt
            };

            return CreatedAtAction(nameof(GetFacility), new { facilityId = facility.Id }, response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating facility");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while creating the facility"
            });
        }
    }

    /// <summary>
    /// Get all facilities
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<FacilityResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFacilities([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var query = _context.Facilities.AsQueryable();

            var total = await query.CountAsync();

            var facilities = await query
                .OrderByDescending(f => f.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(f => new FacilityResponse
                {
                    Id = f.Id,
                    Name = f.Name,
                    Address = f.Address,
                    City = f.City,
                    Province = f.Province,
                    PostalCode = f.PostalCode,
                    PhoneNumber = f.PhoneNumber,
                    Email = f.Email,
                    Type = (int)f.Type,
                    IsActive = f.IsActive,
                    CreatedAt = f.CreatedAt
                })
                .ToListAsync();

            return Ok(new PagedResult<FacilityResponse>
            {
                Items = facilities,
                Total = total,
                Page = page,
                PageSize = pageSize
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching facilities");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching facilities"
            });
        }
    }

    /// <summary>
    /// Get facility by ID
    /// </summary>
    [HttpGet("{facilityId}")]
    [ProducesResponseType(typeof(FacilityResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetFacility(Guid facilityId)
    {
        try
        {
            var facility = await _context.Facilities.FirstOrDefaultAsync(f => f.Id == facilityId);
            if (facility == null)
                throw new NotFoundException($"Facility with ID {facilityId} not found");

            var response = new FacilityResponse
            {
                Id = facility.Id,
                Name = facility.Name,
                Address = facility.Address,
                City = facility.City,
                Province = facility.Province,
                PostalCode = facility.PostalCode,
                PhoneNumber = facility.PhoneNumber,
                Email = facility.Email,
                Type = (int)facility.Type,
                IsActive = facility.IsActive,
                CreatedAt = facility.CreatedAt
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
            _logger.LogError(ex, $"Error fetching facility {facilityId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching the facility"
            });
        }
    }

    /// <summary>
    /// Update facility
    /// </summary>
    [HttpPut("{facilityId}")]
    [ProducesResponseType(typeof(FacilityResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateFacility(Guid facilityId, [FromBody] UpdateFacilityRequest request)
    {
        try
        {
            var facility = await _context.Facilities.FirstOrDefaultAsync(f => f.Id == facilityId);
            if (facility == null)
                throw new NotFoundException($"Facility with ID {facilityId} not found");

            if (!string.IsNullOrEmpty(request.Name))
                facility.Name = request.Name;

            if (!string.IsNullOrEmpty(request.Address))
                facility.Address = request.Address;

            if (!string.IsNullOrEmpty(request.City))
                facility.City = request.City;

            if (!string.IsNullOrEmpty(request.Province))
                facility.Province = request.Province;

            if (!string.IsNullOrEmpty(request.PostalCode))
                facility.PostalCode = request.PostalCode;

            if (!string.IsNullOrEmpty(request.PhoneNumber))
                facility.PhoneNumber = request.PhoneNumber;

            if (!string.IsNullOrEmpty(request.Email))
                facility.Email = request.Email;

            if (request.IsActive.HasValue)
                facility.IsActive = request.IsActive.Value;

            facility.UpdatedAt = DateTime.UtcNow;

            _context.Facilities.Update(facility);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Facility updated: {facilityId}");

            var response = new FacilityResponse
            {
                Id = facility.Id,
                Name = facility.Name,
                Address = facility.Address,
                City = facility.City,
                Province = facility.Province,
                PostalCode = facility.PostalCode,
                PhoneNumber = facility.PhoneNumber,
                Email = facility.Email,
                Type = (int)facility.Type,
                IsActive = facility.IsActive,
                CreatedAt = facility.CreatedAt
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
            _logger.LogError(ex, $"Error updating facility {facilityId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while updating the facility"
            });
        }
    }

    /// <summary>
    /// Delete facility
    /// </summary>
    [HttpDelete("{facilityId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteFacility(Guid facilityId)
    {
        try
        {
            var facility = await _context.Facilities.FirstOrDefaultAsync(f => f.Id == facilityId);
            if (facility == null)
                throw new NotFoundException($"Facility with ID {facilityId} not found");

            _context.Facilities.Remove(facility);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Facility deleted: {facilityId}");

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
            _logger.LogError(ex, $"Error deleting facility {facilityId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while deleting the facility"
            });
        }
    }

    #endregion

    #region Appointments

    /// <summary>
    /// Get facility appointments
    /// </summary>
    [HttpGet("{facilityId}/appointments")]
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
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .OrderByDescending(a => a.AppointmentDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new AppointmentResponse
                {
                    Id = a.Id,
                    PatientId = a.PatientId,
                    PatientName = $"{a.Patient.FirstName} {a.Patient.LastName}",
                    DoctorId = a.DoctorId,
                    DoctorName = a.Doctor != null ? $"{a.Doctor.FirstName} {a.Doctor.LastName}" : null,
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
    [HttpPost("{facilityId}/appointments")]
    [ProducesResponseType(typeof(AppointmentResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateFacilityAppointment(Guid facilityId, [FromBody] CreateFacilityAppointmentRequest request)
    {
        try
        {
            var facility = await _context.Facilities.FirstOrDefaultAsync(f => f.Id == facilityId);
            if (facility == null)
                throw new NotFoundException($"Facility with ID {facilityId} not found");

            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Id == request.PatientId);
            if (patient == null)
                throw new NotFoundException($"Patient with ID {request.PatientId} not found");

            var appointment = new Appointment
            {
                Id = Guid.NewGuid(),
                FacilityId = facilityId,
                PatientId = request.PatientId,
                DoctorId = request.DoctorId,
                AppointmentDate = request.AppointmentDate,
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
                PatientId = appointment.PatientId,
                PatientName = $"{patient.FirstName} {patient.LastName}",
                DoctorId = appointment.DoctorId,
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

    #endregion

    #region Departments

    /// <summary>
    /// Create department for facility
    /// </summary>
    [HttpPost("{facilityId}/departments")]
    [ProducesResponseType(typeof(DepartmentResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateDepartmentForFacility(Guid facilityId, [FromBody] CreateDepartmentRequest request)
    {
        try
        {
            var facility = await _context.Facilities.FirstOrDefaultAsync(f => f.Id == facilityId);
            if (facility == null)
                throw new NotFoundException($"Facility with ID {facilityId} not found");

            var department = new Department
            {
                Id = Guid.NewGuid(),
                FacilityId = facilityId,
                Name = request.Name,
                Description = request.Description,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Departments.Add(department);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Department created for facility: {department.Id}");

            var response = new DepartmentResponse
            {
                Id = department.Id,
                FacilityId = department.FacilityId,
                Name = department.Name,
                Description = department.Description,
                IsActive = department.IsActive,
                CreatedAt = department.CreatedAt
            };

            return CreatedAtAction(nameof(ListDepartments), new { facilityId }, response);
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
            _logger.LogError(ex, "Error creating department");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while creating the department"
            });
        }
    }

    /// <summary>
    /// List departments for facility
    /// </summary>
    [HttpGet("{facilityId}/departments")]
    [ProducesResponseType(typeof(PagedResult<DepartmentResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ListDepartments(Guid facilityId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var facility = await _context.Facilities.FirstOrDefaultAsync(f => f.Id == facilityId);
            if (facility == null)
                throw new NotFoundException($"Facility with ID {facilityId} not found");

            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var query = _context.Departments
                .Where(d => d.FacilityId == facilityId)
                .AsQueryable();

            var total = await query.CountAsync();

            var departments = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(d => new DepartmentResponse
                {
                    Id = d.Id,
                    FacilityId = d.FacilityId,
                    Name = d.Name,
                    Description = d.Description,
                    IsActive = d.IsActive,
                    CreatedAt = d.CreatedAt
                })
                .ToListAsync();

            return Ok(new PagedResult<DepartmentResponse>
            {
                Items = departments,
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
            _logger.LogError(ex, $"Error fetching departments {facilityId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching departments"
            });
        }
    }

    /// <summary>
    /// Update department
    /// </summary>
    [HttpPut("{facilityId}/departments/{departmentId}")]
    [ProducesResponseType(typeof(DepartmentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateDepartment(Guid facilityId, Guid departmentId, [FromBody] UpdateDepartmentRequest request)
    {
        try
        {
            var facility = await _context.Facilities.FirstOrDefaultAsync(f => f.Id == facilityId);
            if (facility == null)
                throw new NotFoundException($"Facility with ID {facilityId} not found");

            var department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == departmentId && d.FacilityId == facilityId);
            if (department == null)
                throw new NotFoundException($"Department with ID {departmentId} not found");

            if (!string.IsNullOrEmpty(request.Name))
                department.Name = request.Name;

            if (!string.IsNullOrEmpty(request.Description))
                department.Description = request.Description;

            if (request.IsActive.HasValue)
                department.IsActive = request.IsActive.Value;

            _context.Departments.Update(department);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Department updated: {departmentId}");

            var response = new DepartmentResponse
            {
                Id = department.Id,
                FacilityId = department.FacilityId,
                Name = department.Name,
                Description = department.Description,
                IsActive = department.IsActive,
                CreatedAt = department.CreatedAt
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
            _logger.LogError(ex, $"Error updating department {departmentId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while updating the department"
            });
        }
    }

    #endregion

    #region Installations

    /// <summary>
    /// Create installation
    /// </summary>
    [HttpPost("{facilityId}/installations")]
    [ProducesResponseType(typeof(InstallationResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateInstallation(Guid facilityId, [FromBody] CreateInstallationRequest request)
    {
        try
        {
            var facility = await _context.Facilities.FirstOrDefaultAsync(f => f.Id == facilityId);
            if (facility == null)
                throw new NotFoundException($"Facility with ID {facilityId} not found");

            var installation = new Installation
            {
                Id = Guid.NewGuid(),
                FacilityId = facilityId,
                Name = request.Name,
                Type = request.Type,
                Location = request.Location,
                Condition = request.Condition,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Installations.Add(installation);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Installation created: {installation.Id}");

            var response = new InstallationResponse
            {
                Id = installation.Id,
                FacilityId = installation.FacilityId,
                Name = installation.Name,
                Type = installation.Type,
                Location = installation.Location,
                Condition = installation.Condition,
                LastMaintenanceDate = installation.LastMaintenanceDate,
                IsActive = installation.IsActive,
                CreatedAt = installation.CreatedAt,
                UpdatedAt = installation.UpdatedAt
            };

            return CreatedAtAction(nameof(GetInstallation), new { facilityId, installationId = installation.Id }, response);
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
            _logger.LogError(ex, "Error creating installation");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while creating the installation"
            });
        }
    }

    /// <summary>
    /// Get facility installations
    /// </summary>
    [HttpGet("{facilityId}/installations")]
    [ProducesResponseType(typeof(PagedResult<InstallationResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetFacilityInstallations(Guid facilityId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var facility = await _context.Facilities.FirstOrDefaultAsync(f => f.Id == facilityId);
            if (facility == null)
                throw new NotFoundException($"Facility with ID {facilityId} not found");

            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var query = _context.Installations
                .Where(i => i.FacilityId == facilityId)
                .AsQueryable();

            var total = await query.CountAsync();

            var installations = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(i => new InstallationResponse
                {
                    Id = i.Id,
                    FacilityId = i.FacilityId,
                    Name = i.Name,
                    Type = i.Type,
                    Location = i.Location,
                    Condition = i.Condition,
                    LastMaintenanceDate = i.LastMaintenanceDate,
                    IsActive = i.IsActive,
                    CreatedAt = i.CreatedAt,
                    UpdatedAt = i.UpdatedAt
                })
                .ToListAsync();

            return Ok(new PagedResult<InstallationResponse>
            {
                Items = installations,
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
            _logger.LogError(ex, $"Error fetching facility installations {facilityId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching installations"
            });
        }
    }

    /// <summary>
    /// Get installation by ID
    /// </summary>
    [HttpGet("{facilityId}/installations/{installationId}")]
    [ProducesResponseType(typeof(InstallationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetInstallation(Guid facilityId, Guid installationId)
    {
        try
        {
            var facility = await _context.Facilities.FirstOrDefaultAsync(f => f.Id == facilityId);
            if (facility == null)
                throw new NotFoundException($"Facility with ID {facilityId} not found");

            var installation = await _context.Installations.FirstOrDefaultAsync(i => i.Id == installationId && i.FacilityId == facilityId);
            if (installation == null)
                throw new NotFoundException($"Installation with ID {installationId} not found");

            var response = new InstallationResponse
            {
                Id = installation.Id,
                FacilityId = installation.FacilityId,
                Name = installation.Name,
                Type = installation.Type,
                Location = installation.Location,
                Condition = installation.Condition,
                LastMaintenanceDate = installation.LastMaintenanceDate,
                IsActive = installation.IsActive,
                CreatedAt = installation.CreatedAt,
                UpdatedAt = installation.UpdatedAt
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
            _logger.LogError(ex, $"Error fetching installation {installationId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching the installation"
            });
        }
    }

    /// <summary>
    /// Update installation
    /// </summary>
    [HttpPut("{facilityId}/installations/{installationId}")]
    [ProducesResponseType(typeof(InstallationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateInstallation(Guid facilityId, Guid installationId, [FromBody] UpdateInstallationRequest request)
    {
        try
        {
            var facility = await _context.Facilities.FirstOrDefaultAsync(f => f.Id == facilityId);
            if (facility == null)
                throw new NotFoundException($"Facility with ID {facilityId} not found");

            var installation = await _context.Installations.FirstOrDefaultAsync(i => i.Id == installationId && i.FacilityId == facilityId);
            if (installation == null)
                throw new NotFoundException($"Installation with ID {installationId} not found");

            if (!string.IsNullOrEmpty(request.Name))
                installation.Name = request.Name;

            if (!string.IsNullOrEmpty(request.Type))
                installation.Type = request.Type;

            if (!string.IsNullOrEmpty(request.Location))
                installation.Location = request.Location;

            if (!string.IsNullOrEmpty(request.Condition))
                installation.Condition = request.Condition;

            if (request.LastMaintenanceDate.HasValue)
                installation.LastMaintenanceDate = request.LastMaintenanceDate;

            if (request.IsActive.HasValue)
                installation.IsActive = request.IsActive.Value;

            installation.UpdatedAt = DateTime.UtcNow;

            _context.Installations.Update(installation);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Installation updated: {installationId}");

            var response = new InstallationResponse
            {
                Id = installation.Id,
                FacilityId = installation.FacilityId,
                Name = installation.Name,
                Type = installation.Type,
                Location = installation.Location,
                Condition = installation.Condition,
                LastMaintenanceDate = installation.LastMaintenanceDate,
                IsActive = installation.IsActive,
                CreatedAt = installation.CreatedAt,
                UpdatedAt = installation.UpdatedAt
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
            _logger.LogError(ex, $"Error updating installation {installationId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while updating the installation"
            });
        }
    }

    /// <summary>
    /// Delete installation
    /// </summary>
    [HttpDelete("{facilityId}/installations/{installationId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteInstallation(Guid facilityId, Guid installationId)
    {
        try
        {
            var facility = await _context.Facilities.FirstOrDefaultAsync(f => f.Id == facilityId);
            if (facility == null)
                throw new NotFoundException($"Facility with ID {facilityId} not found");

            var installation = await _context.Installations.FirstOrDefaultAsync(i => i.Id == installationId && i.FacilityId == facilityId);
            if (installation == null)
                throw new NotFoundException($"Installation with ID {installationId} not found");

            _context.Installations.Remove(installation);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Installation deleted: {installationId}");

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
            _logger.LogError(ex, $"Error deleting installation {installationId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while deleting the installation"
            });
        }
    }

    #endregion

    #region Patients

    /// <summary>
    /// Create facility patient
    /// </summary>
    [HttpPost("{facilityId}/patients")]
    [ProducesResponseType(typeof(FacilityPatientResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateFacilityPatient(Guid facilityId, [FromBody] CreateFacilityPatientRequest request)
    {
        try
        {
            var facility = await _context.Facilities.FirstOrDefaultAsync(f => f.Id == facilityId);
            if (facility == null)
                throw new NotFoundException($"Facility with ID {facilityId} not found");

            var patient = new Patient
            {
                Id = Guid.NewGuid(),
                FacilityId = facilityId,
                MedicalRecordNumber = request.MedicalRecordNumber,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                DateOfBirth = request.DateOfBirth,
                Gender = request.Gender,
                BloodType = request.BloodType,
                Address = request.Address,
                City = request.City,
                CreatedAt = DateTime.UtcNow
            };

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Facility patient created: {patient.Id}");

            var response = new FacilityPatientResponse
            {
                Id = patient.Id,
                FacilityId = patient.FacilityId,
                MedicalRecordNumber = patient.MedicalRecordNumber,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                PhoneNumber = patient.PhoneNumber,
                Email = patient.Email,
                DateOfBirth = patient.DateOfBirth,
                Gender = patient.Gender,
                BloodType = patient.BloodType,
                Address = patient.Address,
                City = patient.City,
                CreatedAt = patient.CreatedAt,
                UpdatedAt = patient.UpdatedAt
            };

            return CreatedAtAction(nameof(GetFacilityPatients), new { facilityId }, response);
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
            _logger.LogError(ex, "Error creating facility patient");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while creating the patient"
            });
        }
    }

    /// <summary>
    /// Get facility patients
    /// </summary>
    [HttpGet("{facilityId}/patients")]
    [ProducesResponseType(typeof(PagedResult<FacilityPatientResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetFacilityPatients(Guid facilityId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var facility = await _context.Facilities.FirstOrDefaultAsync(f => f.Id == facilityId);
            if (facility == null)
                throw new NotFoundException($"Facility with ID {facilityId} not found");

            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var query = _context.Patients
                .Where(p => p.FacilityId == facilityId)
                .AsQueryable();

            var total = await query.CountAsync();

            var patients = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new FacilityPatientResponse
                {
                    Id = p.Id,
                    FacilityId = p.FacilityId,
                    MedicalRecordNumber = p.MedicalRecordNumber,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    PhoneNumber = p.PhoneNumber,
                    Email = p.Email,
                    DateOfBirth = p.DateOfBirth,
                    Gender = p.Gender,
                    BloodType = p.BloodType,
                    Address = p.Address,
                    City = p.City,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                })
                .ToListAsync();

            return Ok(new PagedResult<FacilityPatientResponse>
            {
                Items = patients,
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
            _logger.LogError(ex, $"Error fetching facility patients {facilityId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching patients"
            });
        }
    }

    /// <summary>
    /// Get facility patient details
    /// </summary>
    [HttpGet("{facilityId}/patients/{patientId}/details")]
    [ProducesResponseType(typeof(FacilityPatientDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetFacilityPatientDetails(Guid facilityId, Guid patientId)
    {
        try
        {
            var facility = await _context.Facilities.FirstOrDefaultAsync(f => f.Id == facilityId);
            if (facility == null)
                throw new NotFoundException($"Facility with ID {facilityId} not found");

            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Id == patientId && p.FacilityId == facilityId);
            if (patient == null)
                throw new NotFoundException($"Patient with ID {patientId} not found");

            var totalAppointments = await _context.Appointments
                .CountAsync(a => a.PatientId == patientId);

            var totalMedicalRecords = await _context.MedicalRecords
                .CountAsync(m => m.PatientId == patientId);

            var lastVisitDate = await _context.MedicalRecords
                .Where(m => m.PatientId == patientId)
                .OrderByDescending(m => m.VisitDate)
                .Select(m => m.VisitDate)
                .FirstOrDefaultAsync();

            var response = new FacilityPatientDetailResponse
            {
                Id = patient.Id,
                FacilityId = patient.FacilityId,
                MedicalRecordNumber = patient.MedicalRecordNumber,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                PhoneNumber = patient.PhoneNumber,
                Email = patient.Email,
                DateOfBirth = patient.DateOfBirth,
                Gender = patient.Gender,
                BloodType = patient.BloodType,
                Address = patient.Address,
                City = patient.City,
                TotalAppointments = totalAppointments,
                TotalMedicalRecords = totalMedicalRecords,
                LastVisitDate = lastVisitDate != default ? lastVisitDate : null,
                CreatedAt = patient.CreatedAt,
                UpdatedAt = patient.UpdatedAt
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
            _logger.LogError(ex, $"Error fetching facility patient details {patientId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching patient details"
            });
        }
    }

    #endregion

    #region Staff

    /// <summary>
    /// Add staff to facility
    /// </summary>
    [HttpPost("{facilityId}/staff")]
    [ProducesResponseType(typeof(FacilityStaffResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddStaffToFacility(Guid facilityId, [FromBody] AddStaffToFacilityRequest request)
    {
        try
        {
            var facility = await _context.Facilities.FirstOrDefaultAsync(f => f.Id == facilityId);
            if (facility == null)
                throw new NotFoundException($"Facility with ID {facilityId} not found");

            var staff = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.StaffId);
            if (staff == null)
                throw new NotFoundException($"Staff with ID {request.StaffId} not found");

            var facilityStaff = new FacilityStaff
            {
                Id = Guid.NewGuid(),
                FacilityId = facilityId,
                StaffId = request.StaffId,
                DepartmentId = request.DepartmentId,
                Position = request.Position,
                Specialization = request.Specialization,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.FacilityStaffs.Add(facilityStaff);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Staff added to facility: {facilityStaff.Id}");

            var response = new FacilityStaffResponse
            {
                Id = facilityStaff.Id,
                FacilityId = facilityStaff.FacilityId,
                StaffId = facilityStaff.StaffId,
                DepartmentId = facilityStaff.DepartmentId,
                Position = facilityStaff.Position,
                Specialization = facilityStaff.Specialization,
                StartDate = facilityStaff.StartDate,
                EndDate = facilityStaff.EndDate,
                IsActive = facilityStaff.IsActive,
                CreatedAt = facilityStaff.CreatedAt,
                UpdatedAt = facilityStaff.UpdatedAt
            };

            return CreatedAtAction(nameof(ListStaff), new { facilityId }, response);
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
            _logger.LogError(ex, "Error adding staff to facility");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while adding staff"
            });
        }
    }

    /// <summary>
    /// List staff for facility
    /// </summary>
    [HttpGet("{facilityId}/staff")]
    [ProducesResponseType(typeof(PagedResult<FacilityStaffResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ListStaff(Guid facilityId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var facility = await _context.Facilities.FirstOrDefaultAsync(f => f.Id == facilityId);
            if (facility == null)
                throw new NotFoundException($"Facility with ID {facilityId} not found");

            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var query = _context.FacilityStaffs
                .Where(fs => fs.FacilityId == facilityId && fs.IsActive)
                .AsQueryable();

            var total = await query.CountAsync();

            var staff = await query
                .Include(fs => fs.Staff)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(fs => new FacilityStaffResponse
                {
                    Id = fs.Id,
                    FacilityId = fs.FacilityId,
                    StaffId = fs.StaffId,
                    DepartmentId = fs.DepartmentId,
                    Position = fs.Position,
                    Specialization = fs.Specialization,
                    StartDate = fs.StartDate,
                    EndDate = fs.EndDate,
                    IsActive = fs.IsActive,
                    CreatedAt = fs.CreatedAt,
                    UpdatedAt = fs.UpdatedAt
                })
                .ToListAsync();

            return Ok(new PagedResult<FacilityStaffResponse>
            {
                Items = staff,
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
            _logger.LogError(ex, $"Error fetching facility staff {facilityId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching staff"
            });
        }
    }

    /// <summary>
    /// Update facility staff
    /// </summary>
    [HttpPut("{facilityId}/staff/{staffId}")]
    [ProducesResponseType(typeof(FacilityStaffResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateFacilityStaff(Guid facilityId, Guid staffId, [FromBody] UpdateFacilityStaffRequest request)
    {
        try
        {
            var facility = await _context.Facilities.FirstOrDefaultAsync(f => f.Id == facilityId);
            if (facility == null)
                throw new NotFoundException($"Facility with ID {facilityId} not found");

            var facilityStaff = await _context.FacilityStaffs.FirstOrDefaultAsync(fs => fs.Id == staffId && fs.FacilityId == facilityId);
            if (facilityStaff == null)
                throw new NotFoundException($"Staff with ID {staffId} not found");

            if (request.DepartmentId.HasValue)
                facilityStaff.DepartmentId = request.DepartmentId;

            if (!string.IsNullOrEmpty(request.Position))
                facilityStaff.Position = request.Position;

            if (!string.IsNullOrEmpty(request.Specialization))
                facilityStaff.Specialization = request.Specialization;

            if (request.IsActive.HasValue)
                facilityStaff.IsActive = request.IsActive.Value;

            facilityStaff.UpdatedAt = DateTime.UtcNow;

            _context.FacilityStaffs.Update(facilityStaff);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Facility staff updated: {staffId}");

            var response = new FacilityStaffResponse
            {
                Id = facilityStaff.Id,
                FacilityId = facilityStaff.FacilityId,
                StaffId = facilityStaff.StaffId,
                DepartmentId = facilityStaff.DepartmentId,
                Position = facilityStaff.Position,
                Specialization = facilityStaff.Specialization,
                StartDate = facilityStaff.StartDate,
                EndDate = facilityStaff.EndDate,
                IsActive = facilityStaff.IsActive,
                CreatedAt = facilityStaff.CreatedAt,
                UpdatedAt = facilityStaff.UpdatedAt
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
            _logger.LogError(ex, $"Error updating facility staff {staffId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while updating staff"
            });
        }
    }

    /// <summary>
    /// Remove staff from facility
    /// </summary>
    [HttpDelete("{facilityId}/staff/{staffId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveStaffFromFacility(Guid facilityId, Guid staffId)
    {
        try
        {
            var facility = await _context.Facilities.FirstOrDefaultAsync(f => f.Id == facilityId);
            if (facility == null)
                throw new NotFoundException($"Facility with ID {facilityId} not found");

            var facilityStaff = await _context.FacilityStaffs.FirstOrDefaultAsync(fs => fs.Id == staffId && fs.FacilityId == facilityId);
            if (facilityStaff == null)
                throw new NotFoundException($"Staff with ID {staffId} not found");

            _context.FacilityStaffs.Remove(facilityStaff);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Staff removed from facility: {staffId}");

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
            _logger.LogError(ex, $"Error removing staff from facility {staffId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while removing staff"
            });
        }
    }

    #endregion

    #region Time Slots

    /// <summary>
    /// Create poli time slot
    /// </summary>
    [HttpPost("{facilityId}/time-slots")]
    [ProducesResponseType(typeof(PoliTimeSlotResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreatePoliTimeSlot(Guid facilityId, [FromQuery] Guid poliId, [FromBody] CreatePoliTimeSlotRequest request)
    {
        try
        {
            var poli = await _context.Polis.FirstOrDefaultAsync(p => p.Id == poliId && p.FacilityId == facilityId);
            if (poli == null)
                throw new NotFoundException($"Poli with ID {poliId} not found");

            var timeSlot = new PoliTimeSlot
            {
                Id = Guid.NewGuid(),
                PoliId = poliId,
                StaffId = request.StaffId,
                DayOfWeek = (DayOfWeek)request.DayOfWeek,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                SlotDuration = request.SlotDuration,
                MaxPatients = request.MaxPatients,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.PoliTimeSlots.Add(timeSlot);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Time slot created: {timeSlot.Id}");

            var response = new PoliTimeSlotResponse
            {
                Id = timeSlot.Id,
                PoliId = timeSlot.PoliId,
                StaffId = timeSlot.StaffId,
                DayOfWeek = (int)timeSlot.DayOfWeek,
                StartTime = timeSlot.StartTime,
                EndTime = timeSlot.EndTime,
                SlotDuration = timeSlot.SlotDuration,
                MaxPatients = timeSlot.MaxPatients,
                IsActive = timeSlot.IsActive,
                CreatedAt = timeSlot.CreatedAt,
                UpdatedAt = timeSlot.UpdatedAt
            };

            return CreatedAtAction(nameof(GetPoliTimeSlot), new { facilityId, poliId, slotId = timeSlot.Id }, response);
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
            _logger.LogError(ex, "Error creating time slot");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while creating the time slot"
            });
        }
    }

    /// <summary>
    /// Get poli time slots
    /// </summary>
    [HttpGet("{facilityId}/time-slots")]
    [ProducesResponseType(typeof(PagedResult<PoliTimeSlotResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPoliTimeSlots(Guid facilityId, [FromQuery] Guid poliId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var poli = await _context.Polis.FirstOrDefaultAsync(p => p.Id == poliId && p.FacilityId == facilityId);
            if (poli == null)
                throw new NotFoundException($"Poli with ID {poliId} not found");

            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var query = _context.PoliTimeSlots
                .Where(ts => ts.PoliId == poliId)
                .AsQueryable();

            var total = await query.CountAsync();

            var slots = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(ts => new PoliTimeSlotResponse
                {
                    Id = ts.Id,
                    PoliId = ts.PoliId,
                    StaffId = ts.StaffId,
                    DayOfWeek = (int)ts.DayOfWeek,
                    StartTime = ts.StartTime,
                    EndTime = ts.EndTime,
                    SlotDuration = ts.SlotDuration,
                    MaxPatients = ts.MaxPatients,
                    IsActive = ts.IsActive,
                    CreatedAt = ts.CreatedAt,
                    UpdatedAt = ts.UpdatedAt
                })
                .ToListAsync();

            return Ok(new PagedResult<PoliTimeSlotResponse>
            {
                Items = slots,
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
            _logger.LogError(ex, $"Error fetching time slots for poli {poliId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching time slots"
            });
        }
    }

    /// <summary>
    /// Get poli time slot by ID
    /// </summary>
    [HttpGet("{facilityId}/time-slots/{slotId}")]
    [ProducesResponseType(typeof(PoliTimeSlotResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPoliTimeSlot(Guid facilityId, Guid slotId, [FromQuery] Guid poliId)
    {
        try
        {
            var poli = await _context.Polis.FirstOrDefaultAsync(p => p.Id == poliId && p.FacilityId == facilityId);
            if (poli == null)
                throw new NotFoundException($"Poli with ID {poliId} not found");

            var slot = await _context.PoliTimeSlots.FirstOrDefaultAsync(ts => ts.Id == slotId && ts.PoliId == poliId);
            if (slot == null)
                throw new NotFoundException($"Time slot with ID {slotId} not found");

            var response = new PoliTimeSlotResponse
            {
                Id = slot.Id,
                PoliId = slot.PoliId,
                StaffId = slot.StaffId,
                DayOfWeek = (int)slot.DayOfWeek,
                StartTime = slot.StartTime,
                EndTime = slot.EndTime,
                SlotDuration = slot.SlotDuration,
                MaxPatients = slot.MaxPatients,
                IsActive = slot.IsActive,
                CreatedAt = slot.CreatedAt,
                UpdatedAt = slot.UpdatedAt
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
            _logger.LogError(ex, $"Error fetching time slot {slotId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching the time slot"
            });
        }
    }

    /// <summary>
    /// Update poli time slot
    /// </summary>
    [HttpPut("{facilityId}/time-slots/{slotId}")]
    [ProducesResponseType(typeof(PoliTimeSlotResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdatePoliTimeSlot(Guid facilityId, Guid slotId, [FromQuery] Guid poliId, [FromBody] UpdatePoliTimeSlotRequest request)
    {
        try
        {
            var poli = await _context.Polis.FirstOrDefaultAsync(p => p.Id == poliId && p.FacilityId == facilityId);
            if (poli == null)
                throw new NotFoundException($"Poli with ID {poliId} not found");

            var slot = await _context.PoliTimeSlots.FirstOrDefaultAsync(ts => ts.Id == slotId && ts.PoliId == poliId);
            if (slot == null)
                throw new NotFoundException($"Time slot with ID {slotId} not found");

            if (request.StaffId.HasValue)
                slot.StaffId = request.StaffId;

            if (request.DayOfWeek.HasValue)
                slot.DayOfWeek = (DayOfWeek)request.DayOfWeek.Value;

            if (request.StartTime.HasValue)
                slot.StartTime = request.StartTime.Value;

            if (request.EndTime.HasValue)
                slot.EndTime = request.EndTime.Value;

            if (request.SlotDuration.HasValue)
                slot.SlotDuration = request.SlotDuration.Value;

            if (request.MaxPatients.HasValue)
                slot.MaxPatients = request.MaxPatients.Value;

            if (request.IsActive.HasValue)
                slot.IsActive = request.IsActive.Value;

            slot.UpdatedAt = DateTime.UtcNow;

            _context.PoliTimeSlots.Update(slot);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Time slot updated: {slotId}");

            var response = new PoliTimeSlotResponse
            {
                Id = slot.Id,
                PoliId = slot.PoliId,
                StaffId = slot.StaffId,
                DayOfWeek = (int)slot.DayOfWeek,
                StartTime = slot.StartTime,
                EndTime = slot.EndTime,
                SlotDuration = slot.SlotDuration,
                MaxPatients = slot.MaxPatients,
                IsActive = slot.IsActive,
                CreatedAt = slot.CreatedAt,
                UpdatedAt = slot.UpdatedAt
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
            _logger.LogError(ex, $"Error updating time slot {slotId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while updating the time slot"
            });
        }
    }

    /// <summary>
    /// Delete poli time slot
    /// </summary>
    [HttpDelete("{facilityId}/time-slots/{slotId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeletePoliTimeSlot(Guid facilityId, Guid slotId, [FromQuery] Guid poliId)
    {
        try
        {
            var poli = await _context.Polis.FirstOrDefaultAsync(p => p.Id == poliId && p.FacilityId == facilityId);
            if (poli == null)
                throw new NotFoundException($"Poli with ID {poliId} not found");

            var slot = await _context.PoliTimeSlots.FirstOrDefaultAsync(ts => ts.Id == slotId && ts.PoliId == poliId);
            if (slot == null)
                throw new NotFoundException($"Time slot with ID {slotId} not found");

            _context.PoliTimeSlots.Remove(slot);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Time slot deleted: {slotId}");

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
            _logger.LogError(ex, $"Error deleting time slot {slotId}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while deleting the time slot"
            });
        }
    }

    #endregion
}
