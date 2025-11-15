using SQLite;

namespace StokYonetimMaui.Data.Entities;

[Table("Users")]
public class UserEntity
{
    [PrimaryKey]
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string LoginDate { get; set; } = string.Empty;
}
