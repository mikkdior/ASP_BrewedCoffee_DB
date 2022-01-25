namespace ASP_BrewedCoffee_DB.Models;
public static class CHelper
{
    public static int GetMonthNum(string id, CMenu menu)
    {
        int month = 1; // filler
        CMenu menu_months = new CMenu();

        foreach (CMenuItem item in menu)
        {
            if (item.Title != "Old")
                menu_months.Add(item);
        }

        for (int i = 0; i < menu_months.Count; i++)

            if (menu_months[i].Url == id)
            {
                month = ++i;
                break;
            }

        return month;
    }
    public static DateTime ParseDate(string date)
    {
        string[] parsed = date.Split('.');
        return new DateTime(int.Parse(parsed[2]), int.Parse(parsed[1]), int.Parse(parsed[0]));
    }
    public static CMenu SortArchive(CMenu menu)
    {
        CMenuItem old = menu[0];
        CMenuItem curr_month = menu[DateTime.Now.Month];
        menu.Remove(old);

        IEnumerable<CMenuItem> items = menu.Skip(DateTime.Now.Month - 1).Concat(menu.Take(DateTime.Now.Month - 1));

        CMenu sorted = new CMenu() { Title = menu.Title, ShowCount = menu.ShowCount};
        foreach (CMenuItem item in items) sorted.Add(item);

        sorted.Remove(curr_month);
        sorted.Reverse();
        sorted.Insert(0, curr_month);
        sorted.Add(old);

        return sorted;
    }
    public static string? GetPathFromConfig(string key)
    {
        string[] lines = File.ReadAllLines(CConfService.ConfigPath);

        foreach (string line in lines)
        {
            string[] parts = line.Split(';');
            if (parts[0] == key) return parts[1];
        }

        return null;
    }
    public static int ValidatePage(int page, int all_posts_num, int num_per_page)
    {
        int max_pages = (int)Math.Ceiling(decimal.Divide(all_posts_num, num_per_page));
        if (page > max_pages) page = max_pages;
        else if (page < 1) page = 1;

        return page;
    }
    public static string CropString(string text, int max_len) => text.Length < max_len ? text : text[0..max_len] + "...";
}