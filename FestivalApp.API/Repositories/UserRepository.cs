using FestivalAppAPI.Entities;
using FestivalAppAPI.Interfaces;
using FestivalAppAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace FestivalAppAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByIdAsync(Guid id) =>
            await _context.Users.Include(u => u.Region).FirstOrDefaultAsync(u => u.Id == id);

        public async Task<User?> GetByEmailAsync(string email) =>
            await _context.Users.Include(u => u.Region).FirstOrDefaultAsync(u => u.Email == email);

        public async Task<bool> EmailExistsAsync(string email) =>
            await _context.Users.AnyAsync(u => u.Email == email);

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetParticipantsAsync(string? name, int? competencyId, string? category, int? regionId)
        {
            var query = _context.Users
                .Include(u => u.Region)
                .Include(u => u.Applications.Where(a => a.Status == "approved"))
                    .ThenInclude(a => a.Competency)
                .Where(u => u.Role == "user")
                .Where(u => u.Applications.Any(a => a.Status == "approved"))
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(u => u.FirstName.Contains(name) || u.LastName.Contains(name));
            if (competencyId.HasValue)
                query = query.Where(u => u.Applications.Any(a => a.CompetencyId == competencyId && a.Status == "approved"));
            if (!string.IsNullOrWhiteSpace(category))
                query = query.Where(u => u.Category == category);
            if (regionId.HasValue)
                query = query.Where(u => u.RegionId == regionId);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<User>> GetAllAsync() =>
            await _context.Users.ToListAsync();
    }
}