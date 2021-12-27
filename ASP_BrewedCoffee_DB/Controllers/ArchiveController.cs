namespace ASP_BrewedCoffee_DB.Controllers;
public class ArchiveController : Controller
{
    private CDBContext? DBContext;
    public ArchiveController(CDBContext? db_context = null) { DBContext = db_context; }
    //[Route("/archive/{id}")]
    public IActionResult Index(string month, int page = 1)
    {
        int count = int.Parse(COptions.GetValue("PostsPerPage"));
        var arch_menu = new CMenuFactoryM().Create(new CBuildArchiveStrategyM(), COptions.GetValue("ArchMenuTitle"), true);
        DateTime[] dates = CHelperM.GetDates("/archive/" + month, arch_menu);
        
        HttpContext.Items.Add("CategoriesMenu", new CMenuFactoryM().Create(new CBuildCategoryStrategyM(), COptions.GetValue("CatMenuTitle"), true));
        HttpContext.Items.Add("ArchiveMenu", arch_menu);
        HttpContext.Items.Add("Posts", new CPostsBuilderM().SetNum(count).SetMaxDate(dates[1]).SetMinDate(dates[0]).SetShift(page, count).Build());
        HttpContext.Items.Add("PostsPerPage", count);

        return View();
    }
    /*public IActionResult Index(string id, int page = 1)
    {
        int count = int.Parse(COptions.GetValue("PostsPerPage"));
        var menu = new CMenuFactoryM().Create(new CBuildArchiveStrategyM(), COptions.GetValue("ArchMenuTitle"), true);
        DateTime[] dates = CHelperM.GetDates("/archive/" + id, menu);
        HttpContext.Items.Add("Posts", new CPostsBuilderM().SetNum(count).SetMaxDate(dates[1]).SetMinDate(dates[0]).SetShift(page, count).Build());
        HttpContext.Items.Add("PostsPerPage", count);

        return View();
    }*/
}
