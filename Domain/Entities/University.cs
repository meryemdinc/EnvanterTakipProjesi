using Domain.Common;

namespace Domain.Entities
{
    public class University : BaseEntity
    {
        public string Name { get; set; } = null!;

        public string? City { get; set; }

        public ICollection<Intern>? Students { get; set; }
    }
}
