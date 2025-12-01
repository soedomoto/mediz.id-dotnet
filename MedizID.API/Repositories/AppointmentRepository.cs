using Microsoft.EntityFrameworkCore;
using MedizID.API.Common.Enums;
using MedizID.API.Data;
using MedizID.API.Models;

namespace MedizID.API.Repositories;

/// <summary>
/// Appointment repository with specialized query methods
/// </summary>
public class AppointmentRepository : BaseRepository<Appointment>, IAppointmentRepository
{
    public AppointmentRepository(MedizIDDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Appointment>> GetByPatientAsync(Guid patientId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(a => a.FacilityPatientId == patientId)
            .OrderByDescending(a => a.AppointmentDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Appointment>> GetByFacilityAsync(Guid facilityId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(a => a.FacilityId == facilityId)
            .OrderByDescending(a => a.AppointmentDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Appointment>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(a => a.AppointmentDate >= startDate && a.AppointmentDate <= endDate)
            .OrderByDescending(a => a.AppointmentDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Appointment>> GetByStatusAsync(string status, CancellationToken cancellationToken = default)
    {
        if (!Enum.TryParse<AppointmentStatusEnum>(status, out var statusEnum))
            return new List<Appointment>();

        return await _dbSet
            .Where(a => a.Status == statusEnum)
            .OrderByDescending(a => a.AppointmentDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Appointment>> GetUpcomingAsync(int daysAhead = 7, CancellationToken cancellationToken = default)
    {
        var startDate = DateTime.UtcNow;
        var endDate = startDate.AddDays(daysAhead);

        return await _dbSet
            .Where(a => a.AppointmentDate >= startDate && a.AppointmentDate <= endDate)
            .OrderBy(a => a.AppointmentDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<Appointment?> GetDetailedAsync(Guid appointmentId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(a => a.FacilityPatient)
            .ThenInclude(fp => fp.Patient)
            .Include(a => a.FacilityDoctor)
            .ThenInclude(fs => fs.Staff)
            .Include(a => a.Facility)
            .Include(a => a.Anamnesis)
            .Include(a => a.Diagnoses)
            .Include(a => a.Prescriptions)
            .Include(a => a.LaboratoriumTests)
            .FirstOrDefaultAsync(a => a.Id == appointmentId, cancellationToken);
    }
}
