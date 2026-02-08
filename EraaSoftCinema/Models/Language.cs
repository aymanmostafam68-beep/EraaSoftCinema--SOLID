namespace EraaSoftCinema.Models
{
    public class Language
    {
        public int id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, MinimumLength = 2,
       ErrorMessage = "Name must be between 2 and 50 characters")]
        public string name { get; set; } = string.Empty;

        public List<Movie> Movies { get; set; } = new List<Movie>();
    }
}
