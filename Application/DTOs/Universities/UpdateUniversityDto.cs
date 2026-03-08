namespace Application.DTOs.Universities
{
    public class UpdateUniversityDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? City { get; set; }
    }
}