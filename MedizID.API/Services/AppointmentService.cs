using MedizID.API.DTOs;
using MedizID.API.Models;
using MedizID.API.Repositories;

namespace MedizID.API.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _repository;
    private readonly ILogger<AppointmentService> _logger;

    public AppointmentService(IAppointmentRepository repository, ILogger<AppointmentService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<AppointmentResponse?> GetAppointmentAsync(Guid appointmentId, CancellationToken cancellationToken = default)
    {
        var appointment = await _repository.GetDetailedAsync(appointmentId, cancellationToken);
        return appointment == null ? null : MapToResponse(appointment);
    }

    public async Task<IEnumerable<AppointmentResponse>> GetPatientAppointmentsAsync(Guid patientId, CancellationToken cancellationToken = default)
    {
        var appointments = await _repository.GetByPatientAsync(patientId, cancellationToken);
        return appointments.Select(MapToResponse).ToList();
    }

    public async Task<IEnumerable<AppointmentResponse>> GetFacilityAppointmentsAsync(Guid facilityId, CancellationToken cancellationToken = default)
    {
        var appointments = await _repository.GetByFacilityAsync(facilityId, cancellationToken);
        return appointments.Select(MapToResponse).ToList();
    }

    public async Task<IEnumerable<AppointmentResponse>> GetUpcomingAppointmentsAsync(Guid facilityId, int daysAhead = 7, CancellationToken cancellationToken = default)
    {
        var appointments = await _repository.GetUpcomingAsync(daysAhead, cancellationToken);
        return appointments.Where(a => a.FacilityId == facilityId).Select(MapToResponse).ToList();
    }

    public async Task<AppointmentResponse> CreateAppointmentAsync(CreateAppointmentRequest request, CancellationToken cancellationToken = default)
    {
        var appointment = new Appointment
        {
            Id = Guid.NewGuid(),
            FacilityId = Guid.Empty,
            FacilityPatientId = request.FacilityPatientId,
            FacilityDoctorId = request.FacilityDoctorId,
            AppointmentDate = request.AppointmentDate,
            AppointmentTime = request.AppointmentTime,
            Status = Common.Enums.AppointmentStatusEnum.Scheduled,
            Reason = request.Reason,
            Notes = request.Notes,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _repository.AddAsync(appointment, cancellationToken);
        return MapToResponse(created);
    }

    public async Task<AppointmentResponse> UpdateAppointmentAsync(Guid appointmentId, UpdateAppointmentRequest request, CancellationToken cancellationToken = default)
    {
        var appointment = await _repository.GetByIdAsync(appointmentId, cancellationToken);
        if (appointment == null) throw new InvalidOperationException($"Appointment not found: {appointmentId}");

        if (request.AppointmentDate.HasValue) appointment.AppointmentDate = request.AppointmentDate.Value;
        if (request.AppointmentTime.HasValue) appointment.AppointmentTime = request.AppointmentTime.Value;
        if (!string.IsNullOrEmpty(request.Reason)) appointment.Reason = request.Reason;
        if (!string.IsNullOrEmpty(request.Notes)) appointment.Notes = request.Notes;
        appointment.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(appointment, cancellationToken);
        return MapToResponse(appointment);
    }

    public async Task<bool> CancelAppointmentAsync(Guid appointmentId, string reason, CancellationToken cancellationToken = default)
    {
        var appointment = await _repository.GetByIdAsync(appointmentId, cancellationToken);
        if (appointment == null) return false;

        appointment.Status = Common.Enums.AppointmentStatusEnum.Cancelled;
        appointment.Notes = $"Cancelled: {reason}";
        appointment.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(appointment, cancellationToken);
        return true;
    }

    public async Task<bool> CompleteAppointmentAsync(Guid appointmentId, CancellationToken cancellationToken = default)
    {
        var appointment = await _repository.GetByIdAsync(appointmentId, cancellationToken);
        if (appointment == null) return false;

        appointment.Status = Common.Enums.AppointmentStatusEnum.Completed;
        appointment.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(appointment, cancellationToken);
        return true;
    }

    private AppointmentResponse MapToResponse(Appointment appointment)
    {
        return new AppointmentResponse
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
    }
}
