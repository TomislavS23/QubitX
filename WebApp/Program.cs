using System.Net.Http.Headers;
using WebApp.AutoMapper;
using WebApp.Services;

namespace WebApp;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        
        // AutoMapper support
        builder.Services.AddAutoMapper(typeof(MappingProfile));
        
        // IApiService
        builder.Services.AddScoped<IApiService, ApiService>();

        builder.Services.AddHttpClient("httpclient", client =>
        {
            client.BaseAddress = new Uri("http://localhost:5097/");
        });

        builder.Services.AddAuthentication()
            .AddCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
            });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
        }

        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}