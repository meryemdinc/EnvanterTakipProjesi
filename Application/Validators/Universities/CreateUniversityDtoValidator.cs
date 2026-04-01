using Application.DTOs.Universities;
using FluentValidation;

namespace Application.Validators.Universities
{
    public class CreateUniversityDtoValidator : AbstractValidator<CreateUniversityDto>
    {
        public CreateUniversityDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Üniversite adı boş geçilemez.")
                .MaximumLength(100).WithMessage("Üniversite adı en fazla 100 karakter olabilir.");

            RuleFor(x => x.City)
                .MaximumLength(50).WithMessage("Şehir bilgisi en fazla 50 karakter olabilir.")
                .When(x => !string.IsNullOrEmpty(x.City));
        }
    }
}