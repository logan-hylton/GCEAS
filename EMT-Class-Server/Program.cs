using EMT_Class_Server.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;

namespace EMT_Class_Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();

            builder.Services.AddSingleton<AuthenticationStateProvider, CustomAuthenticationStateProvider>(serviceProvider => new CustomAuthenticationStateProvider("Data Source=emt-class.lhylton.com;Initial Catalog=EmtClass;User ID=gceas;Password=~4kL9i2g7"));

            builder.Services.AddSingleton<WeatherForecastService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");

            app.Run();
        }
    }
}