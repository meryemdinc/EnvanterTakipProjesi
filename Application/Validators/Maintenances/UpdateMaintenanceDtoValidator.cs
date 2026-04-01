using Application.DTOs.Maintenances;
using FluentValidation;

namespace Application.Validators.Maintenances
{
    public class UpdateMaintenanceDtoValidator : AbstractValidator<UpdateMaintenanceDto>
    {
        public UpdateMaintenanceDtoValidator() {
            RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage("Geçersiz bakım ID'si.");

          
            RuleFor(x => x.RepairedAt)
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Tamir tarihi gelecekte bir tarih olamaz.")
                .When(x => x.RepairedAt.HasValue);

            RuleFor(x => x.PerformedBy)
                .MaximumLength(50).WithMessage("Tamiri yapan kişi bilgisi en fazla 50 karakter olabilir.")
                .When(x => !string.IsNullOrEmpty(x.PerformedBy));

         
            RuleFor(x => x.Cost)
                .GreaterThanOrEqualTo(0).WithMessage("Maliyet eksi bir değer olamaz.")
                .When(x => x.Cost.HasValue);

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Açıklama alanı en fazla 500 karakter olabilir.")
                .When(x => !string.IsNullOrEmpty(x.Description));
        }
    
    }
}
