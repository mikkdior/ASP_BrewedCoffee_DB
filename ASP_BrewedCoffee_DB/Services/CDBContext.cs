using Microsoft.EntityFrameworkCore;

namespace ASP_BrewedCoffee_DB.Models;
public class CDBContext : DbContext
{
    private IConfiguration Config;
    public DbSet<CPost>? Posts { get; set; }
    public DbSet<CCategory>? Categories { get; set; }
    public DbSet<COption>? Options { get; set; }
    public DbSet<CRoute>? Routes { get; set; }

    public CDBContext(DbContextOptions options, IConfiguration config) : base(options)
    {
        Config = config;
        Database.EnsureCreated();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(Config["DBConnectionString"]);
    }
    public string GetOptionsValue(string key)
    {
        foreach (COption opt in Options) if (opt.Key == key) return opt.Value;

        return "";
    }
}