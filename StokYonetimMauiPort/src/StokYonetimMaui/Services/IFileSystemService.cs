namespace StokYonetimMaui.Services;

public interface IFileSystemService
{
    string GetDatabasePath(string filename);
    Task<Stream> CreateExportFileAsync(string filename);
}
