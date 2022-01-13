namespace ASP_BrewedCoffee_DB.Models;
public static class CHelper
{
    public static DateTime[] GetDates(string id, CMenu menu)
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
    public static string GetCatName(IEnumerable<CCategory> cats, int id)
    {
        foreach (CCategory cat in cats) if (cat.Id == id) return cat.Title;

        return "";
    }
    public static int GetCatID(CMenu menu, string cat_name)
    {
        foreach (CMenuItem item in menu) if (item.Title == cat_name) return item.ID;

        return -1;
    }
}
