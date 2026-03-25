using System;

namespace FestivalAppAPI.Entities
{
    public class Application
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public int CompetencyId { get; set; }
        public string Status { get; set; } = "pending";
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual User? User { get; set; }
        public virtual Competency? Competency { get; set; }
    }
}