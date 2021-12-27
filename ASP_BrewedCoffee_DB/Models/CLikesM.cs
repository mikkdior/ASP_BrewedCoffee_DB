namespace ASP_BrewedCoffee_v2.Models;
public class CLikesM
{
    private RequestDelegate Next;
    public CLikesM(RequestDelegate next)
    {
        Next = next;
    }
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
                CTableM table = CFilesM.Instance.GetData(CConf.PostsDataPath, new CFilterOptionsM()).Result;

                foreach (CTableLineM table_line in table)
                {
                    if (table_line["Title"] == postname)
                    {
                        int curr_likes = int.Parse(table_line["Likes"]);

                        if (like_button == "like") table_line["Likes"] = (curr_likes + 1).ToString();
                        else if (curr_likes > 0) table_line["Likes"] = (curr_likes - 1).ToString();
                        break;
                    }
                }

                CFilesM.Instance.SaveData(CConf.PostsDataPath, table);
            }
        }

        Next.Invoke(context);
    }
        
}
public static class CLikesExt
{
    public static void UseMyLikes(this IApplicationBuilder self)
    {
        self.UseMiddleware<CLikesM>();
    }
}
