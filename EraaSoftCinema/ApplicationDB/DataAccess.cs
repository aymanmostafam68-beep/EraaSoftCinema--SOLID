using EraaSoftCinema.Models;

namespace EraaSoftCinema.ApplicationDB
{
    public class DataAccess : DbContext
    {
        public  DbSet<Movie> Movies { get; set; }
       public DbSet<Actor> Actors { get; set; }
       public DbSet<MoviesActors> MoviesActors { get; set; }
       public DbSet<Language> MoviesLanguages { get; set; }
       public DbSet<Category> MoviesCategories { get; set; }
       public DbSet<MovieSubimg> MovieSubimgs { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=.;Database=CinemaMovies;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }
}
