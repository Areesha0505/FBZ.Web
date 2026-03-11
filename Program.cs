using Microsoft.AspNetCore.Builder;
using System.Text;

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

   app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Comic}/{action=Index}/{id?}");

app.Run();