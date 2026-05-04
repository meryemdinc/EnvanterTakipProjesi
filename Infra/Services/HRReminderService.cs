using Application.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services
{
    public class HRReminderService(ILogger<HRReminderService> logger) : IHRReminderService
    {
        public async Task SendInternshipEndingReminderAsync(int internId, string internFullName, string internEmail)
        {
            // Gerçek projede burada SMTP ile Mail gönderimi yapılır.
            // Şimdilik sistem loglarına ve konsola düşmesini sağlıyoruz.

            logger.LogInformation("🔔 [İK BİLDİRİMİ]: {FullName} ({Email}) adlı stajyerin staj süresi 3 gün sonra bitiyor! Lütfen çıkış belgelerini ve donanım iadelerini hazırlayın.", internFullName, internEmail);

            await Task.CompletedTask;
        }
    }
}