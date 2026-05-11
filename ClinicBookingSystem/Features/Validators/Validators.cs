using ClinicBookingSystem.Features.AppointmentService;
using ClinicBookingSystem.Features.Authentication;
using ClinicBookingSystem.Features.DoctorServices;
using ClinicBookingSystem.Features.SharedDtos;
using FluentValidation;

namespace ClinicBookingSystem.Features.Validators
{
    public class Validators
    {
        public class CreateTimeslotDtoValidator : AbstractValidator<CreateTimeslotDTO>
        {
            public CreateTimeslotDtoValidator()
            {
                RuleFor(x => x.Starts_At)
                    .NotEmpty().WithMessage("Start time is required")
                    .GreaterThan(DateTime.UtcNow)
                    .WithMessage("Timeslot must be scheduled in the future.");

                RuleFor(x => x.Duration)
                    .GreaterThan(0).WithMessage("Duration must be greater than zero.")
                    .GreaterThanOrEqualTo(30).WithMessage("Duration must be at least 30 minutes.");
            }
        }
        public class CreateDoctorDtoValidator : AbstractValidator<CreateDoctorDTO>
        {
            public CreateDoctorDtoValidator()
            {
                RuleFor(x => x.Username)
                    .NotEmpty().WithMessage("Username is required.")
                    .MinimumLength(3).WithMessage("Username must be at least 3 characters.")
                    .MaximumLength(50).WithMessage("Username cannot exceed 50 characters.")
                    .Matches(@"^[a-zA-Z0-9_-]+$").WithMessage("Username can only contain letters, numbers, hyphens, and underscores.");

                RuleFor(x => x.Specialty)
                    .IsInEnum().WithMessage("Invalid specialty selected.");
            }
        }
        public class UserDtoValidator : AbstractValidator<userDTO>
        {
            public UserDtoValidator()
            {
                RuleFor(x => x.Username)
                    .NotEmpty().WithMessage("Username is required.")
                    .MinimumLength(3).WithMessage("Username must be at least 3 characters.")
                    .MaximumLength(50).WithMessage("Username cannot exceed 50 characters.");

                RuleFor(x => x.Password)
                    .NotEmpty().WithMessage("Password is required.");
            }
        }
        public class PaginatedDtoValidator : AbstractValidator<PaginatedDTO>
        {
            public PaginatedDtoValidator()
            {
                RuleFor(x => x.Username)
                    .NotEmpty().WithMessage("Username is required.")
                    .MinimumLength(1).WithMessage("Username cannot be empty.");

                RuleFor(x => x.Page)
                    .GreaterThanOrEqualTo(1).WithMessage("Page must be at least 1.");

                RuleFor(x => x.PageSize)
                    .GreaterThanOrEqualTo(1).WithMessage("Page size must be at least 1.")
                    .LessThanOrEqualTo(100).WithMessage("Page size cannot exceed 100.");
            }
        }
        public class CreateAppointmentDtoValidator : AbstractValidator<CreateAppointmentDTO>
        {
            public CreateAppointmentDtoValidator()
            {
                RuleFor(x => x.timeslotId)
                    .GreaterThan(0).WithMessage("Timeslot ID must be a valid positive integer.");

                RuleFor(x => x.Notes)
                    .MaximumLength(500).WithMessage("Notes cannot exceed 500 characters.")
                    .When(x => !string.IsNullOrWhiteSpace(x.Notes));
            }
        }
    }
}
