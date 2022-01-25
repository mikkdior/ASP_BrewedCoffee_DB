namespace ASP_BrewedCoffee_DB.Models;
public record RAdminData(string AdminLogin, string AdminPass);
public class CAuthService
{
    public RAdminData AdminAuthData;
    public CAuthService(IConfiguration config) => AdminAuthData = new RAdminData(config["AdminLogin"], config["AdminPass"]);
    public bool CheckAuth(HttpContext context) => context.Session.GetString("is_auth") == "true";
    public bool Authorization(HttpContext context, RAdminData input_data) 
    {
        if (!context.Request.HasFormContentType || context.Request.Form["action"] != "log_in") return false;
        bool is_auth = input_data.AdminLogin == AdminAuthData.AdminLogin && input_data.AdminPass == AdminAuthData.AdminPass;
        if (is_auth) context.Session.SetString("is_auth", "true");

        return is_auth;
    }
    public void UnAuthorization(HttpContext context) => context.Session.SetString("is_auth", "false");
}