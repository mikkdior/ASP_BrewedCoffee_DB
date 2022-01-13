using Microsoft.EntityFrameworkCore;

namespace ASP_BrewedCoffee_DB.Models;
public class CDBContext : DbContext
{
    public DbSet<CPost>? Posts { get; set; }
    public DbSet<CCategory>? Categories { get; set; }
    public DbSet<COption>? Options { get; set; }

    public CDBContext(DbContextOptions options): base(options)
    {
        Database.EnsureCreated();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=coffee_v02;Trusted_Connection=True;");
    }
    public string GetOptionsValue(string key)
    {
        foreach (COption opt in Options) if (opt.Key == key) return opt.Value;

        return "";
    }
}