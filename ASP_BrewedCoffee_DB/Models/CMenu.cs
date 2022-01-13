using Microsoft.EntityFrameworkCore;
namespace ASP_BrewedCoffee_DB.Models;
public class CMenuItem
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Url { get; set; }
    public int Count { get; set; }
}
public class CMenu : List<CMenuItem>
{
    public string? Title { get; set; }
    public bool ShowCount { get; set; }
}
public class CMenuFactory
{
    public CMenu Create(IBuildMenuStrategy strategy, string menu_title, bool show_count)
        => strategy.GetMenuData(menu_title, show_count);
}
public class CBuildCategoryStrategy : IBuildMenuStrategy
{
    public CMenu GetMenuData(string menu_title, bool show_count)
    {
        var menu = new CMenu() { Title = menu_title, ShowCount = show_count };
        string m_title = menu_title.ToLower();

        foreach (CCategory cat in CConf.DB.Categories)
        {
            var m_item = new CMenuItem() 
            { 
                Id = cat.Id, 
                Title = cat.Title, 
                Url = $"/{m_title}/{cat.Slug}" 
            };
            if (show_count) m_item.Count = GetCount(m_item, CConf.DB.Posts);
            menu.Add(m_item);
        }
        
        return menu;
    }
    public int GetCount(CMenuItem menu_item, DbSet<CPost> posts) =>
        (from post in posts where post.CategoryId == menu_item.Id select post).Count();
}
public class CBuildArchiveStrategy : IBuildMenuStrategy
{
    public CMenu GetMenuData(string menu_title, bool show_count)
    {
        string m_title = menu_title.ToLower();
        var menu = new CMenu()
        {
            new CMenuItem(){ Title = "January", Url = $"/{m_title}/jan" },
            new CMenuItem(){ Title = "February", Url = $"/{m_title}/feb" },
            new CMenuItem(){ Title = "March", Url = $"/{m_title}/mar" },
            new CMenuItem(){ Title = "April", Url = $"/{m_title}/apr" },
            new CMenuItem(){ Title = "May", Url = $"/{m_title}/may" },
            new CMenuItem(){ Title = "June", Url = $"/{m_title}/jun" },
            new CMenuItem(){ Title = "July", Url = $"/{m_title}/jul" },
            new CMenuItem(){ Title = "August", Url = $"/{m_title}/aug" },
            new CMenuItem(){ Title = "September", Url = $"/{m_title}/sep" },
            new CMenuItem(){ Title = "October", Url = $"/{m_title}/oct" },
            new CMenuItem(){ Title = "November", Url = $"/{m_title}/nov" },
            new CMenuItem(){ Title = "December", Url = $"/{m_title}/dec" },
        };

        menu.Title = menu_title;
        menu.ShowCount = show_count;

        if (show_count) foreach (CMenuItem item in menu) item.Count = GetCount(item, CConf.DB.Posts);

        return menu;
    }

    public int GetCount(CMenuItem menu_item, DbSet<CPost> posts) => 5;
    /*{
        return CFilesM.Instance.GetCount(CConf.PostsDataPath, new CFilterOptionsBuilderM().AddFilter((table_line) =>
        {
            int curr_month_num = CHelperM.ParseDate(table_line["Date"]).Month;
            string curr_month_name = ((CConf.EMonths)curr_month_num).ToString();

            if (curr_month_name == menu_item.Title) return true;
            return false;
        }).Build());
    }*/
}
