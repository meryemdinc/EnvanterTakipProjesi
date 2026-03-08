namespace Application.DTOs.Universities
{
    public class CreateUniversityDto
    {
        public string Name { get; set; } = null!;
        public string? City { get; set; }
    }
}