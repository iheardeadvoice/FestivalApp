using FestivalAppAPI.Entities;

namespace FestivalAppAPI.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByEmailAsync(string email);
        Task<bool> EmailExistsAsync(string email);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task<IEnumerable<User>> GetParticipantsAsync(string? name, int? competencyId, string? category, int? regionId);
        Task<IEnumerable<User>> GetAllAsync();
    }
}