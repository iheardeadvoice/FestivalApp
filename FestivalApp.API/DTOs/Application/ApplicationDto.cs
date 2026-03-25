using System;

namespace FestivalAppAPI.DTOs.Application
{
    public class CreateApplicationDto
    {
        public int CompetencyId { get; set; }
        public string? Comment { get; set; }
    }

    public class ApplicationDto
    {
        public int Id { get; set; }
        public int CompetencyId { get; set; }
        public string CompetencyTitle { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class UpdateApplicationStatusDto
    {
        public string Status { get; set; } = string.Empty;
    }
}