using StokYonetimMaui.Models;

namespace StokYonetimMaui.Services;

public interface IExcelExportService
{
    Task<string> ExportStockAsync(IEnumerable<StockItem> stockItems, string filename);
}
