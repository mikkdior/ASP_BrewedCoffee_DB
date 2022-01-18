namespace ASP_BrewedCoffee_DB.Models;
public class CPostsService
{
    public CDBContext DB = CConfService.DB;
    public IEnumerable<CPost>? GetPosts(int? cat_id = null) => cat_id == null ?
        DB.Posts : from post in DB.Posts
                        where post.CategoryId == cat_id
                        select post;
    public IEnumerable<CPost>? GetPosts(string month)
    {
        return default;
    }
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