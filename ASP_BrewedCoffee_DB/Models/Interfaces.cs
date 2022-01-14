namespace ASP_BrewedCoffee_DB.Models;
public interface IBuildMenuStrategy
{
    CMenu GetMenuData(string menu_title, bool show_count);
    int GetCount(CMenuItem menu_item, IEnumerable<CPost> posts);
}
