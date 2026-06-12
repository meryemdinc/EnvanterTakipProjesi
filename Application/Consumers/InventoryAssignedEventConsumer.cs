using Application.Messages;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Application.Consumers
{
    // IConsumer arayüzü MassTransit'ten gelir ve hangi mesajı dinleyeceğini belirtir
    public class InventoryAssignedEventConsumer(ILogger<InventoryAssignedEventConsumer> logger) : IConsumer<InventoryAssignedEvent>
    {
        public async Task Consume(ConsumeContext<InventoryAssignedEvent> context)
        {
            var message = context.Message;

            logger.LogInformation("--------------------------------------------------");
            logger.LogInformation($"⏳ RabbitMQ: Mesaj kuyruktan alındı. E-posta gönderimi başlıyor...");

            // Gerçek bir e-posta gönderiyormuşuz gibi sistemi 3 saniye bekletiyoruz
            await Task.Delay(3000);

            logger.LogInformation($"✅ E-POSTA GÖNDERİLDİ!");
            logger.LogInformation($"   Alıcı: {message.EmployeeEmail}");
            logger.LogInformation($"   İçerik: Sayın {message.EmployeeFullName}, '{message.ItemName}' isimli cihaz tarafınıza zimmetlenmiştir.");
            logger.LogInformation("--------------------------------------------------");
        }
    }
}