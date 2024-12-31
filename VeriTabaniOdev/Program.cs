using Microsoft.EntityFrameworkCore;
using VeriTabaniOdev.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// PostgreSQL baðlantýsý için Connection String'i al ve DbContext'i yapýlandýr
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

// Varsayýlan rota
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Login}/{id?}");

// Fixture iþlemleri için rota
app.MapControllerRoute(
    name: "fixture",
    pattern: "{controller=Fixture}/{action=UpdateFixtureView}/{id?}");

app.Run();
