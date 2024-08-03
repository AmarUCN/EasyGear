using DAL.DAO;
using DAL.DB;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);




        // Register ProductDB with the connection string
        builder.Services.AddScoped<ProductDAO>((_) => new ProductDB("Data Source=hildur.ucn.dk;Initial Catalog=dma-csd-v221_10009362;User Id=dma-csd-v221_10009362;Password=Password1!;"));
        builder.Services.AddScoped<ProductLineDAO>((_) => new ProductLineDB("Data Source=hildur.ucn.dk;Initial Catalog=dma-csd-v221_10009362;User Id=dma-csd-v221_10009362;Password=Password1!;"));
        builder.Services.AddScoped<DeliveryDAO>((_) => new DeliveryDB("Data Source=hildur.ucn.dk;Initial Catalog=dma-csd-v221_10009362;User Id=dma-csd-v221_10009362;Password=Password1!;"));
        builder.Services.AddScoped<AccountDAO>((_) => new AccountDB("Data Source=hildur.ucn.dk;Initial Catalog=dma-csd-v221_10009362;User Id=dma-csd-v221_10009362;Password=Password1!;"));
        builder.Services.AddScoped<BasketDAO>((_) => new BasketDB("Data Source=hildur.ucn.dk;Initial Catalog=dma-csd-v221_10009362;User Id=dma-csd-v221_10009362;Password=Password1!;"));

        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
     .AddCookie(options =>
     {
         options.LoginPath = "/Account/Login";
         options.LogoutPath = "/Account/Logout";
         options.AccessDeniedPath = "/Account/AccessDenied";
     });


        // Add services to the container
        builder.Services.AddControllers();
        builder.Services.AddLogging();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}
