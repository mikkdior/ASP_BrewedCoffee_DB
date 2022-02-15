namespace ASP_BrewedCoffee_DB.Models;
public class CPostsService
{
    public CDBContext DB = CConfService.DB;
    public IEnumerable<CPost>? GetPosts(int? cat_id = null) => 
        DB.Posts.OrderByDescending(post => post.CreatedDate)
            .Where(post => cat_id == null ? true : post.CategoryId == cat_id);
    public IEnumerable<CPost>? GetArchivePosts(string month, string arch_menu_title, CMenu arch_menu) =>
        DB.Posts.OrderByDescending(post => post.CreatedDate)
            .Where(post => post.CreatedDate > DateTime.Now.AddYears(-1))
            .Where(post => post.CreatedDate.Month == CHelper.GetMonthNum($"/{arch_menu_title.ToLower()}/" + month, arch_menu))
            .Where(post => post.CreatedDate.Month == DateTime.Now.Month ? post.CreatedDate.Year == DateTime.Now.Year : true);
    public IEnumerable<CPost>? GetOldPosts() =>
        DB.Posts.OrderByDescending(post => post.CreatedDate)
            .Where(post => post.CreatedDate < DateTime.Now.AddYears(-1).AddMonths(1).AddDays(-(DateTime.Now.Day - 1)).Date);
    public IEnumerable<CPost>? GetFavoritePosts(HttpContext context) =>
        DB.Posts.OrderByDescending(post => post.CreatedDate)
            .Where(post => ((List<string>)context.Items["favorites"])
            .Contains(post.Id.ToString()));
    public IEnumerable<CPost> GetCurrentPosts(IEnumerable<CPost> all_posts, int page, int num) =>
        all_posts.Skip((--page) * num).Take(num);
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
    public void DeletePost(int? id)
    {
        if (id == null) return;
        DB.Posts.Remove(DB.Posts.Find(id));
        DB.SaveChanges();
    }
}
