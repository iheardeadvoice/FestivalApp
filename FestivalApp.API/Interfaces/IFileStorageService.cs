namespace FestivalAppAPI.Interfaces
{
    public interface IFileStorageService
    {
        Task<string> SaveFileAsync(Stream fileStream, string fileName, string subDirectory);
        void DeleteFile(string filePath);
        string GetFileUrl(string relativePath);
    }
}