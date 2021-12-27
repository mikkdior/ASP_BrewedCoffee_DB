namespace ASP_BrewedCoffee_DB.Services;
public class CFilterAttributesM
{
    public int Page = 1;
    public int Num = 5;
    public string RedirectPath;
    public string Category = "";
    public DateTime MinDate = default;
    public DateTime MaxDate = default;
}
public class CApiServiceS
{
    private IRouter Router;
    public void Use(IApplicationBuilder app, CRouteServiceS route_service)
    {
        Dictionary<string, CRouteDataM> routes_dict = route_service.MyRoutes;

        Router = new RouteBuilder(app, new RouteHandler(GetFilteredPosts))
            .MapRoute("api", routes_dict["api"].Template + "/{category?}")
            .MapRoute("api-category", routes_dict["api-category"].Template + "/{category}")
            .MapRoute("api-filter", routes_dict["api-filter"].Template + "/{mindate}-{maxdate}", null, new
            {
                mindate = new RegexRouteConstraint(@"^\d\d\.\d\d\.\d\d\d\d?$"),
                maxdate = new RegexRouteConstraint(@"^\d\d\.\d\d\.\d\d\d\d?$")
            })
            .Build();

        app.UseRouter(Router);
    }
    public async Task GetFilteredPosts(HttpContext context)
    {
        CFilterAttributesM ratt = GetAttributes(context);

        CPostsModel posts = new CPostsBuilderM()
            .SetNum(ratt.Num)
            .SetCatName(ratt.Category)
            .SetShift(ratt.Page, ratt.Num)
            .SetMinDate(ratt.MinDate)
            .SetMaxDate(ratt.MaxDate)
            .Build();

        await Print(context, posts);
    }
    public CFilterAttributesM GetAttributes(HttpContext context)
    {
        CFilterAttributesM ratt = new CFilterAttributesM() { RedirectPath = context.Request.Path.Value };
        var values = context.Request.RouteValues;

        if (values.ContainsKey("mindate"))
            ratt.MinDate = CHelperM.ParseDate(values["mindate"].ToString());
        if (values.ContainsKey("maxdate"))
            ratt.MaxDate = CHelperM.ParseDate(values["maxdate"].ToString());
        if (values.ContainsKey("category"))
            ratt.Category = values["category"].ToString();

        bool have_page = context.Request.Query.ContainsKey("page");
        bool have_num = context.Request.Query.ContainsKey("num");

        if (have_page) ratt.Page = int.Parse(context.Request.Query["page"]);
        else ratt.RedirectPath += ratt.RedirectPath.Contains('?') ? $"&page={ratt.Page}" : $"?page={ratt.Page}";

        if (have_num) ratt.Num = int.Parse(context.Request.Query["num"]);
        else ratt.RedirectPath += ratt.RedirectPath.Contains('?') ? $"&num={ratt.Num}" : $"?num={ratt.Num}";

        if (!have_page || !have_num) context.Response.Redirect(ratt.RedirectPath);

        return ratt;
    }
    public async Task Print(HttpContext context, CPostsModel posts)
    {
        await context.Response.WriteAsync(JsonSerializer.Serialize(posts));
    }
}
