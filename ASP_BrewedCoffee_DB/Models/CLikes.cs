namespace ASP_BrewedCoffee_DB.Models;
public class CLikes
{
    private RequestDelegate Next;
    public CLikes(RequestDelegate next) { Next = next; }
    public async Task InvokeAsync(HttpContext context)
    {
        List<string> likes_data;
        bool IsEmpty = true;

        if (context.Session.Keys.Contains<string>("likes"))
        {
            likes_data = JsonSerializer.Deserialize<List<string>>(context.Session.GetString("likes"));
            IsEmpty = false;
        }
        else likes_data = new List<string>();

        if (context.Request.HasFormContentType)
        {
            if (context.Request.Form["action"] == "like" || context.Request.Form["action"] == "dislike")
            {
                string post_id_str = context.Request.Form["post_id"];
                bool in_list = ((IsEmpty) ? false : likes_data.Contains(post_id_str));
                
                if (in_list) likes_data.Remove(post_id_str);
                else likes_data.Add(post_id_str);

                context.Items["likes"] = likes_data;
                context.Session.SetString("likes", JsonSerializer.Serialize(likes_data));
                //--------------------------------------------------
                string like_button = context.Request.Form["action"];
                CPost post = CConfService.DB.Posts.Find(int.Parse(post_id_str));
                
                if (like_button == "like") post.Likes++;
                else if (post.Likes > 0) post.Likes--;
                
                CConfService.DB.SaveChanges();
            }
        }

        if (context.Items.ContainsKey("likes")) context.Items["likes"] = likes_data;
        else context.Items.Add("likes", likes_data);

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

