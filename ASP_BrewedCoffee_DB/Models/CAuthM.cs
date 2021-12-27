namespace ASP_BrewedCoffee_DB.Models;
public class CAuthM
{
    public bool IsAuth = false;
    public void Authorization(string login, string password)
    {
        IsAuth = login == CConf.MasterLogin && password == CConf.MasterPass;
    }
}
public class CAuthorizationM
{
    private RequestDelegate Next;
    private CAuthM Auth;
    public CAuthorizationM(RequestDelegate next) { Next = next; }
    public async Task InvokeAsync(HttpContext context, CAuthM auth)
    {
        context.Items.Add("IsAuth", false);
        if (context.Request.Cookies["is_auth"] == "true") context.Items["IsAuth"] = true;

        if (context.Request.HasFormContentType)
        {
            if (context.Request.Form["action"] == "login")
            {
                auth.Authorization(context.Request.Form["login"], context.Request.Form["pass"]);
                context.Items["IsAuth"] = auth.IsAuth;
                if (auth.IsAuth) context.Response.Cookies.Append("is_auth", "true");
                /*context.Items["IsAuth"] = context.Request.Form["login"] == CConf.MasterLogin && context.Request.Form["pass"] == CConf.MasterPass;
                if ((bool)context.Items["IsAuth"]) context.Response.Cookies.Append("is_auth", "true");*/
            }
        }

        Next.Invoke(context);
    }
}
public static class CAuthorizationExt
{
    public static void UseMyAuthorization(this IApplicationBuilder self)
    {
        self.UseMiddleware<CAuthorizationM>();
    }
}