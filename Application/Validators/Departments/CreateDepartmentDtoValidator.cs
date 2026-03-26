using Application.DTOs.Departments;
using FluentValidation;

namespace Application.Validators.Departments
{
   public class CreateDepartmentDtoValidator : AbstractValidator<CreateDepartmentDto>
    {
        //IsUnique kontrolü için veritabanına erişim gerekebilir, bu nedenle burada Isunique kontrolü yapmak iyi bir fikir değil.
        public CreateDepartmentDtoValidator() {
            RuleFor(x => x.Name)
                    .NotEmpty().WithMessage("Departman adı boş olamaz.")
                    .MinimumLength(2).WithMessage("Departman adı en az 2 karakter olmalıdır.")
                    .MaximumLength(100).WithMessage("Departman adı en fazla 100 karakter olabilir.");
        }
    
    }
}
