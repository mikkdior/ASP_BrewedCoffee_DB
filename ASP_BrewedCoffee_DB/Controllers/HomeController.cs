using Microsoft.EntityFrameworkCore;

namespace ASP_BrewedCoffee_DB.Controllers;
public class HomeController : Controller
{
    public CPosts PostsModel;
    public CCategories CategoriesModel;
    public CDBContext DB;
    public HomeController(CPosts posts, CCategories cats, CDBContext db)
    {
        PostsModel = posts;
        CategoriesModel = cats;
        DB = db;
    }
    public IActionResult Index()
    {
        int num = int.Parse(CConf.DB.GetOptionsValue("PostsOnHome"));
        //------------------------------------------------
        HttpContext.Items.Add("CategoriesMenu", new CMenuFactory().Create(new CBuildCategoryStrategy(), CConf.DB.GetOptionsValue("CatMenuTitle"), true));
        HttpContext.Items.Add("ArchiveMenu", new CMenuFactory().Create(new CBuildArchiveStrategy(), CConf.DB.GetOptionsValue("ArchMenuTitle"), true));
        HttpContext.Items.Add("Posts", DB.Posts
            .OrderBy(post => post.CreatedDate)
            .Take(num));
        

        return View();
    }
    [Route("/categories/{slug}?{page?}")]
    public IActionResult Category(string slug, int page = 1)
    {
        int num = int.Parse(CConf.DB.GetOptionsValue("PostsPerPage"));
        var cat_menu = new CMenuFactory().Create(new CBuildCategoryStrategy(), CConf.DB.GetOptionsValue("CatMenuTitle"), true);
        int cat_id = CCategories.GetCatID(cat_menu, slug);
        //------------------------------------------------
        HttpContext.Items.Add("CategoriesMenu", cat_menu);
        HttpContext.Items.Add("ArchiveMenu", new CMenuFactory().Create(new CBuildArchiveStrategy(), CConf.DB.GetOptionsValue("ArchMenuTitle"), true));
        HttpContext.Items.Add("PostsPerPage", num);
        HttpContext.Items.Add("Posts", DB.Posts
            .OrderBy(post => post.CreatedDate)
            .Where(post => post.CategoryId == cat_id)
            .Skip((--page) * num)
            .Take(num));

        return View();
    }
    [Route("/archive/{month}?{page?}")]
    public IActionResult Archive(string month, int page = 1)
    {
        int num = int.Parse(CConf.DB.GetOptionsValue("PostsPerPage"));
        string arch_menu_title = CConf.DB.GetOptionsValue("ArchMenuTitle");
        var arch_menu = new CMenuFactory().Create(new CBuildArchiveStrategy(), arch_menu_title, true);
        DateTime[] dates = CHelper.GetDates($"/{arch_menu_title.ToLower()}/" + month, arch_menu);
        //------------------------------------------------
        HttpContext.Items.Add("CategoriesMenu", new CMenuFactory().Create(new CBuildCategoryStrategy(), CConf.DB.GetOptionsValue("CatMenuTitle"), true));
        HttpContext.Items.Add("ArchiveMenu", arch_menu);
        HttpContext.Items.Add("PostsPerPage", num);
        HttpContext.Items.Add("Posts", DB.Posts
            .OrderBy(post => post.CreatedDate)
            .Where(post => (dates[0] <= post.CreatedDate) && (dates[1] >= post.CreatedDate))
            .Skip((--page) * num)
            .Take(num));

        return View();
    }
}
