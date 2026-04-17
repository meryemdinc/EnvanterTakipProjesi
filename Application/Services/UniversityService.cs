using Application.DTOs.Universities;
using Application.Exceptions;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using Application.Interfaces;
namespace Application.Services
{
    public class UniversityService(IUnitOfWork unitOfWork, IMapper mapper) : IUniversityService
    {
        public async Task<List<UniversityDto>> GetAllUniversitiesAsync()
        {
            var universities = await unitOfWork.Universities.GetAllAsync();
            return mapper.Map<List<UniversityDto>>(universities);
        }

        public async Task<UniversityDto> GetByIdAsync(int id)
        {
            var university = await unitOfWork.Universities.GetByIdAsync(id);
        
            if (university == null)
            {
                throw new NotFoundException($"{id} ID'li üniversite bulunamadı.");
            }
            return mapper.Map<UniversityDto>(university);
        }

        public async Task CreateAsync(CreateUniversityDto createUniversityDto)
        {
           
            var existingUni = await unitOfWork.Universities.GetByNameAsync(createUniversityDto.Name);
            if (existingUni != null)
            {
                throw new BadRequestException("Bu üniversite zaten sistemde kayıtlı.");
            }

            var universityEntity = mapper.Map<University>(createUniversityDto);
            await unitOfWork.Universities.AddAsync(universityEntity);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(UpdateUniversityDto updateUniversityDto)
        {
            var universityEntity = await unitOfWork.Universities.GetByIdAsync(updateUniversityDto.Id);
            if (universityEntity == null)
            {
                throw new NotFoundException("Üniversite bulunamadı");
            }

           
            var existingNameUni = await unitOfWork.Universities.GetByNameAsync(updateUniversityDto.Name);
            if (existingNameUni != null && existingNameUni.Id != universityEntity.Id)
            {
                throw new BadRequestException("Bu üniversite adı başka bir kayıt tarafından kullanılıyor.");
            }

            mapper.Map(updateUniversityDto, universityEntity);
            unitOfWork.Universities.Update(universityEntity);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var universityEntity = await unitOfWork.Universities.GetByIdAsync(id);
            if (universityEntity == null)
            {
                throw new NotFoundException("Üniversite bulunamadı");
            }

      
            var interns = await unitOfWork.Interns.GetInternsByUniversityAsync(id);
            if (interns.Any())
            {
                throw new BadRequestException("Bu üniversiteye kayıtlı stajyerler var, silinemez!");
            }

            unitOfWork.Universities.Delete(universityEntity);
            await unitOfWork.SaveChangesAsync();
        }
    }
}