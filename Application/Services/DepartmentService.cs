using Application.DTOs.Departments;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Services;
using Application.Managers;
using AutoMapper;
using Domain.Entities;

namespace Application.Services
{
    // DİKKAT: ICacheService cacheService parametresini buraya ekledik!
    public class DepartmentService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        DepartmentManager departmentManager,
        ICacheService cacheService) : IDepartmentService
    {
        private const string CacheKey = "all_departments";

        public async Task<List<DepartmentDto>> GetAllDepartmentsAsync()
        {
            // 1. Önce Cache'e bak!
            var cachedDepartments = await cacheService.GetAsync<List<DepartmentDto>>(CacheKey);

            if (cachedDepartments != null)
            {
                return cachedDepartments; // Veritabanına hiç gitmedik, saniyesinde döndük!
            }

            // 2. Cache'de yoksa Veritabanından (DB) al ve ID'ye göre SIRALA (Sona gitme sorununu çözer)
            var departments = await unitOfWork.Departments.GetAllAsync();
            var sortedList = departments.OrderBy(d => d.Id).ToList();
            var dtoList = mapper.Map<List<DepartmentDto>>(sortedList);

            // 3. Bir dahaki sefere hızlı gelsin diye Cache'e kaydet (Örn: 2 saat kalsın)
            await cacheService.SetAsync(CacheKey, dtoList, TimeSpan.FromHours(2));

            return dtoList;
        }

        public async Task<DepartmentDto> GetByIdAsync(int id)
        {
            var existingDepartment = await unitOfWork.Departments.GetByIdAsync(id);
            if (existingDepartment == null)
            {
                throw new NotFoundException("Departman bulunamadı");
            }
            return mapper.Map<DepartmentDto>(existingDepartment);
        }

        public async Task CreateAsync(CreateDepartmentDto createDepartmentDto)
        {
            await departmentManager.CheckIfNameIsUniqueAsync(createDepartmentDto.Name);

            var department = mapper.Map<Department>(createDepartmentDto);
            await unitOfWork.Departments.AddAsync(department);
            await unitOfWork.SaveChangesAsync();

            // YENİ BİR DEPARTMAN EKLENDİ! Eski listeyi çöpe atalım ki yeni girenler listelensin
            await cacheService.RemoveAsync(CacheKey);
        }

        public async Task UpdateAsync(UpdateDepartmentDto updateDepartmentDto)
        {
            var department = await unitOfWork.Departments.GetByIdAsync(updateDepartmentDto.Id);
            if (department == null)
            {
                throw new NotFoundException("Departman bulunamadı");
            }

            // 1. KURALI ÇALIŞTIR: Yeni isim başka bir departmanda kullanılıyor mu? (Kendi ID'sini yolluyoruz)
            await departmentManager.CheckIfNameIsUniqueAsync(updateDepartmentDto.Name, department.Id);

            // 2. GÜNCELLE
            mapper.Map(updateDepartmentDto, department);
            unitOfWork.Departments.Update(department);
            await unitOfWork.SaveChangesAsync();

            // VERİ GÜNCELLENDİ -> CACHE UÇURULMALI
            await cacheService.RemoveAsync(CacheKey);
        }

        public async Task DeleteAsync(int id)
        {
            var department = await unitOfWork.Departments.GetByIdAsync(id);
            if (department == null)
            {
                throw new NotFoundException("Departman bulunamadı");
            }

            // 1. KURALI ÇALIŞTIR: İçeride çalışan personel var mı?
            await departmentManager.CheckIfHasEmployeesBeforeDeleteAsync(department.Id);

            // 2. SİL
            unitOfWork.Departments.Delete(department);
            await unitOfWork.SaveChangesAsync();

            // VERİ SİLİNDİ -> CACHE UÇURULMALI
            await cacheService.RemoveAsync(CacheKey);
        }
    }
}