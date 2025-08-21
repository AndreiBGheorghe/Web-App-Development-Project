using Proiect;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<CatalogContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Catalog")));
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

RotativaConfiguration.Setup(Path.Combine(Directory.GetCurrentDirectory(), "Rotativa", "wkhtmltopdf", "bin"), "Rotativa");

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Shared/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Catalog}/{action=Index}");

app.Run();