using Application.DTOs.Common;
using Application.DTOs.Maintenances;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Envanter_Takip_Projesi.Controllers
{
    public class MaintenancesController(IMaintenanceService maintenanceService) : CustomBaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var maintenances = await maintenanceService.GetAllMaintenancesAsync();
            return CreateActionResultInstance(Response<List<MaintenanceDto>>.Success(maintenances, 200));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var maintenance = await maintenanceService.GetByIdAsync(id);
            return CreateActionResultInstance(Response<MaintenanceDto>.Success(maintenance, 200));
        }

        // ÖZEL ENDPOINT: Sadece şu an tamirde olanları getir
        // İstek Adresi: api/Maintenances/active
        [HttpGet("active")]
        public async Task<IActionResult> GetActiveMaintenances()
        {
            var activeMaintenances = await maintenanceService.GetActiveMaintenancesAsync();
            return CreateActionResultInstance(Response<List<MaintenanceDto>>.Success(activeMaintenances, 200));
        }

        // ÖZEL ENDPOINT: Bir cihazın tüm tamir geçmişini getir
        // İstek Adresi: api/Maintenances/history/5
        [HttpGet("history/{inventoryItemId}")]
        public async Task<IActionResult> GetHistory(int inventoryItemId)
        {
            var history = await maintenanceService.GetHistoryByItemIdAsync(inventoryItemId);
            return CreateActionResultInstance(Response<List<MaintenanceDto>>.Success(history, 200));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMaintenanceDto createMaintenanceDto)
        {
            await maintenanceService.CreateAsync(createMaintenanceDto);
            return CreateActionResultInstance(Response<NoContent>.Success(204));
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateMaintenanceDto updateMaintenanceDto)
        {
            await maintenanceService.UpdateAsync(updateMaintenanceDto);
            return CreateActionResultInstance(Response<NoContent>.Success(204));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await maintenanceService.DeleteAsync(id);
            return CreateActionResultInstance(Response<NoContent>.Success(204));
        }
    }
}