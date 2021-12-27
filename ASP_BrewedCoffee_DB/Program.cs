var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CDBContext>();
builder.Services.AddTransient<CPostsS>();
builder.Services.AddTransient<CCategoriesS>();

// Add services to the container.
builder.Services.AddMvc((options) =>
{
    options.EnableEndpointRouting = false;
});

builder.Services.AddSingleton<CRouteServiceS>();
builder.Services.AddSingleton<CApiServiceS>();
builder.Services.AddScoped<CAuthM>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

CApiServiceS api_service = builder.Services.BuildServiceProvider().GetService<CApiServiceS>();
CRouteServiceS route_service = builder.Services.BuildServiceProvider().GetService<CRouteServiceS>();
CConf.DB = builder.Services.BuildServiceProvider().GetService<CDBContext>();
CConf.DataAccessFactory = new CDBFactory();
var app = builder.Build();

api_service.Use(app, route_service);
route_service.SetRoutes();

// Configure the HTTP request pipeline.
app.UseSession();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseMyAuthorization();
app.UseMyFavorites();
app.UseMyLikes();

CConf.WebRootPath = app.Environment.WebRootPath;

app.UseStaticFiles();

route_service.Use(app);

app.UseHttpsRedirection();

app.Run();
