namespace StokYonetimMaui.Models;

public class CaseRecord
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public DateTime Date { get; set; } = DateTime.Today;
    public string HospitalName { get; set; } = string.Empty;
    public string DoctorName { get; set; } = string.Empty;
    public string PatientName { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public IList<CaseMaterial> Materials { get; set; } = new List<CaseMaterial>();
}

public class CaseMaterial
{
    public string MaterialName { get; set; } = string.Empty;
    public string SerialLotNumber { get; set; } = string.Empty;
    public string UbbCode { get; set; } = string.Empty;
    public int Quantity { get; set; }
}
