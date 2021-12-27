namespace ASP_BrewedCoffee_DB.Controllers;
public class CategoryController : Controller
{
    private CDBContext? DBContext;
    public CategoryController(CDBContext? db_context = null) { DBContext = db_context; }
    public IActionResult Index(string cat_name, int page = 1)
    {
        int count = int.Parse(COptions.GetValue("PostsPerPage"));
        CMenuM cat_menu = new CMenuFactoryM().Create(new CBuildCategoryStrategyM(), COptions.GetValue("CatMenuTitle"), true);

        CPostsModel? posts = DBContext != null ?
            new CPostsBuilderM().SetNum(count).SetCatID(CHelperM.GetCatID(cat_menu, cat_name)).SetShift(page, count).Build()
            : new CPostsBuilderM().SetNum(count).SetCatName(cat_name).SetShift(page, count).Build();

        HttpContext.Items.Add("CategoriesMenu", cat_menu);
        HttpContext.Items.Add("ArchiveMenu", new CMenuFactoryM().Create(new CBuildArchiveStrategyM(), COptions.GetValue("ArchMenuTitle"), true));
        HttpContext.Items.Add("Posts", new CPostsBuilderM().SetNum(count).Build());
        HttpContext.Items.Add("PostsPerPage", count);

        return View();
    }




    /*public IActionResult Index(string id, int page = 1)
    {
        int count = int.Parse(COptions.GetValue("PostsPerPage"));
        HttpContext.Items.Add("Posts", new CPostsBuilderM().SetNum(count).SetCatName(id).SetShift(page, count).Build());
        HttpContext.Items.Add("PostsPerPage", count);

        return View();
    }*/
}
