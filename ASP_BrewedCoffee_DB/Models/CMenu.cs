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
    public CMenu Create(IBuildMenuStrategy strategy, string menu_title, bool show_count, string groups_slug)
        => strategy.GetMenuData(menu_title, show_count, groups_slug);
}
public class CBuildCategoryStrategy : IBuildMenuStrategy
{
    public CMenu GetMenuData(string menu_title, bool show_count, string groups_slug)
    {
        var menu = new CMenu() { Title = menu_title, ShowCount = show_count };
        string m_title = menu_title.ToLower();

        foreach (CCategory cat in CConfService.DB.Categories)
        {
            var m_item = new CMenuItem() 
            { 
                Id = cat.Id, 
                Title = cat.Title, 
                Url = $"/{groups_slug}/{cat.Slug}",
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
    public CMenu GetMenuData(string menu_title, bool show_count, string groups_slug)
    {
        var menu = new CMenu()
        {
            new CMenuItem(){ Title = "Old", Url = $"/{groups_slug}/old", Slug = "old" },
            new CMenuItem(){ Title = "January", Url = $"/{groups_slug}/jan", Slug = "jan" },
            new CMenuItem(){ Title = "February", Url = $"/{groups_slug}/feb", Slug = "feb" },
            new CMenuItem(){ Title = "March", Url = $"/{groups_slug}/mar", Slug = "mar" },
            new CMenuItem(){ Title = "April", Url = $"/{groups_slug}/apr", Slug = "apr" },
            new CMenuItem(){ Title = "May", Url = $"/{groups_slug}/may", Slug = "may" },
            new CMenuItem(){ Title = "June", Url = $"/{groups_slug}/jun", Slug = "jun" },
            new CMenuItem(){ Title = "July", Url = $"/{groups_slug}/jul", Slug = "jul" },
            new CMenuItem(){ Title = "August", Url = $"/{groups_slug}/aug", Slug = "aug" },
            new CMenuItem(){ Title = "September", Url = $"/{groups_slug}/sep", Slug = "sep" },
            new CMenuItem(){ Title = "October", Url = $"/{groups_slug}/oct", Slug = "oct" },
            new CMenuItem(){ Title = "November", Url = $"/{groups_slug}/nov", Slug = "nov" },
            new CMenuItem(){ Title = "December", Url = $"/{groups_slug}/dec", Slug = "dec" },
        };

        menu.Title = menu_title;
        menu.ShowCount = show_count;

        foreach (CMenuItem item in menu) item.Count = (item.Title == "Old") ? 
                GetOldsCount(CConfService.DB.Posts) : GetCount(item, CConfService.DB.Posts);

        return menu;
    }
    public int GetCount(CMenuItem menu_item, IEnumerable<CPost> posts) =>
        (from post in posts
            where post.CreatedDate > DateTime.Now.AddYears(-1)
            where ((CConfService.EMonths)post.CreatedDate.Month).ToString() == menu_item.Title
            where post.CreatedDate.Month == DateTime.Now.Month ? post.CreatedDate.Year == DateTime.Now.Year : true
            select post).Count();
    public int GetOldsCount(IEnumerable<CPost> posts) =>
        (from post in posts
            where post.CreatedDate < DateTime.Now.AddYears(-1).AddMonths(1).AddDays(-(DateTime.Now.Day - 1)).Date
            select post).Count();
}
