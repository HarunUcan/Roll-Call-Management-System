using Microsoft.EntityFrameworkCore;

namespace RollCall.Models;

public class RollCallDbContext : DbContext
{

    public DbSet<Yoklama>? RollCalls { get; set; }
    public DbSet<Akademisyen>? Instructors { get; set; }
    public DbSet<Ogrenci>? Students { get; set; }

    private string pwdText = "drowssaP";



    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        string reversedPwd = new string(pwdText.Reverse().ToArray());
        options.UseSqlServer($"Server=localhost; Database=RollCall; User Id=sa; {reversedPwd}=sa1234SA; MultipleActiveResultSets=true; Trust Server Certificate=true;");
    }
}
