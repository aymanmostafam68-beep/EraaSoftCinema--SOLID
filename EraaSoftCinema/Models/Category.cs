namespace EraaSoftCinema.Models
{
    public class Category
    {
        public int id { get; set; }
        [Required(ErrorMessage = "Category name is required")]
        [StringLength(100, MinimumLength = 2,
        ErrorMessage = "Category name must be between 2 and 100 characters")]
        public string name { get; set; } = string.Empty;
        //[Required(ErrorMessage = "Image URL is required")]
        public string imgUrl { get; set; } = string.Empty;

        public bool status { get; set; } = true;

        public List<Movie> Movies { get; set; } = new List<Movie>();
    }
}
