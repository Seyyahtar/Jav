using SQLite;

namespace StokYonetimMaui.Data.Entities;

[Table("CaseRecords")]
public class CaseRecordEntity
{
    [PrimaryKey]
    public string Id { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
    public string HospitalName { get; set; } = string.Empty;
    public string DoctorName { get; set; } = string.Empty;
    public string PatientName { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public string MaterialsJson { get; set; } = string.Empty;
}
