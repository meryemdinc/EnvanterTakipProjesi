using Application.DTOs.InventoryItems;
using FluentValidation;

namespace Application.Validators.InventoryItems
{
    public class CreateInventoryItemDtoValidator : AbstractValidator<CreateInventoryItemDto>
    {
        public CreateInventoryItemDtoValidator()
        {
            RuleFor(x => x.ItemCode)
                .NotEmpty().WithMessage("Demirbaş kodu boş geçilemez.")
                .MaximumLength(100).WithMessage("Demirbaş kodu en fazla 100 karakter olabilir.");

          
            RuleFor(x => x.SerialNumber)
                .MinimumLength(3).WithMessage("Seri numarası en az 3 karakter olmalıdır.")
                .MaximumLength(50).WithMessage("Seri numarası en fazla 50 karakter olabilir.")
                .When(x => !string.IsNullOrEmpty(x.SerialNumber));

            RuleFor(x => x.Category)
                .NotEmpty().WithMessage("Kategori alanı boş geçilemez.")
                .MaximumLength(50).WithMessage("Kategori alanı en fazla 50 karakter olabilir."); 

        
            RuleFor(x => x.Brand)
                .MaximumLength(50).WithMessage("Marka alanı en fazla 50 karakter olabilir.")
                .When(x => !string.IsNullOrEmpty(x.Brand));

            RuleFor(x => x.Model)
                .MaximumLength(100).WithMessage("Model alanı en fazla 100 karakter olabilir.")
                .When(x => !string.IsNullOrEmpty(x.Model));

            RuleFor(x => x.Notes)
                .MaximumLength(100).WithMessage("Notlar alanı en fazla 100 karakter olabilir.");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Geçersiz bir demirbaş durumu girdiniz.");

         
            RuleFor(x => x.PurchaseDate)
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Satın alma tarihi gelecekte bir tarih olamaz.")
                .When(x => x.PurchaseDate.HasValue);

            RuleFor(x => x.WarrantyEndDate)
                .GreaterThan(x => x.PurchaseDate).WithMessage("Garanti bitiş tarihi, satın alma tarihinden sonra olmalıdır.")
                .When(x => x.WarrantyEndDate.HasValue && x.PurchaseDate.HasValue);
        }
    }
}