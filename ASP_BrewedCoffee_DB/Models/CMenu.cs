namespace ASP_BrewedCoffee_DB.Models;
public class CMenuItem
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Url { get; set; }
    public string Slug { get; set; }
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

        foreach (CCategory cat in CConfService.DB.Categories)
        {
            var m_item = new CMenuItem() 
            { 
                Id = cat.Id, 
                Title = cat.Title, 
                Url = $"/{m_title}/{cat.Slug}",
                Slug = cat.Slug
            };
            menu.Add(m_item);
        }
        if (show_count) foreach (CMenuItem item in menu) item.Count = GetCount(item, CConfService.DB.Posts);
        
        return menu;
    }
    public int GetCount(CMenuItem menu_item, IEnumerable<CPost> posts) =>
        (from post in posts
        where post.CategoryId == menu_item.Id
        select post).Count();
}
public class CBuildArchiveStrategy : IBuildMenuStrategy
{
    public CMenu GetMenuData(string menu_title, bool show_count)
    {
        string m_title = menu_title.ToLower();
        var menu = new CMenu()
        {
            new CMenuItem(){ Title = "Old", Url = $"/{m_title}/old", Slug = "old" },
            new CMenuItem(){ Title = "January", Url = $"/{m_title}/jan", Slug = "jan" },
            new CMenuItem(){ Title = "February", Url = $"/{m_title}/feb", Slug = "feb" },
            new CMenuItem(){ Title = "March", Url = $"/{m_title}/mar", Slug = "mar" },
            new CMenuItem(){ Title = "April", Url = $"/{m_title}/apr", Slug = "apr" },
            new CMenuItem(){ Title = "May", Url = $"/{m_title}/may", Slug = "may" },
            new CMenuItem(){ Title = "June", Url = $"/{m_title}/jun", Slug = "jun" },
            new CMenuItem(){ Title = "July", Url = $"/{m_title}/jul", Slug = "jul" },
            new CMenuItem(){ Title = "August", Url = $"/{m_title}/aug", Slug = "aug" },
            new CMenuItem(){ Title = "September", Url = $"/{m_title}/sep", Slug = "sep" },
            new CMenuItem(){ Title = "October", Url = $"/{m_title}/oct", Slug = "oct" },
            new CMenuItem(){ Title = "November", Url = $"/{m_title}/nov", Slug = "nov" },
            new CMenuItem(){ Title = "December", Url = $"/{m_title}/dec", Slug = "dec" },
        };

        menu.Title = menu_title;
        menu.ShowCount = show_count;

        foreach (CMenuItem item in menu) item.Count = (item.Title == "Old") ? 
                GetOldsCount(CConfService.DB.Posts) : GetCount(item, CConfService.DB.Posts);

        return menu;
    }
    public int GetCount(CMenuItem menu_item, IEnumerable<CPost> posts) => 
        (from post in posts
        where post.CreatedDate.Year == DateTime.Now.Year
        where ((CConfService.EMonths)post.CreatedDate.Month).ToString() == menu_item.Title 
        select post).Count();
    public int GetOldsCount(IEnumerable<CPost> posts) =>
        (from post in posts
         where post.CreatedDate.Year < DateTime.Now.Year
         select post).Count();
}
