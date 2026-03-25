using Application.DTOs.Assignments;
using FluentValidation;

namespace Application.Validators.Assignments
{
   public class UpdateAssignmentDtoValidator : AbstractValidator<UpdateAssignmentDto>
    {
        public UpdateAssignmentDtoValidator() {
            RuleFor(x => x.Id)
             .GreaterThan(0)
             .WithMessage("Geçerli bir Id girilmelidir.");
          
            RuleFor(x => x.InventoryItemId)
           .GreaterThan(0)
           .WithMessage("Geçerli bir demirbaş ID'si girilmelidir.");

            RuleFor(x => x.InternId)
              .GreaterThan(0)
              .WithMessage("Geçerli bir stajyer ID'si girilmelidir.")
              .When(x => x.InternId.HasValue);

            RuleFor(x => x.EmployeeId)
           .GreaterThan(0)
           .WithMessage("Geçerli bir personel ID'si girilmelidir.")
           .When(x => x.EmployeeId.HasValue);

            RuleFor(x=>x)
                .Must(x => x.InternId.HasValue ^ x.EmployeeId.HasValue)
                .WithMessage("Atama ya bir stajyere ya da bir çalışana yapılmalıdır.");
          
            RuleFor(x=>x.AssignedAt)
            .LessThanOrEqualTo(DateTime.Now)
            .WithMessage("Atanma tarihi gelecekte olamaz.");

            RuleFor(x => x.ActualReturnAt)
              .GreaterThanOrEqualTo(x => x.AssignedAt)
                 .WithMessage("Gerçek iade tarihi, zimmet tarihinden önce olamaz.")
                 .LessThanOrEqualTo(DateTime.Now)
                 .WithMessage("İade tarihi bugünden ileri bir tarih olamaz.")
                .When(x => x.ActualReturnAt.HasValue);

            RuleFor(x => x.Notes)
                .MaximumLength(500)
                .WithMessage("Notlar en fazla 500 karakter olabilir.");
         

        }
    }
}
