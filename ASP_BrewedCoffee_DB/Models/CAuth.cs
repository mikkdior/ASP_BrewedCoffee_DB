namespace ASP_BrewedCoffee_DB.Models
{
    public class CAuth
    {
        public CConf ConfModel; 
        public CAuth(CConf conf)
        {
            ConfModel = conf;
        }
    }
    public class CAuthData
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
