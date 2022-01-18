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
                month = ++i;
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
}
