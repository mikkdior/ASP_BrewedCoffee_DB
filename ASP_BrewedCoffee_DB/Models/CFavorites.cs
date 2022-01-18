namespace ASP_BrewedCoffee_DB.Models;
public class CFavoritesM
{
    private RequestDelegate Next;
    public CFavoritesM(RequestDelegate next) { Next = next; }
    public async Task InvokeAsync(HttpContext context)
    {
        var cookies = new List<string>();

        if (context.Request.Cookies.ContainsKey("favorites"))
            cookies = JsonSerializer.Deserialize<List<string>>(context.Request.Cookies["favorites"]);

        context.Items.Add("favorites", cookies);

        if (context.Request.HasFormContentType)
        {
            if (context.Request.Form["action"] == "favorite")
            {
                cookies = new List<string>();
                string postname = context.Request.Form["postname"];
                bool in_list = false;

                if (context.Request.Cookies["favorites"] != null)
                {
                    cookies = JsonSerializer.Deserialize<List<string>>(context.Request.Cookies["favorites"]);

                    foreach (string title in cookies)
                    {
                        if (title == postname)
                        {
                            in_list = true;
                            break;
                        }
                    }
                }

                if (in_list) cookies.Remove(postname);
                else cookies.Add(postname);

                context.Items["favorites"] = cookies;
                context.Response.Cookies.Append("favorites", JsonSerializer.Serialize(cookies));
            }
        }

        Next.Invoke(context);
    }
}
public static class CFavoritesExt
{
    public static void UseMyFavorites(this IApplicationBuilder self)
    {
        self.UseMiddleware<CFavoritesM>();
    }
}