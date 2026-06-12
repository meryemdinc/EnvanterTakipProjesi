using Application.Messages;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Application.Consumers
{
    public class MaintenanceStartedEventConsumer(ILogger<MaintenanceStartedEventConsumer> logger) : IConsumer<MaintenanceStartedEvent>
    {
        public async Task Consume(ConsumeContext<MaintenanceStartedEvent> context)
        {
            var message = context.Message;

            logger.LogInformation("--------------------------------------------------");
            logger.LogInformation($"🔧 RabbitMQ: Bakım talebi alındı. Bilgilendirme e-postası hazırlanıyor...");

            // Gerçek bir e-posta yolluyormuş gibi 3 saniye bekliyoruz
            await Task.Delay(3000);

            logger.LogInformation($"✅ E-POSTA GÖNDERİLDİ!");
            logger.LogInformation($"   Kime: {message.EmployeeEmail}");
            logger.LogInformation($"   Konu: Cihazınız Bakıma Alındı");
            logger.LogInformation($"   İçerik: '{message.ItemName}' isimli cihazınız '{message.MaintenanceReason}' sebebiyle bakıma alınmıştır. İşlem tamamlandığında size tekrar bilgi verilecektir.");
            logger.LogInformation("--------------------------------------------------");
        }
    }
}