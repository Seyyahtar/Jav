using Microsoft.Maui.Storage;

namespace StokYonetimMaui.Services;

public class FileSystemService : IFileSystemService
{
    public string GetDatabasePath(string filename)
    {
        var directory = FileSystem.AppDataDirectory;
        return Path.Combine(directory, filename);
    }

    public async Task<Stream> CreateExportFileAsync(string filename)
    {
        var directory = FileSystem.AppDataDirectory;
        Directory.CreateDirectory(directory);
        var path = Path.Combine(directory, filename);
        var stream = File.Create(path);
        await Task.CompletedTask;
        return stream;
    }
}
