using APIClient;
using DAL.DAO;
using Microsoft.AspNetCore.Authentication.Cookies;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        
        builder.Services.AddSingleton<ProductDAO>(new ProductAPIClient("https://localhost:7297"));
        builder.Services.AddSingleton<ProductLineDAO>(new ProductLineAPIClient("https://localhost:7297"));
        builder.Services.AddSingleton<DeliveryDAO>(new DeliveryAPIClient("https://localhost:7297"));
        builder.Services.AddSingleton<AccountDAO>(new AccountAPIClient("https://localhost:7297"));


        builder.Services.AddControllersWithViews();
        

        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.AccessDeniedPath = "/Account/AccessDenied";
            });


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Account}/{action=Login}/{id?}");

        app.Run();
    }
}