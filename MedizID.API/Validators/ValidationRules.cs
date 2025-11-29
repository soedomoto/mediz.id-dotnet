using FluentValidation;
using MedizID.API.DTOs;

namespace MedizID.API.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Email must be a valid email address");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required")
            .MinimumLength(6)
            .WithMessage("Password must be at least 6 characters");
    }
}

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Email must be a valid email address");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required")
            .MinimumLength(6)
            .WithMessage("Password must be at least 6 characters");

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First name is required")
            .MaximumLength(100)
            .WithMessage("First name must not exceed 100 characters");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last name is required")
            .MaximumLength(100)
            .WithMessage("Last name must not exceed 100 characters");

        RuleFor(x => x.Role)
            .NotEmpty()
            .WithMessage("Role is required");
    }
}

public class CreateAppointmentRequestValidator : AbstractValidator<CreateAppointmentRequest>
{
    public CreateAppointmentRequestValidator()
    {
        RuleFor(x => x.FacilityPatientId)
            .NotEmpty()
            .WithMessage("Facility Patient ID is required");

        RuleFor(x => x.FacilityDoctorId)
            .NotEmpty()
            .WithMessage("Facility Staff ID is required");

        RuleFor(x => x.AppointmentDate)
            .NotEmpty()
            .WithMessage("Appointment date is required")
            .GreaterThanOrEqualTo(DateTime.Today)
            .WithMessage("Appointment date must be today or in the future");
    }
}

public class CreateMedicalRecordRequestValidator : AbstractValidator<CreateMedicalRecordRequest>
{
    public CreateMedicalRecordRequestValidator()
    {
        RuleFor(x => x.PatientId)
            .NotEmpty()
            .WithMessage("Patient ID is required");

        RuleFor(x => x.VisitDate)
            .NotEmpty()
            .WithMessage("Visit date is required")
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Visit date cannot be in the future");
    }
}

public class CreateFacilityRequestValidator : AbstractValidator<CreateFacilityRequest>
{
    public CreateFacilityRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Facility name is required")
            .MaximumLength(200);

        RuleFor(x => x.Address)
            .NotEmpty()
            .WithMessage("Address is required")
            .MaximumLength(500);

        RuleFor(x => x.City)
            .NotEmpty()
            .WithMessage("City is required")
            .MaximumLength(100);

        RuleFor(x => x.Province)
            .NotEmpty()
            .WithMessage("Province is required")
            .MaximumLength(100);

        RuleFor(x => x.PostalCode)
            .NotEmpty()
            .WithMessage("Postal code is required")
            .MaximumLength(20);

        RuleFor(x => x.Type)
            .NotEmpty()
            .WithMessage("Facility type is required");
    }
}

public class CreateDiagnosisRequestValidator : AbstractValidator<CreateDiagnosisRequest>
{
    public CreateDiagnosisRequestValidator()
    {
        RuleFor(x => x.MedicalRecordId)
            .NotEmpty()
            .WithMessage("Medical record ID is required");

        RuleFor(x => x.DiagnosisCode)
            .NotEmpty()
            .WithMessage("Diagnosis code is required");

        RuleFor(x => x.DiagnosisDescription)
            .NotEmpty()
            .WithMessage("Diagnosis description is required")
            .MaximumLength(500);
    }
}
