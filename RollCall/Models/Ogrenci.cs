using System.ComponentModel.DataAnnotations;

namespace RollCall.Models;

public class Ogrenci
{
    [Key]
    public int Id { get; set; }
    public string? Ad { get; set; }
    public string? Soyad { get; set; }
    public string? Numara { get; set; }
    public string? Departman { get; set; }
}