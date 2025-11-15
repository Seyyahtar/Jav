using SQLite;
using StokYonetimMaui.Data.Entities;

namespace StokYonetimMaui.Services;

public class SqliteDatabaseInitializer : IDatabaseInitializer
{
    private const string DatabaseFilename = "stok_yonetim.db3";
    private readonly IFileSystemService _fileSystemService;
    private SQLiteAsyncConnection? _connection;

    public SqliteDatabaseInitializer(IFileSystemService fileSystemService)
    {
        _fileSystemService = fileSystemService;
    }

    public async Task<SQLiteAsyncConnection> GetConnectionAsync()
    {
        if (_connection is not null)
        {
            return _connection;
        }

        var path = _fileSystemService.GetDatabasePath(DatabaseFilename);
        _connection = new SQLiteAsyncConnection(path, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);

        await _connection.CreateTableAsync<StockItemEntity>();
        await _connection.CreateTableAsync<CaseRecordEntity>();
        await _connection.CreateTableAsync<HistoryRecordEntity>();
        await _connection.CreateTableAsync<ChecklistRecordEntity>();
        await _connection.CreateTableAsync<UserEntity>();

        return _connection;
    }
}
