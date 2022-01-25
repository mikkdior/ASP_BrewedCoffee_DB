namespace ASP_BrewedCoffee_DB.Models
{
    public class CDBProvider : ConfigurationProvider
    {
        public CDBContext DB = CConfService.DB;
        public override void Load()
        {
            Data = new Dictionary<string, string>(); 
            foreach (var route in DB.Routes) Data.Add("route_" + route.Name, route.Template);
            foreach (var option in DB.Options) Data.Add("option_" + option.Key, option.Value);
        }
    }
    public class DBConfigurationSource : IConfigurationSource
    {
        public IConfigurationProvider Build(IConfigurationBuilder builder) => new CDBProvider();
    }
    public static class DBConfigurationExt
    {
        public static IConfigurationBuilder AddDBProviderData(this IConfigurationBuilder self)
        {
            self.Add(new DBConfigurationSource());

            return self;
        }
    }
}
