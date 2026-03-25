using Application.DTOs.AppUsers;
using FluentValidation;

namespace Application.Validators.AppUsers
{
    public class RegisterDtoValidator: AbstractValidator<RegisterDto>
    {
     
        public RegisterDtoValidator() {
            RuleFor(x => x.FirstName)
                    .NotEmpty().WithMessage("FirstName alanı boş bırakılamaz.")
                    .MaximumLength(50).WithMessage("FirstName alanı en fazla 50 karakter olabilir.");
           RuleFor(x => x.LastName)
                    .NotEmpty().WithMessage("LastName alanı boş bırakılamaz.")
                    .MaximumLength(50).WithMessage("LastName alanı en fazla 50 karakter olabilir.");

            RuleFor(x => x.Email)
                    .NotEmpty().WithMessage("Email alanı boş bırakılamaz.")
                    .MaximumLength(100).WithMessage("Email alanı en fazla 100 karakter olabilir.")
                    .EmailAddress().WithMessage("Geçerli bir email adresi giriniz.");
            RuleFor(x => x.Password)
                     .NotEmpty().WithMessage("Şifre alanı boş bırakılamaz.")
                     .MinimumLength(10).WithMessage("Şifreniz en az 8 karakter olmalıdır.")
                     .Matches(@"[A-Z]").WithMessage("Şifreniz en az bir büyük harf içermelidir.")
                     .Matches(@"[a-z]").WithMessage("Şifreniz en az bir küçük harf içermelidir.")
                     .Matches(@"[0-9]").WithMessage("Şifreniz en az bir rakam içermelidir.")
                     .Matches(@"[\W]").WithMessage("Şifreniz en az bir özel karakter içermelidir.");
            RuleFor(x => x.EmployeeId)
                    .GreaterThan(0).When(x => x.EmployeeId.HasValue).WithMessage("EmployeeId sıfırdan büyük olmalıdır.");


        }
    }
}
