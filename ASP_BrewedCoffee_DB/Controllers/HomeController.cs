namespace ASP_BrewedCoffee_DB.Controllers;
public class HomeController : Controller
{
    public IActionResult Index()
    {
        HttpContext.Items.Add("CategoriesMenu", new CMenuFactoryM().Create(new CBuildCategoryStrategyM(), COptions.GetValue("CatMenuTitle"), true));
        HttpContext.Items.Add("ArchiveMenu", new CMenuFactoryM().Create(new CBuildArchiveStrategyM(), COptions.GetValue("ArchMenuTitle"), true));
        HttpContext.Items.Add("Posts", new CPostsBuilderM().SetNum(int.Parse(COptions.GetValue("PostsOnHome"))).Build());

        return View();
    }

    /*public IActionResult Index()
    {
        int count = int.Parse(COptions.GetValue("PostsOnHome"));
        CPostsM posts = new CPostsBuilderM().SetNum(count).Build();
        HttpContext.Items.Add("Posts", posts);

        return View();
    }*/
    public IActionResult Page404()
    {
        return Content("page not found");
    }
}
