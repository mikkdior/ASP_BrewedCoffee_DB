using Microsoft.AspNetCore.Mvc;

namespace ASP_BrewedCoffee_DB.Controllers;
public class AdminController : Controller
{
    public CPosts PostsModel;
    public CCategories CategoriesModel;
    public CAuth AuthModel;

    public AdminController(CAuth auth, CCategories cats, CPosts posts)
    {
        AuthModel = auth;
        PostsModel = posts;
        CategoriesModel = cats;
    }
    //---------------------------------------------------------------------

    // main admin page
    [Route("/admin")]
    public IActionResult Index()
    {
        return View();
    }

    // auth control post request
    [HttpPost]
    [Route("/admin")]
    public IActionResult Index(CAuthData data)
    {
        return View();
    }

    // all posts table
    [Route("/admin/posts")]
    public IActionResult Posts() => View(PostsModel.GetPosts());

    // all categories table
    [Route("/admin/categories")]
    public IActionResult Categories() => View(CategoriesModel.GetCats());
        
    //---------------------------------------------------

    // тут пользователь идет по пути с возможностью добавления
    // айди. если айди есть - форма заполняется данными во вьюшке. Если нет - приходит пустая форма для 
    // заполнения. - добавление нового поста
    [Route("/admin/posts/post/{id?}")]
    public IActionResult EditPost(int? id) => View(id == null ? new CPost() : PostsModel.GetPost(id));

    // тут будет только после пост запроса отрабатывать этот метод. т.е. уже принимать заполненную
    // форму с новым постом, который добавится в базу данных. Если присутствует айди - 
    // в базе данных меняются даные поста по этому айдишнику.
    [HttpPost]
    [Route("/admin/posts/post/{id?}")]
    public IActionResult EditPost(CPost post, int? id)
    {
        if (id == null)
            PostsModel.Add(post);
        else
        {
            post.Id = id.Value;
            PostsModel.Edit(post);
        }

        return View(post);
    }

    // тут пользователь идет по пути с возможностью добавления
    // айди. если айди есть - форма заполняется данными во вьюшке. Если нет - приходит пустая форма для 
    // заполнения. - добавление новой категории
    [Route("/admin/categories/category/{id?}")]   
    public IActionResult EditCategory(int? id) => View(id == null ? new CCategory() : CategoriesModel.GetCat(id));

    // тут будет только после пост запроса отрабатывать этот метод. т.е. уже принимать заполненную
    // форму с новой категорией, которая добавится в базу данных. Если присутствует айди - 
    // в базе данных меняются даные категории по этому айдишнику.
    [HttpPost]
    [Route("/admin/categories/category/{id?}")]
    public IActionResult EditCategory(CCategory category, int? id)
    {
        if (id == null)
            CategoriesModel.Add(category);
        else
        {
            category.Id = id.Value;
            CategoriesModel.Edit(category);
        }

        return View(category);
    }
}
