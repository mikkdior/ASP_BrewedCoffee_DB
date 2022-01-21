using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ASP_BrewedCoffee_DB.Controllers;
public class AdminController : Controller
{
    public CPostsService PostsModel;
    public CCategoriesService CategoriesModel;
    public CAuthService AuthModel;
    public AdminController(CAuthService auth, CCategoriesService cats, CPostsService posts)
    {
        AuthModel = auth;
        PostsModel = posts;
        CategoriesModel = cats;
    }
    //---------------------------------------------------------------------
    public bool IsAdmin() => AuthModel.CheckAuth(HttpContext);
    // main admin page
    public IActionResult Index() => IsAdmin() ? Redirect("/admin/posts") : View();
    // auth admin control post request
    [HttpPost]
    public IActionResult Index(RAdminData data) => AuthModel.Authorization(HttpContext, data) ? Redirect("/admin/posts") : View();
    public IActionResult LogOut() 
    {
        HttpContext.Session.SetString("is_auth", "false");
        return Redirect("/admin");
    }
    //---------------------------------------------------------------------
    // all posts table
    public IActionResult Posts() => IsAdmin() ? View(PostsModel.GetPosts()) : Redirect("/admin");

    // all categories table
    public IActionResult Categories() => IsAdmin() ? View(CategoriesModel.GetCats()) : Redirect("/admin");
        
    //---------------------------------------------------

    // тут пользователь идет по пути с возможностью добавления
    // айди. если айди есть - форма заполняется данными во вьюшке. Если нет - приходит пустая форма для 
    // заполнения. - добавление нового поста
    public IActionResult EditPost(int? id) => IsAdmin() ? View(id == null ? new CPost() : PostsModel.GetPost(id)) : Redirect("/admin");

    // тут будет только после пост запроса отрабатывать этот метод. т.е. уже принимать заполненную
    // форму с новым постом, который добавится в базу данных. Если присутствует айди - 
    // в базе данных меняются даные поста по этому айдишнику.
    [HttpPost]
    public IActionResult EditPost(CPost post, int? id)
    {
        if (!IsAdmin()) return Redirect("/admin");
        if (id == null) PostsModel.Add(post);
        else if (HttpContext.Request.Form["action"] == "Delete")
            PostsModel.DeletePost(id.Value);
        else PostsModel.Edit(post, id.Value);

        return Redirect("/admin/posts");
    }

    // тут пользователь идет по пути с возможностью добавления
    // айди. если айди есть - форма заполняется данными во вьюшке. Если нет - приходит пустая форма для 
    // заполнения. - добавление новой категории
    public IActionResult EditCategory(int? id) => IsAdmin() ? View(id == null ? new CCategory() : CategoriesModel.GetCat(id)) : Redirect("/admin");

    // тут будет только после пост запроса отрабатывать этот метод. т.е. уже принимать заполненную
    // форму с новой категорией, которая добавится в базу данных. Если присутствует айди - 
    // в базе данных меняются даные категории по этому айдишнику.
    [HttpPost]
    public IActionResult EditCategory(CCategory category, int? id)
    {
        if (!IsAdmin()) return Redirect("/admin");
        if (id == null) CategoriesModel.Add(category);
        else if (HttpContext.Request.Form["action"] == "Delete")
            CategoriesModel.DeleteCat(id.Value);
        else CategoriesModel.Edit(category, id.Value);

        return Redirect("/admin/categories");;
    }
}
