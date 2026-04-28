using Application.Exceptions;
using Application.Interfaces;

namespace Application.Managers
{
    public class AppUserManager(IUnitOfWork unitOfWork)
    {
        public async Task CheckIfEmailIsUniqueAsync(string email, int? currentId = null)
        {
            var existingUser = await unitOfWork.AppUsers.GetByEmailAsync(email);
            if (existingUser != null && existingUser.Id != currentId)
            {
                throw new BadRequestException("Bu e-posta adresi zaten kullanımda.");
            }
        }
    }
}