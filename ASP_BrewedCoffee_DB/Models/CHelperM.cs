namespace ASP_BrewedCoffee_v2.Models;
public static class CHelperM
{
    public static DateTime[] GetDates(string id, CMenuM menu)
    {
        int year = DateTime.Now.Year;
        int month = 1; // filler

        for (int i = 0; i < menu.Count; i++)
            if (menu[i].Url == id)
            {
                month = i + 1;
                break;
            }

        int max_days = DateTime.DaysInMonth(year, month);

        return new DateTime[2] 
        { 
            new DateTime(year, month, 1), //min
            new DateTime(year, month, max_days) //max
        };
    }
    public static DateTime ParseDate(string date)
    {
        string[] parsed = date.Split('.');
        return new DateTime(int.Parse(parsed[2]), int.Parse(parsed[1]), int.Parse(parsed[0]));
    }
    public static string GetCatName(IEnumerable<CDBCategoryM> cats, int id)
    {
        foreach (CDBCategoryM cat in cats) if (cat.Id == id) return cat.Title;

        return "";
    }
    public static int GetCatID(CMenuM menu, string cat_name)
    {
        foreach (CMenuItemM item in menu) if (item.Title == cat_name) return item.ID;

        return -1;
    }
}
