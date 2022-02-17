using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ASP_BrewedCoffee_DB.Controllers;
public class AdminController : Controller
{
    public CPostsService PostsModel;
    public CAuthService AuthModel;
    public CCategoriesService CategoriesModel;
    private IConfiguration Config;
    public AdminController(CAuthService auth, CCategoriesService cats, CPostsService posts, IConfiguration config)
    {
        AuthModel = auth;
        PostsModel = posts;
        CategoriesModel = cats;
        Config = config;
    }
    [NonAction]
    public bool IsAdmin() => AuthModel.CheckAuth(HttpContext);
    public IActionResult Index() => IsAdmin() ? Redirect(Config["route_admin-posts"]) : View(true);
    // auth admin control post request
    [HttpPost]
    public IActionResult Index(RAdminData data) => AuthModel.Authorization(HttpContext, data) ? Redirect(Config["route_admin-posts"]) : View(false);
    public IActionResult LogOut() 
    {
        AuthModel.UnAuthorization(HttpContext);
        return Redirect(Config["route_admin"]);
    }
    public IActionResult Posts(int page = 1) 
    {
        IEnumerable<CPost> all_posts = PostsModel.GetPosts();

        return IsAdmin() ? View(new CAdminPostsViewModel()
            {
                AllFilteredPostsNum = all_posts.Count(),
                PostsPerPage = int.Parse(Config["option_AdminPostsPerPage"]),
                Page = page,
                CurrentPosts = PostsModel.GetCurrentPosts(all_posts, page, int.Parse(Config["option_AdminPostsPerPage"]))
            }) : Redirect(Config["route_admin"]);
    }
    public IActionResult Categories() => IsAdmin() ? View(CategoriesModel.GetCats()) : Redirect(Config["route_admin"]);
    //---------------------------------------------------
    // тут пользователь идет по пути с возможностью добавления
    // айди. если айди есть - форма заполняется данными во вьюшке. Если нет - приходит пустая форма для 
    // заполнения. - добавление нового поста
    public IActionResult EditPost(int? id)
    {
        if (!IsAdmin()) return Redirect(Config["route_admin"]);
        if (id == null) return View(new CPost());
        if (id < 0) Redirect(Config["route_admin-posts"]);

        CPost post = PostsModel.GetPost(id);
        return View(post == null ? new CPost() : post);
    } 
    // тут будет только после пост запроса отрабатывать этот метод. т.е. уже принимать заполненную
    // форму с новым постом, который добавится в базу данных. Если присутствует айди - 
    // в базе данных меняются даные поста по этому айдишнику.
    [HttpPost]
    public IActionResult EditPost(CPost post, int? id)
    {
        if (!IsAdmin()) return Redirect(Config["route_admin"]);
        if (post == null) return Redirect(Config["route_admin-posts"]);
        if (id == null) PostsModel.Add(post);
        else if (HttpContext.Request.Form["action"] == "Delete")
            PostsModel.DeletePost(id.Value);
        else PostsModel.Edit(post, id.Value);

        return Redirect(Config["route_admin-posts"]);
    }
    public IActionResult DeletePost(int? id)
    {
        if (!IsAdmin()) return Redirect(Config["route_admin"]);
        PostsModel.DeletePost(id.Value);

        return Redirect(Config["route_admin-posts"]);
    }
    // тут пользователь идет по пути с возможностью добавления
    // айди. если айди есть - форма заполняется данными во вьюшке. Если нет - приходит пустая форма для 
    // заполнения. - добавление новой категории
    public IActionResult EditCategory(int? id) 
    {
        if (!IsAdmin()) return Redirect(Config["route_admin"]);
        if (id == null) return View(new CCategory());
        if (id < 0) Redirect(Config["route_admin-posts"]);

        CCategory cat = CategoriesModel.GetCat(id);
        return View(cat == null ? new CCategory() : cat);
    } 
    // тут будет только после пост запроса отрабатывать этот метод. т.е. уже принимать заполненную
    // форму с новой категорией, которая добавится в базу данных. Если присутствует айди - 
    // в базе данных меняются даные категории по этому айдишнику.
    [HttpPost]
    public IActionResult EditCategory(CCategory category, int? id)
    {
        if (!IsAdmin()) return Redirect(Config["route_admin"]);
        if (id == null) CategoriesModel.Add(category);
        else if (HttpContext.Request.Form["action"] == "Delete")
            CategoriesModel.DeleteCat(id.Value);
        else CategoriesModel.Edit(category, id.Value);

        return Redirect(Config["route_admin-categories"]); ;
    }
    public IActionResult DeleteCategory(int? id)
    {
        if (!IsAdmin()) return Redirect(Config["route_admin"]);
        CategoriesModel.DeleteCat(id.Value);

        return Redirect(Config["route_admin-categories"]);
    }
}
