using ClosedXML.Excel;
using StokYonetimMaui.Models;

namespace StokYonetimMaui.Services;

public class ClosedXmlExcelExportService : IExcelExportService
{
    private readonly IFileSystemService _fileSystemService;

    public ClosedXmlExcelExportService(IFileSystemService fileSystemService)
    {
        _fileSystemService = fileSystemService;
    }

    public async Task<string> ExportStockAsync(IEnumerable<StockItem> stockItems, string filename)
    {
        var filePath = _fileSystemService.GetDatabasePath(filename);
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Stok");

        worksheet.Cell(1, 1).Value = "Malzeme";
        worksheet.Cell(1, 2).Value = "Seri/Lot";
        worksheet.Cell(1, 3).Value = "UBB";
        worksheet.Cell(1, 4).Value = "SKT";
        worksheet.Cell(1, 5).Value = "Adet";
        worksheet.Cell(1, 6).Value = "Eklenme Tarihi";
        worksheet.Cell(1, 7).Value = "Kimden";
        worksheet.Cell(1, 8).Value = "Kime";
        worksheet.Cell(1, 9).Value = "Malzeme Kodu";

        var row = 2;
        foreach (var item in stockItems)
        {
            worksheet.Cell(row, 1).Value = item.MaterialName;
            worksheet.Cell(row, 2).Value = item.SerialLotNumber;
            worksheet.Cell(row, 3).Value = item.UbbCode;
            worksheet.Cell(row, 4).Value = item.ExpiryDate;
            worksheet.Cell(row, 5).Value = item.Quantity;
            worksheet.Cell(row, 6).Value = item.DateAdded;
            worksheet.Cell(row, 7).Value = item.From ?? string.Empty;
            worksheet.Cell(row, 8).Value = item.To ?? string.Empty;
            worksheet.Cell(row, 9).Value = item.MaterialCode ?? string.Empty;
            row++;
        }

        worksheet.Columns().AdjustToContents();
        workbook.SaveAs(filePath);
        await Task.CompletedTask;
        return filePath;
    }
}
