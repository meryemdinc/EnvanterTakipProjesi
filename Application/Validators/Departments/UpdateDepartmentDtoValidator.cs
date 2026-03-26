using Application.DTOs.Departments;
using FluentValidation;

namespace Application.Validators.Departments
{
    public class UpdateDepartmentDtoValidator : AbstractValidator<UpdateDepartmentDto>
    {
        public UpdateDepartmentDtoValidator()
        {
            RuleFor(x=>x.Id)
                .GreaterThan(0).WithMessage("Geçerli bir Id girilmelidir.");
            RuleFor(x => x.Name)
                        .NotEmpty().WithMessage("Departman adı boş olamaz.")
                        .MinimumLength(2).WithMessage("Departman adı en az 2 karakter olmalıdır.")
                        .MaximumLength(100).WithMessage("Departman adı en fazla 100 karakter olabilir.");
        }
    }
}
