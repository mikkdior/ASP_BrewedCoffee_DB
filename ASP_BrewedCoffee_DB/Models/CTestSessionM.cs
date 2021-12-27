namespace ASP_BrewedCoffee_v2.Models;
public class CTestSessionM
{
    private RequestDelegate Next;
    public CTestSessionM(RequestDelegate next)
    {
        Next = next;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Session.Keys.Contains("count"))
        {
            Console.WriteLine("have not key");
            context.Session.SetInt32("count", 0);
        }

        int curr_count = (int)context.Session.GetInt32("count");
        context.Session.SetInt32("count", ++curr_count);
        Console.WriteLine(curr_count);

        Next.Invoke(context);
    }
}
public static class CTestSessionExt
{
    public static void UseTestSession(this IApplicationBuilder self)
    {
        self.UseMiddleware<CTestSessionM>();
    }
}
