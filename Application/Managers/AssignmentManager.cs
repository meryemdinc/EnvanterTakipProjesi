using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;

namespace Application.Managers
{
    public class AssignmentManager(IUnitOfWork unitOfWork)
    {
        // KURAL 1: Eşya zimmetlenmeye uygun mu? (Boşta mı?)
        public void CheckIfItemIsAvailableForAssignment(InventoryItem item)
        {
            if (item.Status != ItemStatus.Available)
            {
                throw new BadRequestException($"Bu eşya şu anda zimmetlenemez. Mevcut durumu: {item.Status}");
            }
        }

        // KURAL 2: Personele/Stajyere aynı kategoriden eşya verilmiş mi?
        public async Task CheckIfUserAlreadyHasSameCategoryItemAsync(int? employeeId, string category)
        {
            // Eğer EmployeeId üzerinden takip ediliyorsa (Stajyer/Personel)
            if (employeeId.HasValue)
            {
                var existingAssignments = await unitOfWork.Assignments.GetAllAsync();

                var hasSameCategoryItem = existingAssignments.Any(a =>
                    a.EmployeeId == employeeId.Value &&
                    a.ActualReturnAt == null && // Henüz iade etmemiş
                    a.InventoryItem != null &&
                    a.InventoryItem.Category == category);

                if (hasSameCategoryItem)
                {
                    throw new BadRequestException("Bu personele/stajyere aynı kategoriden ikinci bir eşya zimmetlenemez!");
                }
            }
        }

        // KURAL 3: Zimmet zaten iade edilmiş mi?
        public void CheckIfAlreadyReturned(Assignment assignment)
        {
            if (assignment.ActualReturnAt != null)
            {
                throw new BadRequestException("Bu zimmet zaten daha önceden iade edilmiş!");
            }
        }
    }
}