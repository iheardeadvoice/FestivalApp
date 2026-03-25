using FestivalAppAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace FestivalAppAPI.Data
{
    public static class SeedData
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            // Regions
            modelBuilder.Entity<Region>().HasData(
                new Region { Id = 1, Name = "Москва" },
                new Region { Id = 2, Name = "Санкт-Петербург" },
                new Region { Id = 3, Name = "Новосибирск" },
                new Region { Id = 4, Name = "Екатеринбург" },
                new Region { Id = 5, Name = "Казань" }
            );

            // Admin
            var adminId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = adminId,
                Email = "admin@festival.ru",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("passadmin"),
                FirstName = "Админ",
                LastName = "Админов",
                Patronymic = "Админович",
                Education = "Высшее",
                Institution = "Фестиваль",
                RegionId = 1,
                Category = "specialist",
                Role = "admin",
                CreatedAt = DateTime.UtcNow
            });

            // Competencies
            modelBuilder.Entity<Competency>().HasData(
                new Competency { Id = 1, Title = "Веб-разработка", Description = "Создание современных веб-приложений", AssignmentFilePath = "uploads/assignments/web_dev_task.pdf" },
                new Competency { Id = 2, Title = "Графический дизайн", Description = "Разработка визуального контента", AssignmentFilePath = "uploads/assignments/design_task.pdf" },
                new Competency { Id = 3, Title = "Аналитика данных", Description = "Обработка и анализ больших данных", AssignmentFilePath = "uploads/assignments/data_analytics_task.pdf" }
            );

            // System config
            modelBuilder.Entity<SystemConfig>().HasData(
                new SystemConfig { Key = "FestivalStartDate", Value = "2025-06-01T00:00:00Z" }
            );
        }
    }
}