namespace EraaSoftCinema.Models.StrongVMs
{
    public class MoviesVM
    {
        public  IEnumerable<Movie> Movies { get; set; } = new List<Movie>();
        public IEnumerable<Category> Categories { get; set; } = new List<Category>();
        public int CurrentPage { get; set; } = 1;
        public double TotalPages { get; set; }
        public int PageCount { get; set; }
       public int?  SelectedCategoryId { get; set; }

    }
}
