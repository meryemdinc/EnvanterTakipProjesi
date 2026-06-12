namespace Application.Messages
{
    // Bu sınıf RabbitMQ kuyruğuna bırakacağımız mesajın taşıyacağı veridir.
    public class InventoryAssignedEvent
    {
        public string EmployeeFullName { get; set; } = string.Empty;
        public string EmployeeEmail { get; set; } = string.Empty;
        public string ItemName { get; set; } = string.Empty;
        public DateTime AssignedAt { get; set; }
    }
}