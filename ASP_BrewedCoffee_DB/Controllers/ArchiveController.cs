namespace ASP_BrewedCoffee.Controllers;
public class ArchiveController : Controller
{
    public IActionResult Index(string id, int page = 1)
    {
        /*int count = int.Parse(COptions.GetValue("PostsPerPage"));
        var menu = new CMenuFactoryM().Create(new CBuildArchiveStrategyM(), COptions.GetValue("ArchMenuTitle"), true);
        DateTime[] dates = CHelperM.GetDates("/archive/" + id, menu);
        HttpContext.Items.Add("Posts", new CPostsBuilderM().SetNum(count).SetMaxDate(dates[1]).SetMinDate(dates[0]).SetShift(page, count).Build());
        HttpContext.Items.Add("PostsPerPage", count);*/

        return View();
    }
}
