namespace ASP_BrewedCoffee_DB.Models;
public class CCategories
{
    private CDBContext DB;
    public CCategories(CDBContext db_context)
    {
        DB = db_context;
    }
    public IEnumerable<CCategory>? GetCats()
    {
        return DB.Categories;
    }
    public void Add(CCategory cat)
    {
        DB.Categories.Add(cat);
        DB.SaveChanges();
    }
    public void Edit(CCategory cat)
    {
        var category = DB.Categories.Find(cat.Id);
        category.Title = cat.Title;
        category.Slug = cat.Slug;
        DB.SaveChanges();
    }
    public CCategory GetCat(int? id) => DB.Categories.Find(id);
}