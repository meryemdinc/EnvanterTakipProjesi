using Application.DTOs.Universities;
using Application.Exceptions;
using Application.Interfaces.Services;
using Application.Managers;
using AutoMapper;
using Domain.Entities;
using Application.Interfaces;

namespace Application.Services
{
    // DİKKAT: ICacheService cacheService parametresini buraya ekledik!
    public class UniversityService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        UniversityManager universityManager,
        ICacheService cacheService) : IUniversityService // Cache parametresi eklendi
    {
        // Cache için kullanacağımız sabit anahtar kelime
        private const string CacheKey = "all_universities";

        public async Task<List<UniversityDto>> GetAllUniversitiesAsync()
        {
            // 1. Önce Cache'e bak!
            var cachedUniversities = await cacheService.GetAsync<List<UniversityDto>>(CacheKey);

            if (cachedUniversities != null)
            {
                // Veritabanına hiç gitmedik, RAM'den saniyesinde döndük!
                return cachedUniversities;
            }

            // 2. Cache'de yoksa Veritabanından (DB) al ve ID'ye göre sırala
            var universities = await unitOfWork.Universities.GetAllAsync();
            var sortedList = universities.OrderBy(x => x.Id).ToList();
            var dtoList = mapper.Map<List<UniversityDto>>(sortedList);

            // 3. Bir dahaki sefere hızlı gelsin diye Cache'e kaydet (Örn: 2 saat kalsın)
            await cacheService.SetAsync(CacheKey, dtoList, TimeSpan.FromHours(2));

            return dtoList;
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
            // 1. KURALI ÇALIŞTIR: Üniversite adı benzersiz mi?
            await universityManager.CheckIfNameIsUniqueAsync(createUniversityDto.Name);

            // 2. KAYDET
            var universityEntity = mapper.Map<University>(createUniversityDto);
            await unitOfWork.Universities.AddAsync(universityEntity);
            await unitOfWork.SaveChangesAsync();

            // YENİ VERİ EKLENDİ -> CACHE UÇURULMALI
            await cacheService.RemoveAsync(CacheKey);
        }

        public async Task UpdateAsync(UpdateUniversityDto updateUniversityDto)
        {
            var universityEntity = await unitOfWork.Universities.GetByIdAsync(updateUniversityDto.Id);
            if (universityEntity == null)
            {
                throw new NotFoundException("Üniversite bulunamadı");
            }

            // 1. KURALI ÇALIŞTIR: Yeni üniversite adı başkasına ait mi? (Kendi ID'sini gönderiyoruz)
            await universityManager.CheckIfNameIsUniqueAsync(updateUniversityDto.Name, universityEntity.Id);

            // 2. GÜNCELLE
            mapper.Map(updateUniversityDto, universityEntity);
            unitOfWork.Universities.Update(universityEntity);
            await unitOfWork.SaveChangesAsync();

            // VERİ GÜNCELLENDİ -> CACHE UÇURULMALI
            await cacheService.RemoveAsync(CacheKey);
        }

        public async Task DeleteAsync(int id)
        {
            var universityEntity = await unitOfWork.Universities.GetByIdAsync(id);
            if (universityEntity == null)
            {
                throw new NotFoundException("Üniversite bulunamadı");
            }

            // 1. KURALI ÇALIŞTIR: İçeride bu üniversiteye kayıtlı stajyer var mı?
            await universityManager.CheckIfHasInternsBeforeDeleteAsync(id);

            // 2. SİL
            unitOfWork.Universities.Delete(universityEntity);
            await unitOfWork.SaveChangesAsync();

            // VERİ SİLİNDİ -> CACHE UÇURULMALI
            await cacheService.RemoveAsync(CacheKey);
        }
    }
}