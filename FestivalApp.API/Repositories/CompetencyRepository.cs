using FestivalAppAPI.Entities;
using FestivalAppAPI.Interfaces;
using FestivalAppAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace FestivalAppAPI.Repositories
{
    public class CompetencyRepository : ICompetencyRepository
    {
        private readonly AppDbContext _context;

        public CompetencyRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Competency?> GetByIdAsync(int id) =>
            await _context.Competencies.FindAsync(id);

        public async Task<IEnumerable<Competency>> GetAllAsync() =>
            await _context.Competencies.ToListAsync();

        public async Task AddAsync(Competency competency)
        {
            await _context.Competencies.AddAsync(competency);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Competency competency)
        {
            _context.Competencies.Update(competency);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id) =>
            await _context.Competencies.AnyAsync(c => c.Id == id);
    }
}