namespace Application.DTOs.Interns
{
    public class UpdateInternDto
    {
        public int Id { get; set; } // Hangi stajyer?

        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;

        public int UniversityId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
