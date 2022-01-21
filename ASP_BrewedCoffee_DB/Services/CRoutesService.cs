namespace ASP_BrewedCoffee_DB.Services
{
    public class CRoutesService
    {
        public void SetRoutes(IApplicationBuilder app)
        {
            app.UseMvc(routes =>
            {
                routes.Routes.Clear();

                foreach (CRoute route in CConfService.DB.Routes)
                    routes.MapRoute(route.Name, route.Template, new { controller = route.Controller, action = route.Action });
            });

            app.UseEndpoints(endpoints => endpoints.MapFallbackToController("Page404", "Home"));
        }
        public CRoute GetRoute(string name)
        {
            foreach (CRoute route in CConfService.DB.Routes) 
                if (route.Name == name) return route;

            return null;
        }
    }
}
