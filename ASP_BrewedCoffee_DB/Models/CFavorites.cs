namespace ASP_BrewedCoffee_DB.Models;
public class CFavoritesM
{
    private RequestDelegate Next;
    public CFavoritesM(RequestDelegate next) { Next = next; }
    public async Task InvokeAsync(HttpContext context)
    {
        CHelper.RegSessionData("favorites", context);
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