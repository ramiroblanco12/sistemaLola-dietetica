using Microsoft.EntityFrameworkCore;
using Proyecto_Lola.Data;
using Proyecto_Lola.Servicios;

var builder = WebApplication.CreateBuilder(args);

// MVC + Views
builder.Services.AddControllersWithViews();

// DbContext
builder.Services.AddDbContext<LolaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"))
);

// Servicios
builder.Services.AddSingleton<ImpresoraTermica>(_ => new ImpresoraTermica("POS58 Printer"));

builder.Services.AddHttpClient("PrintAgent", c =>
{
    c.BaseAddress = new Uri("http://localhost:5155");
    c.Timeout = TimeSpan.FromSeconds(5);
});

var app = builder.Build();

// Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();
