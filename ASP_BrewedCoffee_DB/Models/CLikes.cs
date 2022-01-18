namespace ASP_BrewedCoffee_DB.Models;
public class CLikes
{
    private RequestDelegate Next;
    public CLikes(RequestDelegate next) { Next = next; }
    public async Task InvokeAsync(HttpContext context)
    {
        var likes_data = new List<string>();

        if (context.Session.Keys.Contains<string>("likes"))
            likes_data = JsonSerializer.Deserialize<List<string>>(context.Session.GetString("likes"));

        context.Items.Add("likes", likes_data);

        if (context.Request.HasFormContentType)
        {
            if (context.Request.Form["action"] == "like" || context.Request.Form["action"] == "dislike")
            {
                likes_data = new List<string>();
                string postname = context.Request.Form["postname"];
                bool in_list = false;

                if (context.Session.GetString("likes") != null)
                {
                    likes_data = JsonSerializer.Deserialize<List<string>>(context.Session.GetString("likes"));

                    foreach (string title in likes_data)
                    {
                        if (title == postname)
                        {
                            in_list = true;
                            break;
                        }
                    }
                }

                if (in_list) likes_data.Remove(postname);
                else likes_data.Add(postname);

                context.Items["likes"] = likes_data;
                context.Session.SetString("likes", JsonSerializer.Serialize(likes_data));
                string like_button = context.Request.Form["action"];

                foreach (CPost post in CConfService.DB.Posts)
                {
                    if (post.Title == postname)
                    {
                        if (like_button == "like") post.Likes = ++post.Likes;
                        else if (post.Likes > 0) post.Likes = --post.Likes;
                        break;
                    }
                }
                CConfService.DB.SaveChanges();
            }
        }

        Next.Invoke(context);
    }

}
public static class CLikesExt
{
    public static void UseMyLikes(this IApplicationBuilder self)
    {
        self.UseMiddleware<CLikes>();
    }
}

