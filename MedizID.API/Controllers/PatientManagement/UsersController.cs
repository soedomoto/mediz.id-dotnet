using MedizID.API.Common.Exceptions;
using MedizID.API.Data;
using MedizID.API.DTOs;
using MedizID.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedizID.API.Controllers.PatientManagement;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly MedizIDDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<UsersController> _logger;

    public UsersController(MedizIDDbContext context, UserManager<ApplicationUser> userManager, ILogger<UsersController> logger)
    {
        _context = context;
        _userManager = userManager;
        _logger = logger;
    }

    /// <summary>
    /// Get all users with pagination
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<UserResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var query = _context.Users.Where(u => u.IsActive).AsQueryable();
            var total = await query.CountAsync();

            var users = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new UserResponse
                {
                    Id = u.Id,
                    Email = u.Email ?? string.Empty,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Role = u.Role.ToString(),
                    PhoneNumber = u.PhoneNumber,
                    IsActive = u.IsActive,
                    CreatedAt = u.CreatedAt
                })
                .ToListAsync();

            return Ok(new PagedResult<UserResponse>
            {
                Items = users,
                Total = total,
                Page = page,
                PageSize = pageSize
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching users");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching users"
            });
        }
    }

    /// <summary>
    /// Search users by keyword (matches email, ID, first name, last name)
    /// </summary>
    [HttpGet("search")]
    [ProducesResponseType(typeof(PagedResult<UserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SearchUsers(
        [FromQuery] string keyword,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(keyword))
                throw new ArgumentException("Keyword parameter is required");

            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            // Normalize keyword for case-insensitive search
            var normalizedKeyword = keyword.ToLower().Trim();

            var query = _context.Users
                .Where(u => u.IsActive &&
                    (u.Id.ToString().ToLower().Contains(normalizedKeyword) ||
                     (u.Email != null && u.Email.ToLower().Contains(normalizedKeyword)) ||
                     u.FirstName.ToLower().Contains(normalizedKeyword) ||
                     u.LastName.ToLower().Contains(normalizedKeyword)))
                .AsQueryable();

            var total = await query.CountAsync();

            var users = await query
                .OrderBy(u => u.FirstName)
                .ThenBy(u => u.LastName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new UserResponse
                {
                    Id = u.Id,
                    Email = u.Email ?? string.Empty,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Role = u.Role.ToString(),
                    PhoneNumber = u.PhoneNumber,
                    IsActive = u.IsActive,
                    CreatedAt = u.CreatedAt
                })
                .ToListAsync();

            return Ok(new PagedResult<UserResponse>
            {
                Items = users,
                Total = total,
                Page = page,
                PageSize = pageSize
            });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid search request");
            return BadRequest(new ErrorResponse
            {
                ErrorCode = "INVALID_REQUEST",
                Message = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching users");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while searching for users"
            });
        }
    }

    /// <summary>
    /// Search user by email
    /// </summary>
    [HttpGet("search/email")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SearchUserByEmail([FromQuery] string email)
    {
        try
        {
            if (string.IsNullOrEmpty(email))
                throw new NotFoundException("Email parameter is required");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                throw new NotFoundException($"User with email {email} not found");

            var response = new UserResponse
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role.ToString(),
                PhoneNumber = user.PhoneNumber,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
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
            _logger.LogError(ex, "Error searching user by email");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while searching for user"
            });
        }
    }

    /// <summary>
    /// Create user by email
    /// </summary>
    [HttpPost("create-by-email")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUserByEmail([FromBody] CreateUserRequest request)
    {
        try
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (existingUser != null)
                throw new Exception("User with this email already exists");

            var user = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                UserName = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
                throw new Exception($"Failed to create user: {string.Join(", ", result.Errors.Select(e => e.Description))}");

            _logger.LogInformation($"User created by email: {user.Id}");

            var response = new UserResponse
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role.ToString(),
                PhoneNumber = user.PhoneNumber,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            };

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user by email");
            return BadRequest(new ErrorResponse
            {
                ErrorCode = "CREATION_FAILED",
                Message = ex.Message
            });
        }
    }

    /// <summary>
    /// Get user by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UserDetail), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUser(Guid id)
    {
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                throw new NotFoundException($"User with ID {id} not found");

            var response = new UserDetail
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role.ToString(),
                PhoneNumber = user.PhoneNumber,
                FacilityId = user.FacilityId,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
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
            _logger.LogError(ex, $"Error fetching user {id}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching the user"
            });
        }
    }

    /// <summary>
    /// Update user
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(UserDetail), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserRequest request)
    {
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                throw new NotFoundException($"User with ID {id} not found");

            if (!string.IsNullOrEmpty(request.FirstName))
                user.FirstName = request.FirstName;

            if (!string.IsNullOrEmpty(request.LastName))
                user.LastName = request.LastName;

            if (!string.IsNullOrEmpty(request.Email))
                user.Email = request.Email;

            if (request.FacilityId.HasValue)
            {
                var facility = await _context.Facilities.FirstOrDefaultAsync(f => f.Id == request.FacilityId);
                if (facility != null)
                    user.FacilityId = request.FacilityId;
            }

            if (request.IsActive.HasValue)
                user.IsActive = request.IsActive.Value;

            user.UpdatedAt = DateTime.UtcNow;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"User updated: {id}");

            var response = new UserDetail
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role.ToString(),
                PhoneNumber = user.PhoneNumber,
                FacilityId = user.FacilityId,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
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
            _logger.LogError(ex, $"Error updating user {id}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while updating the user"
            });
        }
    }

    /// <summary>
    /// Get user appointments
    /// </summary>
    [HttpGet("{id}/appointments")]
    [ProducesResponseType(typeof(PagedResult<AppointmentResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserAppointments(Guid id, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                throw new NotFoundException($"User with ID {id} not found");

            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            // Get all FacilityStaff records for this user
            var facilityStaffIds = await _context.FacilityStaffs
                .Where(fs => fs.StaffId == id)
                .Select(fs => fs.Id)
                .ToListAsync();

            var query = _context.Appointments
                .Where(a => facilityStaffIds.Contains(a.FacilityDoctorId.Value))
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
            _logger.LogError(ex, $"Error fetching user appointments {id}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching appointments"
            });
        }
    }

    /// <summary>
    /// Get user medical records
    /// </summary>
    [HttpGet("{id}/medical-records")]
    [ProducesResponseType(typeof(PagedResult<MedicalRecordResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserMedicalRecords(Guid id, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                throw new NotFoundException($"User with ID {id} not found");

            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var query = _context.MedicalRecords.Where(m => m.DoctorId == id).AsQueryable();
            var total = await query.CountAsync();

            var records = await query
                .Include(m => m.Patient)
                .OrderByDescending(m => m.VisitDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(m => new MedicalRecordResponse
                {
                    Id = m.Id,
                    PatientId = m.PatientId,
                    PatientName = $"{m.Patient.FirstName} {m.Patient.LastName}",
                    VisitDate = m.VisitDate,
                    ChiefComplaint = m.ChiefComplaint,
                    Diagnosis = m.Diagnosis,
                    Treatment = m.Treatment,
                    CreatedAt = m.CreatedAt
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
            _logger.LogError(ex, $"Error fetching user medical records {id}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching medical records"
            });
        }
    }

    /// <summary>
    /// Get user prescriptions
    /// </summary>
    [HttpGet("{id}/prescriptions")]
    [ProducesResponseType(typeof(PagedResult<PrescriptionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserPrescriptions(Guid id, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                throw new NotFoundException($"User with ID {id} not found");

            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var query = _context.Prescriptions
                .Where(p => p.MedicalRecord.DoctorId == id)
                .AsQueryable();

            var total = await query.CountAsync();

            var prescriptions = await query
                .OrderByDescending(p => p.CreatedAt)
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
            _logger.LogError(ex, $"Error fetching user prescriptions {id}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching prescriptions"
            });
        }
    }

    /// <summary>
    /// Get user facilities
    /// </summary>
    [HttpGet("{id}/facilities")]
    [ProducesResponseType(typeof(PagedResult<FacilityResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserFacilities(Guid id, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                throw new NotFoundException($"User with ID {id} not found");

            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var query = _context.Facilities.Where(f => f.Users.Any(u => u.Id == id)).AsQueryable();
            var total = await query.CountAsync();

            var facilities = await query
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
                    Type = f.Type,
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
            _logger.LogError(ex, $"Error fetching user facilities {id}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching facilities"
            });
        }
    }

    /// <summary>
    /// Get user patients
    /// <summary>
    /// Get user as patient (returns FacilityPatient records where user is a patient)
    /// </summary>
    [HttpGet("{id}/patients")]
    [ProducesResponseType(typeof(PagedResult<FacilityPatientResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserPatients(Guid id, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                throw new NotFoundException($"User with ID {id} not found");

            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            // Get all FacilityPatient records where this user is the patient
            var query = _context.FacilityPatients
                .Where(fp => fp.PatientId == id)
                .AsQueryable();

            var total = await query.CountAsync();

            var patients = await query
                .Include(fp => fp.Facility)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(fp => new FacilityPatientResponse
                {
                    Id = fp.Id,
                    FacilityId = fp.FacilityId,
                    PatientId = fp.PatientId,
                    PatientName = $"{user.FirstName} {user.LastName}",
                    MedicalRecordNumber = fp.MedicalRecordNumber,
                    IsActive = fp.IsActive,
                    CreatedAt = fp.CreatedAt,
                    UpdatedAt = fp.UpdatedAt
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
            _logger.LogError(ex, $"Error fetching user patients {id}");
            return StatusCode(500, new ErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "An error occurred while fetching patients"
            });
        }
    }
}
