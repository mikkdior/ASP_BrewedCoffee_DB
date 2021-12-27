namespace ASP_BrewedCoffee_v2.Models;
public static class COptions
{
    public static string GetValue(string key)
    {
        var f_options = new CFilterOptionsBuilderM().AddFilter((line) => line["Key"] == key).SetLimit(1).Build();

        if (CConf.DB != null) return CFilesM.Instance.GetData(CConf.DB.Options, f_options).Result[0]["Value"];

        return CFilesM.Instance.GetData(CConf.OptionsDataPath, f_options).Result[0]["Value"];
    }
}
