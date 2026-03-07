namespace Application.DTOs.Interns
{
    public class CreateInternDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;

        // Kullanıcı arayüzünde Dropdown'dan seçilen ID'ler gelir
        public int UniversityId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Not: Score ilk girişte genelde 0'dır veya yoktur, buraya koymayız.
    }
}
