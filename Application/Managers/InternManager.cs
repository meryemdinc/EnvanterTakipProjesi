using Application.Exceptions;
using Application.Interfaces;

namespace Application.Managers
{
    public class InternManager(IUnitOfWork unitOfWork)
    {
        public async Task CheckIfEmailIsUniqueAsync(string email, int? currentId = null)
        {
            var existingIntern = await unitOfWork.Interns.GetByEmailAsync(email);
            if (existingIntern != null && existingIntern.Id != currentId)
            {
                throw new BadRequestException("Bu e-posta adresi zaten başka bir stajyere ait.");
            }
        }
    }
}