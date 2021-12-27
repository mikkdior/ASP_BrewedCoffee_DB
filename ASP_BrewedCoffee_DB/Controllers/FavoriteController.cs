namespace ASP_BrewedCoffee_DB.Controllers;
public class FavoriteController : Controller
{
    public IActionResult Index(int page = 1)
    {
        int count = int.Parse(COptions.GetValue("PostsPerPage"));
        var names = (List<string>)HttpContext.Items["favorites"];
        HttpContext.Items.Add("Posts", new CPostsBuilderM().SetNum(count).SetShift(page, count).SetPostNames(names.ToArray()).Build());
        HttpContext.Items.Add("PostsPerPage", count);

        return View();
    }




    /*public IActionResult Index(int page = 1)
    {
        int count = int.Parse(COptions.GetValue("PostsPerPage"));
        var names = (List<string>)HttpContext.Items["favorites"];
        HttpContext.Items.Add("Posts", new CPostsBuilderM().SetNum(count).SetShift(page, count).SetPostNames(names.ToArray()).Build());
        HttpContext.Items.Add("PostsPerPage", count);

        return View();
    }*/
}
