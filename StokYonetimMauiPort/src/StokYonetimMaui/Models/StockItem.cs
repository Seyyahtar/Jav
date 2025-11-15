namespace StokYonetimMaui.Models;

public class StockItem
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string MaterialName { get; set; } = string.Empty;
    public string SerialLotNumber { get; set; } = string.Empty;
    public string UbbCode { get; set; } = string.Empty;
    public DateTime ExpiryDate { get; set; } = DateTime.Today;
    public int Quantity { get; set; }
    public DateTime DateAdded { get; set; } = DateTime.Today;
    public string? From { get; set; }
    public string? To { get; set; }
    public string? MaterialCode { get; set; }
}
