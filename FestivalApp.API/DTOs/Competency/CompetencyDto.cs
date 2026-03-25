namespace FestivalAppAPI.DTOs.Competency
{
    public class CompetencyDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? AssignmentFileUrl { get; set; }
    }

    public class CreateCompetencyDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public IFormFile? AssignmentFile { get; set; }
    }

    public class UpdateCompetencyDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public IFormFile? AssignmentFile { get; set; }
        public bool DeleteAssignment { get; set; } = false;
    }
}