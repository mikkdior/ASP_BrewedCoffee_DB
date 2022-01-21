namespace ASP_BrewedCoffee_DB.Models
{
    public class CCsvProvider : ConfigurationProvider
    {
        public override void Load()
        {
            string[] lines = File.ReadAllLines(CHelper.GetPathFromConfig("csv_provider_data_path"));

            Data = new Dictionary<string, string>();
            foreach (string line in lines)
            {
                string[] parts = line.Split(';');
                Data.Add(parts[0], line.Substring(parts[0].Length + 1));
            }
        }
    }
    public class CSVConfigurationSource : IConfigurationSource
    {
        public IConfigurationProvider Build(IConfigurationBuilder builder) => new CCsvProvider();
    }
    public static class CSVConfigurationExt
    {
        public static IConfigurationBuilder AddCSVProviderData(this IConfigurationBuilder self)
        {
            self.Add(new CSVConfigurationSource());

            return self;
        }
    }
}
