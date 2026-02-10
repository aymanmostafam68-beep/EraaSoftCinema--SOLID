using System.Globalization;
using EraaSoftCinema.Areas.Admin.Controllers;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace EraaSoftCinema
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<IRepo<Models.Movie>, Repository<Models.Movie>>();
            builder.Services.AddScoped<IRepo<Models.Category>, Repository<Models.Category>>();
            builder.Services.AddScoped<IRepo<Models.Actor>, Repository<Models.Actor>>();
            builder.Services.AddScoped<IRepo<Models.Language>, Repository<Models.Language>>();
            builder.Services.AddScoped<IRepo<Models.MoviesActors>, Repository<Models.MoviesActors>>();
            builder.Services.AddScoped<IRepo<Models.MovieSubimg>, Repository<Models.MovieSubimg>>();

            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
            const string defaultCulture = "en";
            var supportedCultures = new[]
            {
            new CultureInfo(defaultCulture),
            new CultureInfo("en"),
             new CultureInfo("ar")
};

            builder.Services.Configure<RequestLocalizationOptions>(options => {
                options.DefaultRequestCulture = new RequestCulture(defaultCulture);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            builder.Services.AddSession();




            var app = builder.Build();
            app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);



            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();
            app.UseSession();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
