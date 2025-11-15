using SQLite;

namespace StokYonetimMaui.Data.Entities;

[Table("HistoryRecords")]
public class HistoryRecordEntity
{
    [PrimaryKey]
    public string Id { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
    public int Type { get; set; }
    public string Description { get; set; } = string.Empty;
    public string DetailsJson { get; set; } = string.Empty;
}
