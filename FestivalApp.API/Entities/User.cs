using System;

namespace FestivalAppAPI.Entities
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Patronymic { get; set; }
        public string Education { get; set; } = string.Empty;
        public string Institution { get; set; } = string.Empty;
        public int RegionId { get; set; }
        public string? PhotoPath { get; set; }
        public string Category { get; set; } = string.Empty; // school, student, specialist
        public string Role { get; set; } = "user";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public virtual Region? Region { get; set; }
        public virtual ICollection<Application> Applications { get; set; } = new List<Application>();
    }
}