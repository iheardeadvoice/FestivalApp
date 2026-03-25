using FestivalAppAPI.Entities;

namespace FestivalAppAPI.Interfaces
{
    public interface ICompetencyRepository
    {
        Task<Competency?> GetByIdAsync(int id);
        Task<IEnumerable<Competency>> GetAllAsync();
        Task AddAsync(Competency competency);
        Task UpdateAsync(Competency competency);
        Task<bool> ExistsAsync(int id);
    }
}