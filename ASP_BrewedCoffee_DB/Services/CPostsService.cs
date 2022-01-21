namespace ASP_BrewedCoffee_DB.Models;
public class CPostsService
{
    public CDBContext DB = CConfService.DB;
    public IEnumerable<CPost>? GetPosts(int? cat_id = null) => cat_id == null ?
        DB.Posts.OrderBy(post => post.CreatedDate) : DB.Posts.OrderBy(post => post.CreatedDate).Where(post => post.CategoryId == cat_id);
    public IEnumerable<CPost>? GetArchivePosts(string month, string arch_menu_title, CMenu arch_menu)
    {
        DateTime[] dates = CHelper.GetDates($"/{arch_menu_title.ToLower()}/" + month, arch_menu);
        return DB.Posts.OrderBy(post => post.CreatedDate)
                .Where(post => post.CreatedDate >= dates[0])
                .Where(post => post.CreatedDate <= dates[1]);
    }
    public IEnumerable<CPost>? GetOldPosts() => 
        DB.Posts.OrderBy(post => post.CreatedDate).Where(post => post.CreatedDate.Year < DateTime.Now.Year);
    public IEnumerable<CPost>? GetFavoritePosts(HttpContext context) =>
        DB.Posts.OrderBy(post => post.CreatedDate).Where(post => ((List<string>)context.Items["favorites"]).Contains(post.Id.ToString()));
    public IEnumerable<CPost> GetCurrentPosts(IEnumerable<CPost> all_posts, int page, int num)
        => all_posts.Skip((--page) * num).Take(num);
    public CPost GetPost(int? id) => DB.Posts.Find(id);
    public void Add(CPost post)
    {
        DB.Posts.Add(post);
        DB.SaveChanges();
    }
    public void Edit(CPost post, int id)
    {
        var post_ = DB.Posts.Find(id);
        post_.Title = post.Title;
        post_.CategoryId = post.CategoryId;
        post_.Author = post.Author;
        post_.Content = post.Content;
        post_.CreatedDate = post.CreatedDate;
        DB.SaveChanges();
    }
    public void DeletePost(int id)
    {
        DB.Posts.Remove(DB.Posts.Find(id));
        DB.SaveChanges();
    }
}