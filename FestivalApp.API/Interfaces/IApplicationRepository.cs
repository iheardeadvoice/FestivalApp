using FestivalAppAPI.Entities;

namespace FestivalAppAPI.Interfaces
{
    public interface IApplicationRepository
    {
        Task<Application?> GetByIdAsync(int id);
        Task<IEnumerable<Application>> GetByUserIdAsync(Guid userId);
        Task<Application?> GetByUserAndCompetencyAsync(Guid userId, int competencyId);
        Task<IEnumerable<Application>> GetAllAsync();
        Task AddAsync(Application application);
        Task UpdateAsync(Application application);
    }
}