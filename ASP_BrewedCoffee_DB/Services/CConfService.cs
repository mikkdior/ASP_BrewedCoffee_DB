namespace ASP_BrewedCoffee_DB.Models;
public class CConfService
{
    public enum EMonths { January = 1, February, March, April, May, June, July, August, September, October, November, December }
    public static string WebRootPath;
    public static string ConfigPath = "config/config.csv";
    public static string DbConnString = "Server=(localdb)\\mssqllocaldb;Database=coffee_v02;Trusted_Connection=True;";
    public static CDBContext DB;
    public CConfService()
    {

    }
}