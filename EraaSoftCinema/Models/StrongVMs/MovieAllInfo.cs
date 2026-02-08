namespace EraaSoftCinema.Models.StrongVMs
{
    public class MovieAllInfo
    {
        public Movie movie { get; set; } =  new Movie();
        public List<Actor> actors { get; set; } = new List<Actor>()!;
        public List<Category> Categories { get; set; } = new List<Category>();

        public List<Language> Languages { get; set; } = new List<Language>();

        public List<MovieSubimg> MovieSubimgs { get; set; } = new List<MovieSubimg>();




        public string LanguageName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;


     }
}
