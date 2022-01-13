namespace ASP_BrewedCoffee_DB.Models;
public class CCategories
{
    private CDBContext DB = CConf.DB;
    public IEnumerable<CCategory>? GetCats()
    {
        return DB.Categories;
    }
    public void Add(CCategory cat)
    {
        DB.Categories.Add(cat);
        DB.SaveChanges();
    }
    public void Edit(CCategory cat, int id)
    {
        var category = DB.Categories.Find(id);
        category.Title = cat.Title;
        category.Slug = cat.Slug;
        DB.SaveChanges();
    }
    public CCategory GetCat(int? id) => DB.Categories.Find(id);
}