using Application.Exceptions;
using Application.Interfaces;

namespace Application.Managers
{
    public class UniversityManager(IUnitOfWork unitOfWork)
    {
        public async Task CheckIfNameIsUniqueAsync(string name, int? currentId = null)
        {
            var existingUni = await unitOfWork.Universities.GetByNameAsync(name);
            if (existingUni != null && existingUni.Id != currentId)
            {
                throw new BadRequestException("Bu üniversite adı başka bir kayıt tarafından kullanılıyor.");
            }
        }

        public async Task CheckIfHasInternsBeforeDeleteAsync(int universityId)
        {
            var interns = await unitOfWork.Interns.GetInternsByUniversityAsync(universityId);
            if (interns.Any())
            {
                throw new BadRequestException("Bu üniversiteye kayıtlı stajyerler var, silinemez!");
            }
        }
    }
}