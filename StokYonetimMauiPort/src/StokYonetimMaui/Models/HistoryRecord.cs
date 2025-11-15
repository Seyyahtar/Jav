namespace StokYonetimMaui.Models;

public enum HistoryRecordType
{
    StockAdd,
    StockRemove,
    Case,
    StockDelete,
    Checklist
}

public class HistoryRecord
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public DateTime Date { get; set; } = DateTime.Now;
    public HistoryRecordType Type { get; set; }
    public string Description { get; set; } = string.Empty;
    public string DetailsJson { get; set; } = string.Empty;
}
