using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RollCall.Models;

namespace RollCall.Controllers;

public class YoklamaController : Controller
{
    private readonly ILogger<HomeController> _logger;
    RollCallDbContext _context = new RollCallDbContext();

    public YoklamaController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [HttpPost("yoklama-olustur")]
    public IActionResult Create(string startTime, int Instructors, string lessonName)
    {


        if (startTime != null && Instructors != 0 && lessonName != null)
        {
            try
            {
                Random random = new Random();
                int activationCode;
                do
                {
                    activationCode = random.Next(1000000, 9999000);
                } while (_context.RollCalls?.FirstOrDefault(x => x.AktivasyonKodu == activationCode.ToString()) != null);


                int startHour = int.Parse(startTime.Split(".")[0]);
                int startMinute = int.Parse(startTime.Split(".")[1]);

                DateTime startTimeDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, startHour, startMinute, DateTime.Now.Second);
                DateTime endTime = DateTime.Now.AddMinutes(20);
                //time check for the start time 15 minutes before the current time and 15 minutes after the current time
                if (startTimeDate < DateTime.Now.AddMinutes(-15) || startTimeDate > DateTime.Now.AddMinutes(15))
                {
                    return RedirectToAction("Yonetim", "Akademisyen", new { activationCode = activationCode, state = 3 });
                }


                if (_context != null)
                {
                    var instructor = _context.Instructors?.FirstOrDefault(x => x.Id == Instructors);
                    var ogretimElemani = instructor?.Ad + " " + instructor?.Soyad ?? string.Empty;

                    if (_context.RollCalls != null)
                    {
                        _context.RollCalls.Add(new Yoklama
                        {
                            DersAdi = lessonName,
                            AktivasyonKodu = activationCode.ToString(),
                            OgretimElemani = ogretimElemani,
                            YoklamaBaslangic = startTimeDate,
                            YoklamaBitis = endTime,
                            Katilimcilar = ""
                        });

                        _context.SaveChanges();
                    }
                }

                ViewBag.ActivationCode = activationCode;

                return RedirectToAction("Yonetim", "Akademisyen", new { activationCode = activationCode, state = 1 });
            }
            catch (Exception)
            {
                return RedirectToAction("Yonetim", "Akademisyen", new { state = 4 });
            }
        }

        return RedirectToAction("Yonetim", "Akademisyen", new { state = 4 });
    }

    [HttpGet("yoklama-sil")]
    public bool Delete([FromQuery] string activationCode)
    {
        if (_context.RollCalls != null)
        {
            var rollCall = _context.RollCalls.Where(x => x.AktivasyonKodu == activationCode).FirstOrDefault();
            if (rollCall != null)
            {
                _context.RollCalls.Remove(rollCall);
                _context.SaveChanges();
                return true;
            }
        }
        return false;
    }

    [HttpGet("yoklama-bitir")]
    public bool Finish([FromQuery] string activationCode)
    {
        if (_context != null && _context.RollCalls != null) // Check if _context and _context.RollCalls are not null
        {
            var rollCall = _context.RollCalls.FirstOrDefault(x => x.AktivasyonKodu == activationCode); // Use FirstOrDefault instead of Where().FirstOrDefault()
            if (rollCall != null)
            {
                rollCall.YoklamaBitis = DateTime.Now;
                _context.SaveChanges();
                return true;
            }
        }
        return false;
    }

    [HttpGet("yoklama-kalan-sure")]
    public string RemainingTime([FromQuery] string activationCode)
    {
        if (_context != null && _context.RollCalls != null)
        {
            var rollCall = _context.RollCalls.FirstOrDefault(x => x.AktivasyonKodu == activationCode);
            if (rollCall != null)
            {
                // minutes and seconds remaining for the end of the roll call
                TimeSpan remainingTime = rollCall.YoklamaBitis - DateTime.Now;

                if (remainingTime.TotalSeconds < 0) return "0";

                string remainingTimeStr = $"{remainingTime.Minutes}:{remainingTime.Seconds}";

                return JsonConvert.SerializeObject(remainingTimeStr);
            }
        }
        return string.Empty;
    }

    [HttpGet("ornek-veri-ekle")]
    public void AddSampleData()
    {
        if (_context != null)
        {
            Akademisyen instructor1 = new Akademisyen
            {
                Ad = "Ahmet",
                Soyad = "Yılmaz",
                Dersler = "Mat1,Mat2,Diferansiyel Denklemler",
                TelNo = "05555555555",
                Sifre = "123456"
            };
            Akademisyen instructor2 = new Akademisyen
            {
                Ad = "Mehmet",
                Soyad = "Kaya",
                Dersler = "Fizik1,Fizik2,Statik",
                TelNo = "05555555555",
                Sifre = "123456"
            };
            Akademisyen instructor3 = new Akademisyen
            {
                Ad = "Ayşe",
                Soyad = "Demir",
                Dersler = "Web Programlama,Veritabanı Yönetimi,Bilgisayar Mühendisliğine Giriş,Veri Yapıları ve Algoritmalar",
                TelNo = "05555555555",
                Sifre = "123456"
            };

            Ogrenci student1 = new Ogrenci
            {
                Ad = "Ali",
                Soyad = "Yılmaz",
                Numara = "20222013200",
                Departman = "Bilgisaayar Mühendisliği"
            };
            Ogrenci student2 = new Ogrenci
            {
                Ad = "Veli",
                Soyad = "Kaya",
                Numara = "20222013201",
                Departman = "Endüstri Mühendisliği"
            };
            Ogrenci student3 = new Ogrenci
            {
                Ad = "Tolga",
                Soyad = "Aydın",
                Numara = "20222013202",
                Departman = "Makine Mühendisliği"
            };

            if (_context != null)
            {
                if (_context.Instructors != null)
                {
                    _context.Instructors.Add(instructor1);
                    _context.Instructors.Add(instructor2);
                    _context.Instructors.Add(instructor3);
                }

                if (_context.Students != null)
                {
                    _context.Students.Add(student1);
                    _context.Students.Add(student2);
                    _context.Students.Add(student3);
                }

                _context.SaveChanges();
            }
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
