using FestivalAppAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace FestivalAppAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Competency> Competencies { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }
        public DbSet<SystemConfig> SystemConfigs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Application>()
                .HasOne(a => a.User)
                .WithMany(u => u.Applications)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Application>()
                .HasOne(a => a.Competency)
                .WithMany(c => c.Applications)
                .HasForeignKey(a => a.CompetencyId)
                .OnDelete(DeleteBehavior.Restrict);

            SeedData.Seed(modelBuilder);
        }
    }
}