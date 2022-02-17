namespace ASP_BrewedCoffee_DB.Models;
public class CLikes
{
    private RequestDelegate Next;
    public CLikes(RequestDelegate next) { Next = next; }
    public async Task InvokeAsync(HttpContext context)
    {
        CHelper.RegSessionData("likes", context);
        CHelper.RegSessionData("hidden", context);

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

