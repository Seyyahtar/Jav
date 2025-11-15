using SQLite;

namespace StokYonetimMaui.Data.Entities;

[Table("ChecklistRecords")]
public class ChecklistRecordEntity
{
    [PrimaryKey]
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string CreatedDate { get; set; } = string.Empty;
    public string? CompletedDate { get; set; }
    public bool IsCompleted { get; set; }
    public string PatientsJson { get; set; } = string.Empty;
}
