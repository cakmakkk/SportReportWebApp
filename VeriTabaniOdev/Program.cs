using Microsoft.EntityFrameworkCore;
using VeriTabaniOdev.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// PostgreSQL ba�lant�s� i�in Connection String'i al ve DbContext'i yap�land�r
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<FileDibiDbSonContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Varsay�lan rota
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Login}/{id?}");

// Fixture i�lemleri i�in rota
app.MapControllerRoute(
    name: "fixture",
    pattern: "{controller=Fixture}/{action=UpdateFixtureView}/{id?}");

app.Run();
