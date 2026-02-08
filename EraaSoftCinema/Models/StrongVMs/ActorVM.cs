namespace EraaSoftCinema.Models.StrongVMs
{
    public class ActorVM
    {
        public IEnumerable<Actor> Actors { get; set; } = default!;
        public int CurrentPage { get; set; } = 1;
        public double TotalPages { get; set; }
        public int PageCount { get; set; }
    }
}
