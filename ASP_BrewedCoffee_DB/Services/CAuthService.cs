namespace ASP_BrewedCoffee_DB.Models;
public record RAuthData(string Login, string Pass);
public class CAuthService
{
    public RAuthData AuthData;
    public CAuthService() => AuthData = new RAuthData(CConfService.DB.GetOptionsValue("AdminLogin"), CConfService.DB.GetOptionsValue("AdminPass"));
    public bool CheckAuth(HttpContext context) => context.Request.Cookies["is_auth"] == "true";
    public bool Authorization(HttpContext context, RAuthData input_data) 
    {
        if (!context.Request.HasFormContentType || context.Request.Form["action"] != "log_in") return false;
        bool is_auth = input_data.Login == AuthData.Login && input_data.Pass == AuthData.Pass;
        if (is_auth) context.Response.Cookies.Append("is_auth", "true");

        return is_auth;
    } 
}