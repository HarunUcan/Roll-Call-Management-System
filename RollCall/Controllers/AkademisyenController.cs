using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RollCall.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RollCall.Controllers;

public class AkademisyenController : Controller
{
    private RollCallDbContext _context = new RollCallDbContext();


    [HttpGet("akademisyen-giris")]
    public IActionResult AkademisyenGiris()
    {
        return View();
    }
    [HttpGet("akademisyen-yonetim")]
    public IActionResult Yonetim(string activationCode, int state)
    {
        _context = new RollCallDbContext();
        List<SelectListItem> items = new List<SelectListItem>();
        var Instructors = _context.Instructors?.ToList() ?? new List<Akademisyen>();
        foreach (var item in Instructors)
        {
            items.Add(new SelectListItem { Text = item.Ad + " " + item.Soyad, Value = item.Id.ToString() });
        }
        ViewBag.Instructors = items;
        ViewBag.ActivationCode = activationCode;
        ViewBag.State = state;
        return View();
    }
    [HttpGet("akademisyen-yonetim/ders-getir")]
    public List<string> DersGetir([FromQuery] int instructorId)
    {
        var instructor = _context.Instructors?.FirstOrDefault(x => x.Id == instructorId);
        if (instructor != null)
        {
            var lessons = instructor.Dersler;
            List<string> LessonsList = lessons?.Split(',').ToList() ?? new List<string>();
            return LessonsList;
        }
        else
        {
            return new List<string>();
        }
    }

    


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
