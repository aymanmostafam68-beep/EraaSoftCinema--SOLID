namespace EraaSoftCinema.Models
{
    // 1. The PrimaryKey attribute requires strings for property names or an expression
    [PrimaryKey(nameof(MovieId), nameof(ActorId))]
    public class MoviesActors
    {
        public int MovieId { get; set; }
        public int ActorId { get; set; }

        // Navigation Properties
        public Movie Movie { get; set; } = default!;
        public Actor Actor { get; set; } = default!;
    }
}