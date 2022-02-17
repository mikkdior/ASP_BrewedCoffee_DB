namespace ASP_BrewedCoffee_DB.Models;
public interface IBuildMenuStrategy
{
    CMenu GetMenuData(string menu_title, bool show_count, string groups_slug);
    int GetCount(CMenuItem menu_item, IEnumerable<CPost> posts);
}
