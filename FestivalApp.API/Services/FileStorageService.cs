using FestivalAppAPI.Interfaces;

namespace FestivalAppAPI.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;

        public FileStorageService(IWebHostEnvironment env, IConfiguration config)
        {
            _env = env;
            _config = config;
        }

        public async Task<string> SaveFileAsync(Stream fileStream, string fileName, string subDirectory)
        {
            var uploadsRoot = Path.Combine(_env.WebRootPath ?? _env.ContentRootPath, "uploads", subDirectory);
            if (!Directory.Exists(uploadsRoot))
                Directory.CreateDirectory(uploadsRoot);

            var uniqueName = $"{Guid.NewGuid()}_{Path.GetFileName(fileName)}";
            var filePath = Path.Combine(uploadsRoot, uniqueName);

            using var fileStreamOutput = new FileStream(filePath, FileMode.Create);
            await fileStream.CopyToAsync(fileStreamOutput);

            return Path.Combine("uploads", subDirectory, uniqueName).Replace('\\', '/');
        }

        public void DeleteFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return;
            var fullPath = Path.Combine(_env.WebRootPath ?? _env.ContentRootPath, filePath.Replace('/', Path.DirectorySeparatorChar));
            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }

        public string GetFileUrl(string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath)) return string.Empty;
            var baseUrl = _config["BaseUrl"] ?? "http://localhost:5000";
            return $"{baseUrl}/{relativePath.Replace('\\', '/')}";
        }
    }
}