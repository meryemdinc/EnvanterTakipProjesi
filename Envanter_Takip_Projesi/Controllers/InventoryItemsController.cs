using Application.DTOs.Common;
using Application.DTOs.InventoryItems;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Envanter_Takip_Projesi.Controllers
{
    public class InventoryItemsController(IInventoryItemService inventoryItemService, IElasticSearchService elasticSearchService) : CustomBaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await inventoryItemService.GetAllItemsAsync();
            return CreateActionResultInstance(Response<List<InventoryItemDto>>.Success(items, 200));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await inventoryItemService.GetByIdAsync(id);
            return CreateActionResultInstance(Response<InventoryItemDto>.Success(item, 200));
        }

        // ÖZEL ENDPOINT: Sadece boştaki eşyaları getir (Zimmet ekranında dropdown için çok işine yarayacak!)
        // İstek Adresi: api/InventoryItems/available
        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableItems()
        {
            var availableItems = await inventoryItemService.GetAvailableItemsAsync();
            return CreateActionResultInstance(Response<List<InventoryItemDto>>.Success(availableItems, 200));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateInventoryItemDto createInventoryItemDto)
        {
            // 1. Veritabanına (PostgreSQL) Kaydet
            await inventoryItemService.CreateAsync(createInventoryItemDto);

            // 2. 🚀 EKSİK OLAN KISIM EKLENDİ: Arama yapabilmek için ElasticSearch'e indeksle!
            await elasticSearchService.IndexDocumentAsync(createInventoryItemDto, "inventory_items");

            return CreateActionResultInstance(Response<NoContent>.Success(204));
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateInventoryItemDto updateInventoryItemDto)
        {
            await inventoryItemService.UpdateAsync(updateInventoryItemDto);
            return CreateActionResultInstance(Response<NoContent>.Success(204));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await inventoryItemService.DeleteAsync(id);
            return CreateActionResultInstance(Response<NoContent>.Success(204));
        }

        // 1. ARAMA UCU
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string keyword)
        {
            // inventory_items indeksinde, kullanıcının girdiği kelimeyi ara
            var results = await elasticSearchService.SearchAsync<InventoryItemDto>(keyword, "inventory_items");

            return CreateActionResultInstance(Response<List<InventoryItemDto>>.Success(results, 200));
        }

        // ÖZEL ENDPOINT: Veritabanındaki eski kayıtları ElasticSearch'e kopyalar
        // İstek Adresi: POST api/InventoryItems/sync-elastic
        [HttpPost("sync-elastic")]
        public async Task<IActionResult> SyncElastic()
        {
            // 1. Veritabanındaki tüm eşyaları çek
            var allItems = await inventoryItemService.GetAllItemsAsync();

            // 2. Hepsini tek tek ElasticSearch'e indeksle
            int successCount = 0;
            foreach (var item in allItems)
            {
                await elasticSearchService.IndexDocumentAsync(item, "inventory_items");
                successCount++;
            }

            // Bu sefer Response zarfı yerine basit bir mesaj dönelim
            return Ok(new { Message = $"Harika! Toplam {successCount} adet geçmiş cihaz başarıyla ElasticSearch arama motoruna aktarıldı." });
        }
    }
}