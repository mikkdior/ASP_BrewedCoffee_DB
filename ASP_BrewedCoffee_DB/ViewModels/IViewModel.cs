namespace ASP_BrewedCoffee_DB.ViewModels
{
    public interface IViewModel
    {
        int PostsPerPage { get; set; }
        int Page { get; set; }
        int AllFilteredPostsNum { get; set; }
        public IEnumerable<CPost>? CurrentPosts { get; set; }
    }
}
