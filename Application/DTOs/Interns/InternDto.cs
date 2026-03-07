namespace Application.DTOs.Interns
{
    public class InternDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string FullName { get; set; } = null!; // Birleştirilmiş isim kolaylık sağlar
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;

        // İlişkiler (Flattening - Düzleştirme)
        public int UniversityId { get; set; }
        public string UniversityName { get; set; } = null!; // ID yerine İsim!

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Frontend'de hesaplanacak alanlar yerine buradan hazır gönderebiliriz
        public int Score { get; set; }
        public bool IsActive { get; set; } // Stajı devam ediyor mu?
    }
}