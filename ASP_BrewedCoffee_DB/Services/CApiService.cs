namespace ASP_BrewedCoffee_DB.Services;
public class CFilterAttributes
{
    public int Page = 1;
    public int Num = 5;
    public string RedirectPath;
    public int CategoryId = -1;
    public DateTime MinDate = DateTime.MinValue;
    public DateTime MaxDate = DateTime.MaxValue;
}
public class CApiService
{
    private IRouter Router;
    public void Use(IApplicationBuilder app, CRoutesService route_service)
    {
        Router = new RouteBuilder(app, new RouteHandler(GetFilteredPosts))
            .MapRoute("api", route_service.GetRoute("api").Template + "/{category?}")
            .MapRoute("api-category", route_service.GetRoute("api-category").Template + "/{category}")
            .MapRoute("api-filter", route_service.GetRoute("api-filter").Template + "/{mindate}-{maxdate}", null, new
            {
                mindate = new RegexRouteConstraint(@"^\d\d\.\d\d\.\d\d\d\d?$"),
                maxdate = new RegexRouteConstraint(@"^\d\d\.\d\d\.\d\d\d\d?$")
            })
            .Build();

        app.UseRouter(Router);
    }
    public async Task GetFilteredPosts(HttpContext context)
    {
        CFilterAttributes ratt = GetAttributes(context);
        string json = JsonSerializer.Serialize(CConfService.DB.Posts
                                                    .OrderBy(post => post.CreatedDate)
                                                    .Where(post =>
                                                        post.CategoryId == -1 ? true : post.CategoryId == ratt.CategoryId
                                                        && post.CreatedDate >= ratt.MinDate
                                                        && post.CreatedDate <= ratt.MaxDate)
                                                    .Skip((ratt.Page - 1) * ratt.Num)
                                                    .Take(ratt.Num));

        await context.Response.WriteAsync(json);
    }
    public CFilterAttributes GetAttributes(HttpContext context)
    {
        CFilterAttributes ratt = new CFilterAttributes() { RedirectPath = context.Request.Path.Value };
        var values = context.Request.RouteValues;

        if (values.ContainsKey("mindate"))
            ratt.MinDate = CHelper.ParseDate(values["mindate"].ToString());
        if (values.ContainsKey("maxdate"))
            ratt.MaxDate = CHelper.ParseDate(values["maxdate"].ToString());
        if (values.ContainsKey("category")) 
            ratt.CategoryId = Convert.ToInt32(values["category"]);

        bool have_page = context.Request.Query.ContainsKey("page");
        bool have_num = context.Request.Query.ContainsKey("num");

        if (have_page) ratt.Page = int.Parse(context.Request.Query["page"]);
        else ratt.RedirectPath += ratt.RedirectPath.Contains('?') ? $"&page={ratt.Page}" : $"?page={ratt.Page}";

        if (have_num) ratt.Num = int.Parse(context.Request.Query["num"]);
        else ratt.RedirectPath += ratt.RedirectPath.Contains('?') ? $"&num={ratt.Num}" : $"?num={ratt.Num}";

        if (!have_page || !have_num) context.Response.Redirect(ratt.RedirectPath);

        return ratt;
    }
}
