using FestivalAppAPI.Entities;
using FestivalAppAPI.Interfaces;
using FestivalAppAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace FestivalAppAPI.Repositories
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly AppDbContext _context;

        public ApplicationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Application?> GetByIdAsync(int id) =>
            await _context.Applications.Include(a => a.Competency).FirstOrDefaultAsync(a => a.Id == id);

        public async Task<IEnumerable<Application>> GetByUserIdAsync(Guid userId) =>
            await _context.Applications.Include(a => a.Competency).Where(a => a.UserId == userId).ToListAsync();

        public async Task<Application?> GetByUserAndCompetencyAsync(Guid userId, int competencyId) =>
            await _context.Applications.FirstOrDefaultAsync(a => a.UserId == userId && a.CompetencyId == competencyId);

        public async Task<IEnumerable<Application>> GetAllAsync() =>
            await _context.Applications.Include(a => a.User).Include(a => a.Competency).ToListAsync();

        public async Task AddAsync(Application application)
        {
            await _context.Applications.AddAsync(application);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Application application)
        {
            _context.Applications.Update(application);
            await _context.SaveChangesAsync();
        }
    }
}