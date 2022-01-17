var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<CConfService>();
builder.Services.AddDbContext<CDBContext>();
builder.Services.AddTransient<CRouteService>();
CConfService.DB = builder.Services.BuildServiceProvider().GetService<CDBContext>();
CRouteService routes_service = builder.Services.BuildServiceProvider().GetService<CRouteService>();
builder.Services.AddTransient<CPostsService>();
builder.Services.AddTransient<CCategoriesService>();
builder.Services.AddTransient<CAuthService>();
//--------------------------------------------------
builder.Services.AddSession();
builder.Services.AddMvc((options) => options.EnableEndpointRouting = false);

//--------------------------------------------------
var app = builder.Build();

Console.WriteLine(app.Environment.WebRootPath);
// Configure the HTTP request pipeline.

/*if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}*/
//--------------------------------------------------
routes_service.SetRoutes(app);
app.UseSession();

app.UseMvc();
app.UseMvcWithDefaultRoute();

app.UseStaticFiles();

app.UseHttpsRedirection();

app.Run();