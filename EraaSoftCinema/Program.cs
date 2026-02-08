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



            var app = builder.Build();

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

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
