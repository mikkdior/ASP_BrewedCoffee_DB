var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<CConf>();
builder.Services.AddTransient<CPosts>();
builder.Services.AddTransient<CCategories>();
builder.Services.AddTransient<CAuth>();
builder.Services.AddDbContext<CDBContext>();
builder.Services.AddSession();

builder.Services.AddMvc((options) => options.EnableEndpointRouting = false);

var app = builder.Build();

// Configure the HTTP request pipeline.

/*if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}*/
app.UseSession();

app.UseMvc();
app.UseMvcWithDefaultRoute();

app.UseStaticFiles();

app.UseHttpsRedirection();

app.Run();
