namespace ASP_BrewedCoffee_DB.Controllers;
public class HomeController : Controller
{
    public CPostsService PostsModel;
    public CCategoriesService CategoriesModel;
    public CDBContext DB;
    private IConfiguration Config;
    public HomeController(CPostsService posts, CCategoriesService cats, CDBContext db, IConfiguration config)
    {
        PostsModel = posts;
        CategoriesModel = cats;
        DB = db;
        Config = config;
    }
    public IActionResult Index()
    {
        int num = int.Parse(CConfService.DB.GetOptionsValue("PostsOnHome"));
        //------------------------------------------------
        HttpContext.Items.Add("CategoriesMenu", new CMenuFactory().Create(new CBuildCategoryStrategy(), Config["CatMenuTitle"], true));
        HttpContext.Items.Add("ArchiveMenu", CHelper.SortArchive(new CMenuFactory().Create(new CBuildArchiveStrategy(), Config["ArchMenuTitle"], true)));
        HttpContext.Items.Add("Posts", DB.Posts
            .OrderBy(post => post.CreatedDate)
            .Take(num));

        return View();
    }
    public IActionResult Category(string slug, int page = 1)
    {
        int num = int.Parse(CConfService.DB.GetOptionsValue("PostsPerPage"));
        var cat_menu = new CMenuFactory().Create(new CBuildCategoryStrategy(), Config["CatMenuTitle"], true);
        int cat_id = CCategoriesService.GetCatID(cat_menu, slug);
        //------------------------------------------------
        HttpContext.Items.Add("CategoriesMenu", cat_menu);
        HttpContext.Items.Add("ArchiveMenu", CHelper.SortArchive(new CMenuFactory().Create(new CBuildArchiveStrategy(), Config["ArchMenuTitle"], true)));
        HttpContext.Items.Add("PostsPerPage", num);
        HttpContext.Items.Add("Posts", DB.Posts
            .OrderBy(post => post.CreatedDate)
            .Where(post => post.CategoryId == cat_id)
            .Skip((--page) * num)
            .Take(num));

        return View();
    }
    public IActionResult Archive(string month, int page = 1)
    {
        int num = int.Parse(CConfService.DB.GetOptionsValue("PostsPerPage"));
        string arch_menu_title = CConfService.DB.GetOptionsValue("ArchMenuTitle");
        var arch_menu = new CMenuFactory().Create(new CBuildArchiveStrategy(), arch_menu_title, true);
        var sorted_arch_menu = CHelper.SortArchive(arch_menu);
        //------------------------------------------------
        HttpContext.Items.Add("CategoriesMenu", new CMenuFactory().Create(new CBuildCategoryStrategy(), Config["CatMenuTitle"], true));
        HttpContext.Items.Add("ArchiveMenu", sorted_arch_menu);
        HttpContext.Items.Add("PostsPerPage", num);
        IEnumerable<CPost> posts;

        DateTime[] dates;
        if (month == "old")
        {
            posts = DB.Posts
            .OrderBy(post => post.CreatedDate)
            .Where(post => post.CreatedDate.Year < DateTime.Now.Year)
            .Skip((--page) * num)
            .Take(num);
        }
        else
        {
            dates = CHelper.GetDates($"/{arch_menu_title.ToLower()}/" + month, arch_menu);
            posts = DB.Posts
            .OrderBy(post => post.CreatedDate)
            .Where(post => post.CreatedDate >= dates[0])
            .Where(post => post.CreatedDate <= dates[1])
            .Skip((--page) * num)
            .Take(num);
        }
        HttpContext.Items.Add("Posts", posts);

        return View();
    }
    public IActionResult Favorites(int page = 1)
    {
        int num = int.Parse(DB.GetOptionsValue("PostsPerPage"));
        //------------------------------------------------
        HttpContext.Items.Add("CategoriesMenu", new CMenuFactory().Create(new CBuildCategoryStrategy(), Config["CatMenuTitle"], true));
        HttpContext.Items.Add("ArchiveMenu", CHelper.SortArchive(new CMenuFactory().Create(new CBuildArchiveStrategy(), Config["ArchMenuTitle"], true)));
        HttpContext.Items.Add("Posts", DB.Posts
            .OrderBy(post => post.CreatedDate)
            .Where(post => ((List<string>)HttpContext.Items["favorites"]).Contains(post.Id.ToString()))
            .Skip((--page) * num)
            .Take(num));

        return View();
    }
    public IActionResult Page404() => View();
}
