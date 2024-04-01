using System.ComponentModel.DataAnnotations;

namespace RollCall.Models;

public class Akademisyen
{
    [Key]
    public int Id { get; set; }
    public string? Ad { get; set; }
    public string? Soyad { get; set; }
    public string? TelNo { get; set; }
    public string? Sifre { get; set; }
    public string? Dersler { get; set; }
}
