namespace ASP_BrewedCoffee_DB.Controllers;
public class HomeController : Controller
{
    public CPostsService PostsService;
    private IConfiguration Config;
    public HomeController(CPostsService posts, IConfiguration config)
    {
        PostsService = posts;
        Config = config;
    }
    public IActionResult Index()
    {
        int num = int.Parse(Config["option_PostsOnHome"]);
        //------------------------------------------------
        HttpContext.Items.Add("CategoriesMenu", new CMenuFactory().Create(new CBuildCategoryStrategy(), Config["option_CatMenuTitle"], true));
        HttpContext.Items.Add("ArchiveMenu", CHelper.SortArchive(new CMenuFactory().Create(new CBuildArchiveStrategy(), Config["option_ArchMenuTitle"], true)));
        HttpContext.Items.Add("CurrentPosts", PostsService.GetPosts().Take(num));

        return View();
    }
    public IActionResult Category(string slug, int page = 1)
    {
        int num = int.Parse(Config["option_PostsPerPage"]);
        var cat_menu = new CMenuFactory().Create(new CBuildCategoryStrategy(), Config["option_CatMenuTitle"], true);
        int cat_id = CCategoriesService.GetCatID(cat_menu, slug);
        IEnumerable<CPost> all_filtered_posts = PostsService.GetPosts(cat_id);
        int all_filtered_posts_num = all_filtered_posts.Count();
        page = CHelper.ValidatePage(page, all_filtered_posts_num, num);
        //------------------------------------------------
        HttpContext.Items.Add("CategoriesMenu", cat_menu);
        HttpContext.Items.Add("ArchiveMenu", CHelper.SortArchive(new CMenuFactory().Create(new CBuildArchiveStrategy(), Config["option_ArchMenuTitle"], true)));
        HttpContext.Items.Add("PostsPerPage", num);
        HttpContext.Items.Add("Page", page);
        HttpContext.Items.Add("CurrentPosts", PostsService.GetCurrentPosts(all_filtered_posts, page, num));
        HttpContext.Items.Add("AllFilteredPostsNum", all_filtered_posts_num);

        return View();
    }
    public IActionResult Archive(string month, int page = 1)
    {
        int num = int.Parse(Config["option_PostsPerPage"]);
        string arch_menu_title = Config["option_ArchMenuTitle"];
        var arch_menu = new CMenuFactory().Create(new CBuildArchiveStrategy(), arch_menu_title, true);
        var sorted_arch_menu = CHelper.SortArchive(arch_menu);
        IEnumerable<CPost> all_filtered_posts = month == "old" ? PostsService.GetOldPosts() : PostsService.GetArchivePosts(month, arch_menu_title, arch_menu);
        int all_filtered_posts_num = all_filtered_posts.Count();
        page = CHelper.ValidatePage(page, all_filtered_posts_num, num);
        //------------------------------------------------
        HttpContext.Items.Add("CategoriesMenu", new CMenuFactory().Create(new CBuildCategoryStrategy(), Config["option_CatMenuTitle"], true));
        HttpContext.Items.Add("ArchiveMenu", sorted_arch_menu);
        HttpContext.Items.Add("PostsPerPage", num);
        HttpContext.Items.Add("Page", page);
        HttpContext.Items.Add("CurrentPosts", PostsService.GetCurrentPosts(all_filtered_posts, page, num));
        HttpContext.Items.Add("AllFilteredPostsNum", all_filtered_posts_num);

        return View();
    }
    public IActionResult Favorites(int page = 1)
    {
        int num = int.Parse(Config["option_PostsPerPage"]);
        IEnumerable<CPost> all_filtered_posts = PostsService.GetFavoritePosts(HttpContext);
        int all_filtered_posts_num = all_filtered_posts.Count();
        page = CHelper.ValidatePage(page, all_filtered_posts_num, num);
        //------------------------------------------------
        HttpContext.Items.Add("CategoriesMenu", new CMenuFactory().Create(new CBuildCategoryStrategy(), Config["option_CatMenuTitle"], true));
        HttpContext.Items.Add("ArchiveMenu", CHelper.SortArchive(new CMenuFactory().Create(new CBuildArchiveStrategy(), Config["option_ArchMenuTitle"], true)));
        HttpContext.Items.Add("PostsPerPage", num);
        HttpContext.Items.Add("Page", page);
        HttpContext.Items.Add("CurrentPosts", PostsService.GetCurrentPosts(all_filtered_posts, page, num));
        HttpContext.Items.Add("AllFilteredPostsNum", all_filtered_posts.Count());

        return View();
    }
    public IActionResult Page404() => View();
}
