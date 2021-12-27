namespace ASP_BrewedCoffee_DB.Models;
public static class CConf
{
    public enum EMonths
    {
        January = 1,
        February,
        March,
        April,
        May,
        June,
        July,
        August,
        September,
        October,
        November,
        December
    }
    public static IDataAccessFactory DataAccessFactory { get; set; }
    public static string CategoriesDataPath = "data/Categories.csv";
    public static string PostsDataPath = "data/Posts.csv";
    public static string OptionsDataPath = "data/Options.csv";
    public static string RoutesPath = "data/Routes.json";
    public static int CountFilteredPosts = 6;
    public static string MasterPass = "2";
    public static string MasterLogin = "1";
    public static string WebRootPath;
    public static CDBContext? DB;
    public static string DbConnString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=BrewedCoffeeDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
    //public static CancellationTokenSource CancellationToken = new CancellationTokenSource();
}