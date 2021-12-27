namespace ASP_BrewedCoffee_v2.Services;

public class CRouteServiceS
{
    public Dictionary<string, CRouteDataM> MyRoutes = new Dictionary<string, CRouteDataM>();
    public CRouteServiceS()
    {
        SetRoutes();
    }
    public void SetRoutes() => MyRoutes = JsonSerializer.Deserialize<Dictionary<string, CRouteDataM>>(File.ReadAllText(CConf.RoutesPath));

    public void Use(IApplicationBuilder app)
    {
        app.UseMvc(routes =>
        {
            routes.Routes.Clear();

            foreach(KeyValuePair<string, CRouteDataM> pair in MyRoutes)
                routes.MapRoute(pair.Key, pair.Value.Template, new { controller = pair.Value.Controller, action = pair.Value.Action });
        });

        app.UseRouting();
        app.UseEndpoints(endpoints => endpoints.MapFallbackToController("Page404", "Home"));
    }
}