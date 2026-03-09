using Application.DTOs.Interns;
using Application.DTOs.AppUsers;
using Application.DTOs.Assignments;
using Application.DTOs.Departments;
using Application.DTOs.Employees;
using Application.DTOs.InventoryItems;
using Application.DTOs.Maintenances;
using Application.DTOs.Universities;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ==========================
            // 1. INTERN MAPPING
            // ==========================
            CreateMap<Intern, InternDto>()
                // ?. operatörü (Null Check) ekledim. Eğer University null gelirse hata vermez, boş geçer.
                .ForMember(dest => dest.UniversityName, opt => opt.MapFrom(src => src.University != null ? src.University.Name : null))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName));

            CreateMap<CreateInternDto, Intern>();
            CreateMap<UpdateInternDto, Intern>();


            // ==========================
            // 2. EMPLOYEE MAPPING
            // ==========================
            CreateMap<Employee, EmployeeDto>()
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department != null ? src.Department.Name : null))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName))
                .ForMember(dest => dest.HasSystemAccess, opt => opt.MapFrom(src => src.AppUserId != null));

            CreateMap<CreateEmployeeDto, Employee>();
            CreateMap<UpdateEmployeeDto, Employee>();


            // ==========================
            // 3. DEPARTMENT MAPPING
            // ==========================
            CreateMap<Department, DepartmentDto>()
                .ForMember(dest => dest.EmployeeCount, opt => opt.MapFrom(src => src.Employees != null ? src.Employees.Count : 0));

            CreateMap<CreateDepartmentDto, Department>();
            CreateMap<UpdateDepartmentDto, Department>();


            // ==========================
            // 4. UNIVERSITY MAPPING
            // ==========================
            CreateMap<University, UniversityDto>()
                .ForMember(dest => dest.InternCount, opt => opt.MapFrom(src => src.Students != null ? src.Students.Count : 0));

            CreateMap<CreateUniversityDto, University>();
            CreateMap<UpdateUniversityDto, University>();


            // ==========================
            // 5. INVENTORY ITEM MAPPING
            // ==========================
            CreateMap<InventoryItem, InventoryItemDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))

                // BU KISMI DÜZENLEDİM (Daha okunaklı ve parantez hatasız):
                .ForMember(dest => dest.CurrentHolderName, opt => opt.MapFrom(src =>
                    src.Assignments.Any(a => a.ActualReturnAt == null) // Aktif zimmet var mı?
                    ? (src.Assignments.First(a => a.ActualReturnAt == null).EmployeeId != null
                        ? src.Assignments.First(a => a.ActualReturnAt == null).Employee.FullName
                        : src.Assignments.First(a => a.ActualReturnAt == null).Intern.FullName)
                    : null));

            CreateMap<CreateInventoryItemDto, InventoryItem>();
            CreateMap<UpdateInventoryItemDto, InventoryItem>();


            // ==========================
            // 6. ASSIGNMENT MAPPING 
            // ==========================
            CreateMap<Assignment, AssignmentDto>()
                // EKSİK 1: Eşya Adı (Marka + Model)
                .ForMember(dest => dest.InventoryItemName, opt => opt.MapFrom(src =>
                     src.InventoryItem != null
                     ? $"{src.InventoryItem.Brand} {src.InventoryItem.Model} ({src.InventoryItem.ItemCode})"
                     : null))
                // EKSİK 2: Seri Numarası
                .ForMember(dest => dest.SerialNumber, opt => opt.MapFrom(src => src.InventoryItem != null ? src.InventoryItem.SerialNumber : null))

                // Diğer alanlar
                .ForMember(dest => dest.InternName, opt => opt.MapFrom(src => src.Intern != null ? src.Intern.FullName : null))
                .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.FullName : null))
                .ForMember(dest => dest.AssignedTo, opt => opt.MapFrom(src =>
                    src.Intern != null ? $"{src.Intern.FullName} (Stajyer)"
                    : (src.Employee != null ? $"{src.Employee.FullName} (Çalışan)" : "Bilinmiyor")));

            CreateMap<CreateAssignmentDto, Assignment>();
            CreateMap<UpdateAssignmentDto, Assignment>();
            CreateMap<ReturnAssignmentDto, Assignment>();


            // ==========================
            // 7. MAINTENANCE MAPPING
            // ==========================
            CreateMap<Maintenance, MaintenanceDto>()
                .ForMember(dest => dest.InventoryItemName, opt => opt.MapFrom(src =>
                    src.InventoryItem != null
                    ? $"{src.InventoryItem.Brand} {src.InventoryItem.Model} ({src.InventoryItem.ItemCode})"
                    : null))
                .ForMember(dest => dest.IsCompleted, opt => opt.MapFrom(src => src.RepairedAt != null));

            CreateMap<CreateMaintenanceDto, Maintenance>();
            CreateMap<UpdateMaintenanceDto, Maintenance>();


            // ==========================
            // 8. APPUSER MAPPING
            // ==========================
            CreateMap<AppUser, AppUserDto>()
                .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.FullName : null));

            CreateMap<RegisterDto, AppUser>()
             .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
             .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

            CreateMap<LoginDto, AppUser>();
            CreateMap<AppUser, AuthResponseDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.FullName : null))
               .ForMember(dest => dest.Token, opt => opt.Ignore());
        }
    }
}