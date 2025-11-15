namespace StokYonetimMaui.Models;

public class ChecklistRecord
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Title { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; } = DateTime.Today;
    public DateTime? CompletedDate { get; set; }
    public bool IsCompleted { get; set; }
    public IList<ChecklistPatient> Patients { get; set; } = new List<ChecklistPatient>();
}

public class ChecklistPatient
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public string? Note { get; set; }
    public string? Phone { get; set; }
    public string? City { get; set; }
    public string? Hospital { get; set; }
    public string? Date { get; set; }
    public string? Time { get; set; }
    public bool Checked { get; set; }
}
