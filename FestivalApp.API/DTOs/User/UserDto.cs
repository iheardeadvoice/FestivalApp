using System;

namespace FestivalAppAPI.DTOs.User
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Patronymic { get; set; }
        public string Education { get; set; } = string.Empty;
        public string Institution { get; set; } = string.Empty;
        public int RegionId { get; set; }
        public string RegionName { get; set; } = string.Empty;
        public string? PhotoUrl { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}