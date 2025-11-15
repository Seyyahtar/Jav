using System.Collections.Generic;
using StokYonetimMaui.Models;

namespace StokYonetimMaui.Services;

public interface IAppRepository
{
    Task<IReadOnlyList<StockItem>> GetStockAsync();
    Task SaveStockItemAsync(StockItem item);
    Task DeleteStockItemAsync(string id);
    Task<bool> StockHasDuplicateAsync(string materialName, string serialLotNumber, string? ignoreId = null);
    Task RemoveStockQuantitiesAsync(IEnumerable<CaseMaterial> materials);

    Task<IReadOnlyList<CaseRecord>> GetCasesAsync();
    Task SaveCaseAsync(CaseRecord record);

    Task<IReadOnlyList<HistoryRecord>> GetHistoryAsync();
    Task AddHistoryAsync(HistoryRecord record);
    Task RemoveHistoryAsync(string id);

    Task<IReadOnlyList<ChecklistRecord>> GetChecklistsAsync();
    Task SaveChecklistAsync(ChecklistRecord record);
    Task UpdateChecklistAsync(ChecklistRecord record);
    Task<ChecklistRecord?> GetActiveChecklistAsync();

    Task<User?> GetUserAsync();
    Task SaveUserAsync(User user);
    Task ClearUserAsync();
}
