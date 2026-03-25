namespace FestivalAppAPI.Entities
{
    public class Competency
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? AssignmentFilePath { get; set; }

        public virtual ICollection<Application> Applications { get; set; } = new List<Application>();
    }
}