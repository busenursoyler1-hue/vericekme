namespace vericekme.Models;

public class UserViewModel
{
    public long? Id { get; set; }
    public string? Name { get; set; }

    public string? Username { get; set; } // ✅ BUNU EKLE

    public string? Website { get; set; }
}