namespace EraaSoftCinema.Models
{
    public class Actor
    {
        public int id { get; set; }
        [Required(ErrorMessage = "Actor name is required")]
        [StringLength(30, MinimumLength = 3,ErrorMessage = "Actor name must be between 3 and 30 characters")]
        public string name { get; set; } = string.Empty;
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Description must be between 3 and 100 characters")]

        public string description { get; set; } = string.Empty;

        public string imgUrl { get; set; } = string.Empty;

        public List<MoviesActors> actorMovies { get; set; } = new List<MoviesActors>();
    }
}
