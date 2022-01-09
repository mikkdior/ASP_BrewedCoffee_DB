namespace ASP_BrewedCoffee_DB.Controllers;
public class HomeController : Controller
{
    public CPosts PostsModel;
    public CCategories CategoriesModel;
    public HomeController(CPosts posts, CCategories cats)
    {
        PostsModel = posts;
        CategoriesModel = cats;
    }
    public IActionResult Index()
    {
        return View();
    }
    [Route("/{slug}")]
    public IActionResult Category(string slug)
    {

        return View();
    }
    [Route("/archive/{month}")]
    public IActionResult Archive(string month)
    {

        return View();
    }
}
