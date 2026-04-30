using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;

namespace Application.Managers
{
    public class MaintenanceManager(IUnitOfWork unitOfWork)
    {
        // KURAL 1: Cihaz zaten aktif olarak bakımda mı?
        public async Task CheckIfItemAlreadyInMaintenanceAsync(int inventoryItemId)
        {
            var existingActiveMaintenance = await unitOfWork.Maintenances.FindAsync(m =>
                m.InventoryItemId == inventoryItemId && m.RepairedAt == null);

            if (existingActiveMaintenance.Any())
            {
                throw new BadRequestException("Bu cihaza ait devam eden aktif bir bakım kaydı zaten mevcut.");
            }
        }

        // KURAL 2: Cihaz bakıma uygun mu? (Örn: Zimmetliyse alınamaz)
        public void CheckIfItemIsEligibleForMaintenance(InventoryItem item)
        {
            if (item.Status == ItemStatus.Assigned)
            {
                throw new BadRequestException("Zimmetli olan bir cihaz doğrudan bakıma alınamaz. Önce zimmet iadesi yapmalısınız.");
            }
        }

        // KURAL 3: Tamamlanmış bir bakım silinebilir mi?
        public void CheckIfMaintenanceCanBeDeleted(Maintenance maintenance)
        {
            if (maintenance.RepairedAt != null)
            {
                throw new BadRequestException("Bu kaydı silemezsin, cihazın bakımı çoktan tamamlanmış.");
            }
        }
    }
}