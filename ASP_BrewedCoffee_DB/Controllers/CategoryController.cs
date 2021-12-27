namespace ASP_BrewedCoffee.Controllers;
public class CategoryController : Controller
{
    //[Route("/category/{id}")]
    public IActionResult Index(string id, int page = 1)
    {
        /*int count = int.Parse(COptions.GetValue("PostsPerPage"));
        HttpContext.Items.Add("Posts", new CPostsBuilderM().SetNum(count).SetCatName(id).SetShift(page, count).Build());
        HttpContext.Items.Add("PostsPerPage", count);*/

        return View();
    }
}
