using System.ComponentModel.DataAnnotations.Schema;

namespace EraaSoftCinema.Models
{
    [PrimaryKey(nameof(MovieId), nameof(SubImgUrl))]

    public class MovieSubimg
    {
        public string SubImgUrl { get; set; } = string.Empty;
        public int MovieId { get; set; }

        [ForeignKey(nameof(MovieId))]
        public Movie movie { get; set; } = default!;

    }
}
