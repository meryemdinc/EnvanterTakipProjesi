using Application.DTOs.Employees;
using FluentValidation;

namespace Application.Validators.Employees
{
    public class CreateEmployeeDtoValidator : AbstractValidator<CreateEmployeeDto>
    {
        public CreateEmployeeDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name cannot be empty")
                .MaximumLength(50).WithMessage("First name cannot exceed 50 characters");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name cannot be empty")
                .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email cannot be empty")
                .EmailAddress().WithMessage("A valid email address is required")
                .MaximumLength(100).WithMessage("Email cannot exceed 100 characters");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Phone number cannot be empty")
                .Matches(@"^5\d{9}$").WithMessage("Phone number must be in the format 5xxxxxxxxx");

            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Role cannot be empty")
                .MaximumLength(50).WithMessage("Role cannot exceed 50 characters");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Start date is required")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Start date cannot be in the future");

            RuleFor(x => x.DepartmentId)
                .GreaterThan(0).WithMessage("DepartmentId must be greater than 0 if provided")
                .When(x => x.DepartmentId.HasValue);
        }
    }
}