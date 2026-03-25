namespace FestivalAppAPI.DTOs.Participant
{
    public class ParticipantDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Competency { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string? PhotoUrl { get; set; }
    }
}