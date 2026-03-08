namespace Application.DTOs.Universities
{
    public class UniversityDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? City { get; set; }

        // Bu okuldan kaç stajyerimiz var?
        public int InternCount { get; set; }
    }
}