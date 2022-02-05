namespace ASP_BrewedCoffee_DB.ViewModels;
public class CAdminPostsViewModel : IViewModel
{
    public int PostsPerPage { get; set; }
    public int Page { get; set; }
    public int AllFilteredPostsNum { get; set; }
    public IEnumerable<CPost>? CurrentPosts { get; set; }
}
