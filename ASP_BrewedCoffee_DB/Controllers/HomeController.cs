using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
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
    public IActionResult Index() => View(new CHomeViewModel()
        {
            CategoriesMenu = new CMenuFactory().Create(new CBuildCategoryStrategy(), Config["option_CatMenuTitle"], true),
            ArchiveMenu = CHelper.SortArchive(new CMenuFactory().Create(new CBuildArchiveStrategy(), Config["option_ArchMenuTitle"], true)),
            CurrentPosts = PostsService.GetPosts().Take(int.Parse(Config["option_PostsOnHome"]))
        });
    public IActionResult Category(string slug, int page = 1) 
    {
        int num = int.Parse(Config["option_PostsPerPage"]);
        var cat_menu = new CMenuFactory().Create(new CBuildCategoryStrategy(), Config["option_CatMenuTitle"], true);
        int cat_id = CCategoriesService.GetCatID(cat_menu, slug);
        IEnumerable<CPost> all_filtered_posts = PostsService.GetPosts(cat_id);
        int all_filtered_posts_num = all_filtered_posts.Count();
        page = CHelper.ValidatePage(page, all_filtered_posts_num, num);
        //--------------------------------------------
        return View(new CHomeViewModel() 
        {
            CategoriesMenu = cat_menu,
            ArchiveMenu = CHelper.SortArchive(new CMenuFactory().Create(new CBuildArchiveStrategy(), Config["option_ArchMenuTitle"], true)),
            PostsPerPage = num,
            Page = page,
            CurrentPosts = PostsService.GetCurrentPosts(all_filtered_posts, page, num),
            AllFilteredPostsNum = all_filtered_posts_num
        });
    }
    public IActionResult Archive(string month, int page = 1)
    {
        int num = int.Parse(Config["option_PostsPerPage"]);
        string arch_menu_title = Config["option_ArchMenuTitle"];
        var arch_menu = new CMenuFactory().Create(new CBuildArchiveStrategy(), arch_menu_title, true);
        IEnumerable<CPost> all_filtered_posts = month == arch_menu[0].Slug ? PostsService.GetOldPosts() : PostsService.GetArchivePosts(month, arch_menu_title, arch_menu);
        var sorted_arch_menu = CHelper.SortArchive(arch_menu);
        int all_filtered_posts_num = all_filtered_posts.Count();
        page = CHelper.ValidatePage(page, all_filtered_posts_num, num);
        //------------------------------------------------
        return View(new CHomeViewModel()
        {
            CategoriesMenu = new CMenuFactory().Create(new CBuildCategoryStrategy(), Config["option_CatMenuTitle"], true),
            ArchiveMenu = sorted_arch_menu,
            PostsPerPage = num,
            Page = page,
            CurrentPosts = PostsService.GetCurrentPosts(all_filtered_posts, page, num),
            AllFilteredPostsNum = all_filtered_posts_num
        });
    }
    public IActionResult Favorites(int page = 1)
    {
        int num = int.Parse(Config["option_PostsPerPage"]);
        IEnumerable<CPost> all_filtered_posts = PostsService.GetFavoritePosts(HttpContext);
        int all_filtered_posts_num = all_filtered_posts.Count();
        page = CHelper.ValidatePage(page, all_filtered_posts_num, num);
        //------------------------------------------------
        return View(new CHomeViewModel()
        {
            CategoriesMenu = new CMenuFactory().Create(new CBuildCategoryStrategy(), Config["option_CatMenuTitle"], true),
            ArchiveMenu = CHelper.SortArchive(new CMenuFactory().Create(new CBuildArchiveStrategy(), Config["option_ArchMenuTitle"], true)),
            PostsPerPage = num,
            Page = page,
            CurrentPosts = PostsService.GetCurrentPosts(all_filtered_posts, page, num),
            AllFilteredPostsNum = all_filtered_posts.Count()
        });
    }
    [HttpGet]
    public IActionResult LikeAction(string postid, string acttype)
    {
        if (postid == null || acttype == null) return Content("");
        CPost? post = PostsService.SetLikes(postid, acttype);
        if (post == null) return Content("");
        CHelper.RegSessionData("likes", HttpContext, postid);

        return Content(post.Likes.ToString());
    }
    [HttpGet]
    public IActionResult FavoriteAction(string postid)
    {
        if (postid == null) return Content("");
        CHelper.RegSessionData("favorites", HttpContext, postid);

        return Content("");
    }
    [HttpGet]
    public IActionResult HideAction(string title)
    {
        if (title == null) return Content("");
        CHelper.RegSessionData("hidden", HttpContext, title);

        return Content("");
    }
    [HttpGet]
    public IActionResult UseSessionData(string key, string value = "")
    {
        CHelper.RegSessionData(key, HttpContext, value);

        return Content(HttpContext.Session.GetString(key));
    }

    public IActionResult Page404() => View();
}
