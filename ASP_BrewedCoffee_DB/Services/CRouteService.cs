namespace ASP_BrewedCoffee_DB.Services
{
    public class CRouteService
    {
        public void SetRoutes(IApplicationBuilder app)
        {
            app.UseMvc(routes =>
            {
                routes.Routes.Clear();

                foreach (CRoute route in CConfService.DB.Routes)
                    routes.MapRoute(route.Name, route.Template, new { controller = route.Controller, action = route.Action });
            });

            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapFallbackToController("Page404", "Home"));
        }
    }
}
