using System.ComponentModel.DataAnnotations.Schema;

namespace EraaSoftCinema.Models
{
    public class Movie
    {
        [Key]
        public int id { get; set; }
        public bool status { get; set; }= true;
        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 100 characters")]
        public string? title { get; set; }
        [Required(ErrorMessage = "Description is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Description must be between 3 and 100 characters")]
        public string? description { get; set; }
        [Required(ErrorMessage = "Director name is required")]
        [StringLength(100, MinimumLength = 3)]
        public string? director { get; set; }
        [Required(ErrorMessage = "Release year is required")]
        [Range(2000, 2099, ErrorMessage = "Enter a valid release year")]
        public int releaseYear { get; set; }
        [Required(ErrorMessage = "Price is required")]
        [Range(1, 1000, ErrorMessage = "Price must be between 1 to 1000")]

        public decimal Price { get; set; }
        public string MainImgUrl { get; set; } = string.Empty;
        [Required(ErrorMessage = "Duration is required")]
        [Range(1, 500, ErrorMessage = "Duration must be in minutes")]
        public int Duration { get; set; }=0;
        [Required(ErrorMessage = "Genre is required")]
        [StringLength(50, MinimumLength = 3)]
        public string genre { get; set; } = string.Empty;
        [Required(ErrorMessage = "Category is required")]

        public int CategoryId { get; set; }


        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; } = default!;


        public int languageId { get; set; }


        [ForeignKey(nameof(languageId))]
        public Language  Language { get; set; } =default!;

        public List<MoviesActors> actorMovies { get; set; } = new List<MoviesActors>();

        public List<MovieSubimg> MovieSubimgs { get; set; } = new List<MovieSubimg>();







    }
}
