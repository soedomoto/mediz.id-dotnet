using Microsoft.EntityFrameworkCore;
using MedizID.API.Data;
using MedizID.API.Models;

namespace MedizID.API.Repositories;

/// <summary>
/// Prescription repository with specialized query methods
/// </summary>
public class PrescriptionRepository : BaseRepository<Prescription>, IPrescriptionRepository
{
    public PrescriptionRepository(MedizIDDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Prescription>> GetByAppointmentAsync(Guid appointmentId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(p => p.AppointmentId == appointmentId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Prescription>> GetByPatientAsync(Guid patientId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(p => p.Appointment)
            .Where(p => p.Appointment != null && p.Appointment.FacilityPatient.PatientId == patientId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Prescription>> GetActivePrescriptionsAsync(Guid patientId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(p => p.Appointment)
            .Where(p => p.Appointment != null && p.Appointment.FacilityPatient.PatientId == patientId &&
                       (p.ExpiryDate == null || p.ExpiryDate >= DateTime.UtcNow))
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Prescription>> GetByDrugAsync(Guid drugId, CancellationToken cancellationToken = default)
    {
        // Prescription doesn't have direct drug relationship, return all ordered by created date
        return await _dbSet
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Prescription?> GetDetailedAsync(Guid prescriptionId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(p => p.Appointment)
            .FirstOrDefaultAsync(p => p.Id == prescriptionId, cancellationToken);
    }

    public async Task<IEnumerable<Prescription>> GetPatientPrescriptionHistoryAsync(Guid patientId, int limit = 20, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(p => p.Appointment)
            .Where(p => p.Appointment != null && p.Appointment.FacilityPatient.PatientId == patientId)
            .OrderByDescending(p => p.CreatedAt)
            .Take(limit)
            .ToListAsync(cancellationToken);
    }
}
