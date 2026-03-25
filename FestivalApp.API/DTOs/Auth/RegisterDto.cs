using Microsoft.AspNetCore.Http;

namespace FestivalAppAPI.DTOs.Auth
{
    public class RegisterDto
    {
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string? Patronymic { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Education { get; set; } = string.Empty;
        public string Institution { get; set; } = string.Empty;
        public int RegionId { get; set; }
        public string Category { get; set; } = string.Empty;
        public IFormFile? Photo { get; set; }
    }
}