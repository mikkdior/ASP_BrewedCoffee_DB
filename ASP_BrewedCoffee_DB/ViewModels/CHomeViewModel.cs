namespace ASP_BrewedCoffee_DB.ViewModels;
public class CHomeViewModel : IViewModel
{
    public CMenu CategoriesMenu { get; set; }
    public CMenu ArchiveMenu { get; set; }
    public IEnumerable<CPost>? CurrentPosts { get; set; }
    public int PostsPerPage { get; set; }
    public int Page { get; set; }
    public int AllFilteredPostsNum { get; set; }
}
