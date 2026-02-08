namespace EraaSoftCinema.Models.StrongVMs
{
    public class CategoryVM
    {
        public IEnumerable<Category> Categories { get; set; } = default!;
        public int CurrentPage { get; set; } = 1;
        public double TotalPages { get; set; }
        public int PageCount { get; set; }
    }
}
