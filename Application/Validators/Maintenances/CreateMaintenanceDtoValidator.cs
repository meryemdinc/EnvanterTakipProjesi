using Application.DTOs.Maintenances;
using FluentValidation;

namespace Application.Validators.Maintenances
{
    public class CreateMaintenanceDtoValidator : AbstractValidator<CreateMaintenanceDto>
    {
        public CreateMaintenanceDtoValidator()
        {
            RuleFor(x => x.InventoryItemId)
                .GreaterThan(0).WithMessage("Geçerli bir demirbaş ID'si girilmelidir.");

            RuleFor(x => x.ReportedAt)
                .NotEmpty().WithMessage("Arıza bildirim tarihi boş geçilemez.")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Bildirim tarihi gelecekte bir tarih olamaz.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Açıklama alanı en fazla 500 karakter olabilir.")
                .When(x => !string.IsNullOrEmpty(x.Description));
        }
    }
}