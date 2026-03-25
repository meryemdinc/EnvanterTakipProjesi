using Application.DTOs.AppUsers;
using FluentValidation;

namespace Application.Validators.AppUsers
{
    public class LoginDtoValidator:AbstractValidator<LoginDto>
    {
       
        public LoginDtoValidator() 
        {
            RuleFor(x => x.Email)
                    .NotEmpty().WithMessage("Email alanı boş bırakılamaz.")
                  .MaximumLength(100).WithMessage("Email alanı en fazla 100 karakter olabilir.")
                    .EmailAddress().WithMessage("Geçerli bir email adresi giriniz.");
            RuleFor(x=>x.Password)
                    .NotEmpty().WithMessage("Password alanı boş bırakılamaz.");

        }

    }
}
