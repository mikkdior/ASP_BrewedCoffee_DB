namespace ASP_BrewedCoffee_DB.Models;
public class CFavoritesM
{
    private RequestDelegate Next;
    public CFavoritesM(RequestDelegate next) { Next = next; }
    public async Task InvokeAsync(HttpContext context)
    {
        List<string> favorites_data;
        bool IsEmpty = true;

        if (context.Session.Keys.Contains("favorites"))
        {
            favorites_data = JsonSerializer.Deserialize<List<string>>(context.Session.GetString("favorites"));
            IsEmpty = false;
        }
        else favorites_data = new List<string>();

        if (context.Request.HasFormContentType)
        {
            if (context.Request.Form["action"] == "favorite")
            {
                string post_id_str = context.Request.Form["post_id"];
                bool in_list = ((IsEmpty) ? false : favorites_data.Contains(post_id_str));

                if (in_list) favorites_data.Remove(post_id_str);
                else favorites_data.Add(post_id_str);

                context.Items["favorites"] = favorites_data;
                context.Session.SetString("favorites", JsonSerializer.Serialize(favorites_data));
            }
        }

        if (context.Items.ContainsKey("favorites")) context.Items["favorites"] = favorites_data;
        else context.Items.Add("favorites", favorites_data);

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