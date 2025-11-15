using SQLite;

namespace StokYonetimMaui.Data.Entities;

[Table("StockItems")]
public class StockItemEntity
{
    [PrimaryKey]
    public string Id { get; set; } = string.Empty;
    public string MaterialName { get; set; } = string.Empty;
    public string SerialLotNumber { get; set; } = string.Empty;
    public string UbbCode { get; set; } = string.Empty;
    public string ExpiryDate { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string DateAdded { get; set; } = string.Empty;
    public string? From { get; set; }
    public string? To { get; set; }
    public string? MaterialCode { get; set; }
}
