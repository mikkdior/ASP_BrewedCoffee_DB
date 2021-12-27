namespace ASP_BrewedCoffee.Controllers;
public class HomeController : Controller
{
    public IActionResult Index()
    {
        /*int count = int.Parse(COptions.GetValue("PostsOnHome"));
        CPostsM posts = new CPostsBuilderM().SetNum(count).Build();
        HttpContext.Items.Add("Posts", posts);*/

        return View();
    }
    public IActionResult Page404()
    {
        return Content("page not found");
    }
}
