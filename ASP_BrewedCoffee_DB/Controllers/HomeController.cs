namespace ASP_BrewedCoffee_DB.Controllers;
public class HomeController : Controller
{
    public CPostsService PostsService;
    public CDBContext DB;
    public HomeController(CPostsService posts, CDBContext db)
    {
        PostsService = posts;
        DB = db;
    }
    public IActionResult Index()
    {
        int num = int.Parse(DB.GetOptionsValue("PostsOnHome"));
        //------------------------------------------------
        HttpContext.Items.Add("CategoriesMenu", new CMenuFactory().Create(new CBuildCategoryStrategy(), DB.GetOptionsValue("CatMenuTitle"), true));
        HttpContext.Items.Add("ArchiveMenu", CHelper.SortArchive(new CMenuFactory().Create(new CBuildArchiveStrategy(), DB.GetOptionsValue("ArchMenuTitle"), true)));
        HttpContext.Items.Add("CurrentPosts", PostsService.GetPosts().Take(num));

        return View();
    }
    public IActionResult Category(string slug, int page = 1)
    {
        int num = int.Parse(DB.GetOptionsValue("PostsPerPage"));
        var cat_menu = new CMenuFactory().Create(new CBuildCategoryStrategy(), DB.GetOptionsValue("CatMenuTitle"), true);
        int cat_id = CCategoriesService.GetCatID(cat_menu, slug);
        IEnumerable<CPost> all_filtered_posts = PostsService.GetPosts(cat_id);
        //------------------------------------------------
        HttpContext.Items.Add("CategoriesMenu", cat_menu);
        HttpContext.Items.Add("ArchiveMenu", CHelper.SortArchive(new CMenuFactory().Create(new CBuildArchiveStrategy(), DB.GetOptionsValue("ArchMenuTitle"), true)));
        HttpContext.Items.Add("PostsPerPage", num);
        HttpContext.Items.Add("CurrentPosts", PostsService.GetCurrentPosts(all_filtered_posts, page, num));
        HttpContext.Items.Add("AllFilteredPostsNum", all_filtered_posts.Count());



        return View();
    }
    public IActionResult Archive(string month, int page = 1)
    {
        int num = int.Parse(DB.GetOptionsValue("PostsPerPage"));
        string arch_menu_title = DB.GetOptionsValue("ArchMenuTitle");
        var arch_menu = new CMenuFactory().Create(new CBuildArchiveStrategy(), arch_menu_title, true);
        var sorted_arch_menu = CHelper.SortArchive(arch_menu);
        IEnumerable<CPost> all_filtered_posts = month == "old" ? PostsService.GetOldPosts() : PostsService.GetArchivePosts(month, arch_menu_title, arch_menu);
        //------------------------------------------------
        HttpContext.Items.Add("CategoriesMenu", new CMenuFactory().Create(new CBuildCategoryStrategy(), DB.GetOptionsValue("CatMenuTitle"), true));
        HttpContext.Items.Add("ArchiveMenu", sorted_arch_menu);
        HttpContext.Items.Add("PostsPerPage", num);
        HttpContext.Items.Add("CurrentPosts", PostsService.GetCurrentPosts(all_filtered_posts, page, num));
        HttpContext.Items.Add("AllFilteredPostsNum", all_filtered_posts.Count());

        return View();
    }
    public IActionResult Favorites(int page = 1)
    {
        int num = int.Parse(DB.GetOptionsValue("PostsPerPage"));
        IEnumerable<CPost> all_filtered_posts = PostsService.GetFavoritePosts(HttpContext);
        //------------------------------------------------
        HttpContext.Items.Add("CategoriesMenu", new CMenuFactory().Create(new CBuildCategoryStrategy(), DB.GetOptionsValue("CatMenuTitle"), true));
        HttpContext.Items.Add("ArchiveMenu", CHelper.SortArchive(new CMenuFactory().Create(new CBuildArchiveStrategy(), DB.GetOptionsValue("ArchMenuTitle"), true)));
        HttpContext.Items.Add("PostsPerPage", num);
        HttpContext.Items.Add("CurrentPosts", PostsService.GetCurrentPosts(all_filtered_posts, page, num));
        HttpContext.Items.Add("AllFilteredPostsNum", all_filtered_posts.Count());

        return View();
    }
    public IActionResult Page404() => View();
}
