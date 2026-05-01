namespace Application.Interfaces.Services
{
    public interface IHRReminderService
    {
        // Hangfire bu metodu zamanı gelince tetikleyecek
        Task SendInternshipEndingReminderAsync(int internId, string internFullName, string internEmail);
    }
}