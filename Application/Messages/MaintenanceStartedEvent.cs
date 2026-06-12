namespace Application.Messages
{
    public class MaintenanceStartedEvent
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public string EmployeeEmail { get; set; } = string.Empty;
        public string? MaintenanceReason { get; set; }
    }
}