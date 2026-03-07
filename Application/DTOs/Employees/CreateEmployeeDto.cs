namespace Application.DTOs.Employees
{
    public class CreateEmployeeDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Role { get; set; } = "Engineer"; 

        public DateTime StartDate { get; set; } = DateTime.Now;

        public int? DepartmentId { get; set; }

        // Not: AppUser (Login hesabı) genelde buradan değil, "Register" işleminden bağlanır.
    }
}