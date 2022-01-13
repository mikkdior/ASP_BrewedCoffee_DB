namespace ASP_BrewedCoffee_DB.Models;
public class CPosts
{
    public CDBContext DB = CConf.DB;
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
        post.Id = DB.Posts.Count();
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
}