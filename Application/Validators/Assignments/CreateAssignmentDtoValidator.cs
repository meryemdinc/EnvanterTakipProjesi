using Application.DTOs.Assignments;
using FluentValidation;

namespace Application.Validators.Assignments
{
    public class CreateAssignmentDtoValidator : AbstractValidator<CreateAssignmentDto>
    {
        public CreateAssignmentDtoValidator() {
        RuleFor(x => x.InventoryItemId)
                .GreaterThan(0).WithMessage("InventoryItemId sıfırdan büyük olmalıdır.");
            RuleFor(x=>x.InternId)
                .GreaterThan(0).When(x => x.InternId.HasValue).WithMessage("InternId sıfırdan büyük olmalıdır.");
            RuleFor(x=>x.EmployeeId)
                .GreaterThan(0).When(x => x.EmployeeId.HasValue).WithMessage("EmployeeId sıfırdan büyük olmalıdır.");
            RuleFor(x => x.AssignedAt)
                .LessThanOrEqualTo(DateTime.Now).WithMessage("AssignedAt alanı gelecekteki bir tarih olamaz.");
            RuleFor(x => x.Notes)
                .MaximumLength(500).WithMessage("Notes alanı en fazla 500 karakter olabilir.");
          
            RuleFor(x => x)
                .Must(x => (x.InternId.HasValue && !x.EmployeeId.HasValue) || (!x.InternId.HasValue && x.EmployeeId.HasValue))
                .WithMessage("Zimmet işlemi ya sadece bir stajyere ya da sadece bir personele yapılmalıdır. İkisi birden seçilemez veya ikisi birden boş bırakılamaz.");
        }
    
    }
}
