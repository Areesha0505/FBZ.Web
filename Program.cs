using Microsoft.AspNetCore.Builder;
using System.Text;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSession();   

var app = builder.Build();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Comic}/{action=Index}/{id?}");

app.Run();