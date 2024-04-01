using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RollCall.Models;

namespace RollCall.Controllers;

public class OgrenciGirisController : Controller
{

    private readonly RollCallDbContext _context = new RollCallDbContext();


    [HttpGet("ogrenci-giris")]
    public IActionResult OgrenciGiris([FromQuery] int code, int state, string message)
    {
        ViewBag.State = state;
        ViewBag.Message = message;
        return View();
    }
    [HttpGet("ogrenci-getir")]
    public string OgrenciGetir([FromQuery] string studentId)
    {
        var student = _context.Students?.Where(x => x.Numara == studentId).FirstOrDefault();
        return JsonConvert.SerializeObject(student);
    }
    [HttpGet("ders-getir")]
    public string DersGetir([FromQuery] string activationCode)
    {
        var rollCall = _context.RollCalls?.Where(x => x.AktivasyonKodu == activationCode).FirstOrDefault();
        var lessonName = rollCall?.DersAdi;
        return JsonConvert.SerializeObject(lessonName);
    }
    [HttpPost("ogrenci-giris")]
    public ActionResult YoklamaKontrol(string activationCode, string studentId)
    {
        var rollCall = _context.RollCalls?.Where(x => x.AktivasyonKodu == activationCode).FirstOrDefault();
        if (rollCall != null)
        {
            var endTime = rollCall.YoklamaBitis;
            if (DateTime.Now > endTime)
            {
                return RedirectToAction("OgrenciGiris", "OgrenciGiris", new { state = 1, message = "Yoklama suresi sona erdi." });
            }
            var student = _context.Students?.Where(x => x.Numara == studentId).FirstOrDefault();
            if (student != null)
            {
                if (rollCall.Katilimcilar != null && rollCall.Katilimcilar.Contains(studentId))
                {
                    return RedirectToAction("OgrenciGiris", "OgrenciGiris", new { state = 1, message = "Ogrenci zaten yoklamaya katilmis." });
                }
                else
                {
                    rollCall.Katilimcilar += studentId + ",";
                    _context.SaveChanges();
                    return RedirectToAction("OgrenciGiris", "OgrenciGiris", new { state = 1, message = "Ogrenci yoklamaya basarili bir sekilde katildi." });
                }
            }
            else
            {
                return RedirectToAction("OgrenciGiris", "OgrenciGiris", new { state = 1, message = "Ogrenci bulunamadi." });
            }
        }
        else
        {
            return RedirectToAction("OgrenciGiris", "OgrenciGiris", new { state = 1, message = "Yoklama bulunamadi." });
        }
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
public enum RollCallState
{
    YoklamaSuresiSonaErdi = 1,
    OgrenciZatenYoklamayaKatilmis = 2,
    OgrenciYoklamayaKatildi = 3,
    OgrenciBulunamadi = 4,
    YoklamaBulunamadi = 5
}
