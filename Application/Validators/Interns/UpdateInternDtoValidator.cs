using Application.DTOs.Interns;
using FluentValidation;

namespace Application.Validators.Interns
{
   public class UpdateInternDtoValidator : AbstractValidator<UpdateInternDto>
    { 
        public UpdateInternDtoValidator() {
            RuleFor(i => i.Id)
                .GreaterThan(0).WithMessage("Intern ID must be greater than 0.");
            RuleFor(i => i.FirstName)
              .NotEmpty().WithMessage("First name is required.")
              .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");
            RuleFor(i => i.LastName)
                  .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");
            RuleFor(i => i.Email)
                .NotEmpty().WithMessage("Email is required.")
                  .EmailAddress().WithMessage("A valid email address is required")
                .MaximumLength(100).WithMessage("Email cannot exceed 100 characters.");

            RuleFor(i => i.Phone)
                .NotEmpty().WithMessage("Phone number is required.")
                .MaximumLength(10).WithMessage("Phone number cannot exceed 10 characters.")
                .Matches(@"^5\d{9}$").WithMessage("Phone number must be in the format 5xx xxx xx xx.");

            RuleFor(i => i.UniversityId)
           .GreaterThan(0).WithMessage("University ID must be greater than 0.");

            RuleFor(i => i.StartDate)
                .NotEmpty().WithMessage("Start date is required.");

            RuleFor(i => i.EndDate)
                .NotEmpty().WithMessage("End date is required.")
                .GreaterThan(i => i.StartDate).WithMessage("End date must be after start date.");
         

        }
    
    }
}
