using Application.DTOs.Universities;
using Application.Interfaces; // IUnitOfWork burada
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;

namespace Application.Services
{
    // Primary Constructor kullanımı gayet güzel (C# 12+)
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
            return mapper.Map<UniversityDto>(university);
        }

        // DÜZELTME 1: Async ve SaveAsync Eklendi
        public async Task CreateAsync(CreateUniversityDto createUniversityDto)
        {
            var universityEntity = mapper.Map<University>(createUniversityDto);

            // Repository'e ekle (Henüz DB'ye gitmedi)
            await unitOfWork.Universities.AddAsync(universityEntity);

            // KRİTİK: Değişiklikleri veritabanına onayla!
            await unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(UpdateUniversityDto updateUniversityDto)
        {
            // Önce gerçek kaydı bul
            var universityEntity = await unitOfWork.Universities.GetByIdAsync(updateUniversityDto.Id);

            if (universityEntity == null)
            {
                // Buraya ileride özel bir NotFoundException yazabiliriz
                throw new Exception("University not found");
            }

            // Mevcut kaydın üzerine yeni verileri yaz (Merge)
            mapper.Map(updateUniversityDto, universityEntity);

            // Repository'i bilgilendir (EF Core'da Tracking açıksa şart değil ama güvenli)
            unitOfWork.Universities.Update(universityEntity);

            // Değişiklikleri Kaydet
            await unitOfWork.SaveChangesAsync();
        }

        // DÜZELTME 2: Delete Mantığı Baştan Aşağı Düzeldi
        public async Task DeleteAsync(int id)
        {
            // HATA 1: 'await' eksikti. Task dönüyordu, entity değil.
            var universityEntity = await unitOfWork.Universities.GetByIdAsync(id);

            // HATA 2: Değişken ismi yanlıştı (university vs universityEntity)
            if (universityEntity == null)
            {
                throw new Exception("University not found");
            }

            // Silinecek entity'i repository'e ver
            unitOfWork.Universities.Delete(universityEntity);

            // KRİTİK: Değişiklikleri Kaydet
            await unitOfWork.SaveChangesAsync();
        }
    }
}