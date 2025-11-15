using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using SQLite;
using StokYonetimMaui.Data.Entities;
using StokYonetimMaui.Models;

namespace StokYonetimMaui.Services;

public class AppRepository : IAppRepository
{
    private readonly IDatabaseInitializer _databaseInitializer;

    public AppRepository(IDatabaseInitializer databaseInitializer)
    {
        _databaseInitializer = databaseInitializer;
    }

    public async Task<IReadOnlyList<StockItem>> GetStockAsync()
    {
        var connection = await _databaseInitializer.GetConnectionAsync();
        var entities = await connection.Table<StockItemEntity>().ToListAsync();
        return entities.Select(Map).OrderByDescending(s => s.DateAdded).ToList();
    }

    public async Task SaveStockItemAsync(StockItem item)
    {
        var connection = await _databaseInitializer.GetConnectionAsync();
        var entity = Map(item);
        await connection.InsertOrReplaceAsync(entity);
    }

    public async Task DeleteStockItemAsync(string id)
    {
        var connection = await _databaseInitializer.GetConnectionAsync();
        await connection.DeleteAsync<StockItemEntity>(id);
    }

    public async Task<bool> StockHasDuplicateAsync(string materialName, string serialLotNumber, string? ignoreId = null)
    {
        var connection = await _databaseInitializer.GetConnectionAsync();
        var entities = await connection.Table<StockItemEntity>().ToListAsync();
        return entities.Any(s =>
            !string.Equals(s.Id, ignoreId, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(s.MaterialName, materialName, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(s.SerialLotNumber, serialLotNumber, StringComparison.OrdinalIgnoreCase));
    }

    public async Task RemoveStockQuantitiesAsync(IEnumerable<CaseMaterial> materials)
    {
        var connection = await _databaseInitializer.GetConnectionAsync();
        foreach (var material in materials)
        {
            var entity = await connection.Table<StockItemEntity>()
                .Where(s => s.MaterialName == material.MaterialName && s.SerialLotNumber == material.SerialLotNumber)
                .FirstOrDefaultAsync();

            if (entity is null)
            {
                continue;
            }

            entity.Quantity -= material.Quantity;
            if (entity.Quantity <= 0)
            {
                await connection.DeleteAsync<StockItemEntity>(entity.Id);
            }
            else
            {
                await connection.UpdateAsync(entity);
            }
        }
    }

    public async Task<IReadOnlyList<CaseRecord>> GetCasesAsync()
    {
        var connection = await _databaseInitializer.GetConnectionAsync();
        var entities = await connection.Table<CaseRecordEntity>().ToListAsync();
        return entities.Select(Map).OrderByDescending(c => c.Date).ToList();
    }

    public async Task SaveCaseAsync(CaseRecord record)
    {
        var connection = await _databaseInitializer.GetConnectionAsync();
        var entity = Map(record);
        await connection.InsertOrReplaceAsync(entity);
    }

    public async Task<IReadOnlyList<HistoryRecord>> GetHistoryAsync()
    {
        var connection = await _databaseInitializer.GetConnectionAsync();
        var entities = await connection.Table<HistoryRecordEntity>().OrderByDescending(h => h.Date).ToListAsync();
        return entities.Select(Map).ToList();
    }

    public async Task AddHistoryAsync(HistoryRecord record)
    {
        var connection = await _databaseInitializer.GetConnectionAsync();
        var entity = Map(record);
        await connection.InsertAsync(entity);
    }

    public async Task RemoveHistoryAsync(string id)
    {
        var connection = await _databaseInitializer.GetConnectionAsync();
        await connection.DeleteAsync<HistoryRecordEntity>(id);
    }

    public async Task<IReadOnlyList<ChecklistRecord>> GetChecklistsAsync()
    {
        var connection = await _databaseInitializer.GetConnectionAsync();
        var entities = await connection.Table<ChecklistRecordEntity>().OrderByDescending(c => c.CreatedDate).ToListAsync();
        return entities.Select(Map).ToList();
    }

    public async Task SaveChecklistAsync(ChecklistRecord record)
    {
        var connection = await _databaseInitializer.GetConnectionAsync();
        var entity = Map(record);
        await connection.InsertAsync(entity);
    }

    public async Task UpdateChecklistAsync(ChecklistRecord record)
    {
        var connection = await _databaseInitializer.GetConnectionAsync();
        var entity = Map(record);
        await connection.UpdateAsync(entity);
    }

    public async Task<ChecklistRecord?> GetActiveChecklistAsync()
    {
        var connection = await _databaseInitializer.GetConnectionAsync();
        var entity = await connection.Table<ChecklistRecordEntity>().Where(c => !c.IsCompleted).FirstOrDefaultAsync();
        return entity is null ? null : Map(entity);
    }

    public async Task<User?> GetUserAsync()
    {
        var connection = await _databaseInitializer.GetConnectionAsync();
        var entity = await connection.Table<UserEntity>().FirstOrDefaultAsync();
        return entity is null ? null : Map(entity);
    }

    public async Task SaveUserAsync(User user)
    {
        var connection = await _databaseInitializer.GetConnectionAsync();
        var entity = Map(user);
        entity.Id = 1;
        await connection.InsertOrReplaceAsync(entity);
    }

    public async Task ClearUserAsync()
    {
        var connection = await _databaseInitializer.GetConnectionAsync();
        await connection.DeleteAllAsync<UserEntity>();
    }

    private static StockItem Map(StockItemEntity entity) => new()
    {
        Id = entity.Id,
        MaterialName = entity.MaterialName,
        SerialLotNumber = entity.SerialLotNumber,
        UbbCode = entity.UbbCode,
        ExpiryDate = DateTime.TryParse(entity.ExpiryDate, out var date) ? date : DateTime.Today,
        Quantity = entity.Quantity,
        DateAdded = DateTime.TryParse(entity.DateAdded, out var added) ? added : DateTime.Today,
        From = entity.From,
        To = entity.To,
        MaterialCode = entity.MaterialCode
    };

    private static StockItemEntity Map(StockItem item) => new()
    {
        Id = item.Id,
        MaterialName = item.MaterialName,
        SerialLotNumber = item.SerialLotNumber,
        UbbCode = item.UbbCode,
        ExpiryDate = item.ExpiryDate.ToString("yyyy-MM-dd"),
        Quantity = item.Quantity,
        DateAdded = item.DateAdded.ToString("yyyy-MM-dd"),
        From = item.From,
        To = item.To,
        MaterialCode = item.MaterialCode
    };

    private static CaseRecord Map(CaseRecordEntity entity) => new()
    {
        Id = entity.Id,
        Date = DateTime.TryParse(entity.Date, out var date) ? date : DateTime.Today,
        HospitalName = entity.HospitalName,
        DoctorName = entity.DoctorName,
        PatientName = entity.PatientName,
        Notes = entity.Notes,
        Materials = JsonSerializer.Deserialize<List<CaseMaterial>>(entity.MaterialsJson) ?? new List<CaseMaterial>()
    };

    private static CaseRecordEntity Map(CaseRecord record) => new()
    {
        Id = record.Id,
        Date = record.Date.ToString("yyyy-MM-dd"),
        HospitalName = record.HospitalName,
        DoctorName = record.DoctorName,
        PatientName = record.PatientName,
        Notes = record.Notes,
        MaterialsJson = JsonSerializer.Serialize(record.Materials)
    };

    private static HistoryRecord Map(HistoryRecordEntity entity) => new()
    {
        Id = entity.Id,
        Date = DateTime.TryParse(entity.Date, out var date) ? date : DateTime.Now,
        Type = (HistoryRecordType)entity.Type,
        Description = entity.Description,
        DetailsJson = entity.DetailsJson
    };

    private static HistoryRecordEntity Map(HistoryRecord record) => new()
    {
        Id = record.Id,
        Date = record.Date.ToString("o"),
        Type = (int)record.Type,
        Description = record.Description,
        DetailsJson = record.DetailsJson
    };

    private static ChecklistRecord Map(ChecklistRecordEntity entity) => new()
    {
        Id = entity.Id,
        Title = entity.Title,
        CreatedDate = DateTime.TryParse(entity.CreatedDate, out var created) ? created : DateTime.Today,
        CompletedDate = DateTime.TryParse(entity.CompletedDate, out var completed) ? completed : null,
        IsCompleted = entity.IsCompleted,
        Patients = JsonSerializer.Deserialize<List<ChecklistPatient>>(entity.PatientsJson) ?? new List<ChecklistPatient>()
    };

    private static ChecklistRecordEntity Map(ChecklistRecord record) => new()
    {
        Id = record.Id,
        Title = record.Title,
        CreatedDate = record.CreatedDate.ToString("yyyy-MM-dd"),
        CompletedDate = record.CompletedDate?.ToString("yyyy-MM-dd"),
        IsCompleted = record.IsCompleted,
        PatientsJson = JsonSerializer.Serialize(record.Patients)
    };

    private static User Map(UserEntity entity) => new()
    {
        Username = entity.Username,
        LoginDate = DateTime.TryParse(entity.LoginDate, out var login) ? login : DateTime.Now
    };

    private static UserEntity Map(User user) => new()
    {
        Username = user.Username,
        LoginDate = user.LoginDate.ToString("o")
    };
}
