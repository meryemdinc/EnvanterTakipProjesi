namespace Application.DTOs.Departments
{
    public class DepartmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        // Bu departmanda kaç kişi çalışıyor? (İstatistik için)
        public int EmployeeCount { get; set; }
    }
}
