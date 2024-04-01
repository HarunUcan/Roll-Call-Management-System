using System.ComponentModel.DataAnnotations;

namespace RollCall.Models;
public class Yoklama
{
    [Key]
    public int Id { get; set; }
    public string? DersAdi { get; set; }
    public string? AktivasyonKodu { get; set; }
    public string? OgretimElemani { get; set; }
    public DateTime YoklamaBaslangic { get; set; }
    public DateTime YoklamaBitis { get; set; }
    public string? Katilimcilar { get; set; }
}