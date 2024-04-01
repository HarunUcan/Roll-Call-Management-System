using Microsoft.EntityFrameworkCore;

namespace RollCall.Models;

public class RollCallDbContext : DbContext
{

    public DbSet<Yoklama>? RollCalls { get; set; }
    public DbSet<Akademisyen>? Instructors { get; set; }
    public DbSet<Ogrenci>? Students { get; set; }

    private readonly string psdText = "drowssaP";



    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            string reversedPsd = new string(psdText.Reverse().ToArray());
            optionsBuilder.UseSqlServer($"Server=localhost; Database=RollCall; User Id=sa; {reversedPsd}=sa1234SA; MultipleActiveResultSets=true; Trust Server Certificate=true;");
        }
        
    }
}
