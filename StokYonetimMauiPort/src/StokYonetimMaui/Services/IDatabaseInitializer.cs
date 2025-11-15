using SQLite;

namespace StokYonetimMaui.Services;

public interface IDatabaseInitializer
{
    Task<SQLiteAsyncConnection> GetConnectionAsync();
}
