namespace ASP_BrewedCoffee_DB.Services;
public class CFilterAttributes
{
    public int Page = 1;
    public int Num = 5;
    public string? RedirectPath;
    public int CategoryId = -1;
    public DateTime MinDate = DateTime.MinValue;
    public DateTime MaxDate = DateTime.MaxValue;
}
public class CApiService
{
    private IRouter? Router;
    public void UseApi(IApplicationBuilder app, CRoutesService route_service)
    {
        Router = new RouteBuilder(app, new RouteHandler(GetFilteredPosts))
            .MapRoute("api", route_service.GetRoute("api").Template + "/{category?}")
            .MapRoute("api-category", route_service.GetRoute("api-category").Template + "/{category}")
            .MapRoute("api-filter", route_service.GetRoute("api-filter").Template + "/{mindate}-{maxdate}/{category?}", null, new
            {
                mindate = new RegexRouteConstraint(@"^\d\d\.\d\d\.\d\d\d\d?$"),
                maxdate = new RegexRouteConstraint(@"^\d\d\.\d\d\.\d\d\d\d?$")
            })
            .Build();

        app.UseRouter(Router);
    }
    public async Task GetFilteredPosts(HttpContext context)
    {
        CFilterAttributes fatt = GetAttributes(context);
        await context.Response.WriteAsync(JsonSerializer.Serialize
            (CConfService.DB.Posts
                .OrderBy(post => post.CreatedDate)
                .Where(post => fatt.CategoryId == -1 ? true : post.CategoryId == fatt.CategoryId)
                .Where(post => post.CreatedDate >= fatt.MinDate)
                .Where(post => post.CreatedDate <= fatt.MaxDate)
                .Skip((fatt.Page - 1) * fatt.Num)
                .Take(fatt.Num)));
    }
    public CFilterAttributes GetAttributes(HttpContext context)
    {
        CFilterAttributes fatt = new CFilterAttributes() { RedirectPath = context.Request.Path.Value };
        var values = context.Request.RouteValues;

        if (values.ContainsKey("mindate"))
            fatt.MinDate = CHelper.ParseDate(values["mindate"].ToString());
        if (values.ContainsKey("maxdate"))
            fatt.MaxDate = CHelper.ParseDate(values["maxdate"].ToString());
        if (values.ContainsKey("category")) 
            fatt.CategoryId = Convert.ToInt32(values["category"]);

        bool have_page = context.Request.Query.ContainsKey("page");
        bool have_num = context.Request.Query.ContainsKey("num");

        if (have_page) fatt.Page = int.Parse(context.Request.Query["page"]);
        else fatt.RedirectPath += fatt.RedirectPath.Contains('?') ? $"&page={fatt.Page}" : $"?page={fatt.Page}";

        if (have_num) fatt.Num = int.Parse(context.Request.Query["num"]);
        else fatt.RedirectPath += fatt.RedirectPath.Contains('?') ? $"&num={fatt.Num}" : $"?num={fatt.Num}";

        if (!have_page || !have_num) context.Response.Redirect(fatt.RedirectPath);

        return fatt;
    }
}
