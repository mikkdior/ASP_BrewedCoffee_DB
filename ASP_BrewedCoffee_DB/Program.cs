var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddSingleton<CConfService>();
builder.Services.AddDbContext<CDBContext>();
builder.Services.AddTransient<CRoutesService>();
builder.Services.AddSingleton<CApiService>();
//--------------------------------------------------
CConfService.DB = builder.Services.BuildServiceProvider().GetService<CDBContext>();
CRoutesService routes_service = builder.Services.BuildServiceProvider().GetService<CRoutesService>();
CApiService api_service = builder.Services.BuildServiceProvider().GetService<CApiService>();
//--------------------------------------------------
builder.Services.AddTransient<CPostsService>();
builder.Services.AddTransient<CCategoriesService>();
builder.Services.AddTransient<CAuthService>();
//--------------------------------------------------
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddMvc((options) => options.EnableEndpointRouting = false);
//--------------------------------------------------
var app = builder.Build();
// Configure the HTTP request pipeline.
CConfService.WebRootPath = app.Environment.WebRootPath;
api_service.Use(app, routes_service);
routes_service.SetRoutes(app);
app.UseSession();
app.UseMyFavorites();
app.UseMyLikes();
//--------------------------------------------------
app.UseMvc();
app.UseMvcWithDefaultRoute();
app.UseStaticFiles();
app.UseHttpsRedirection();
app.Run();