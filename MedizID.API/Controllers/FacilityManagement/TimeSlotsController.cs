using MedizID.API.Common.Exceptions;
using MedizID.API.Data;
using MedizID.API.DTOs;
using MedizID.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedizID.API.Controllers.FacilityManagement;

[ApiController]
[Route("api/v1/facilities/{facilityId}/time-slots")]
[Produces("application/json")]
[Authorize]
public class TimeSlotsController : ControllerBase
{
    private readonly MedizIDDbContext _context;
    private readonly ILogger<TimeSlotsController> _logger;

    public TimeSlotsController(MedizIDDbContext context, ILogger<TimeSlotsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Create poli time slot
    /// </summary>
    [HttpPost]
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
    [HttpGet]
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
    [HttpGet("{slotId}")]
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
    [HttpPut("{slotId}")]
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
    [HttpDelete("{slotId}")]
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
}
