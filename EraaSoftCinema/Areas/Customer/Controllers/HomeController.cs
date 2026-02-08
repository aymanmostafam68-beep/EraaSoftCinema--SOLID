using System.Diagnostics;
using System.Threading.Tasks;
using EraaSoftCinema.Models;
using EraaSoftCinema.Models.StrongVMs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace EraaSoftCinema.Areas.Customer.Controllers
{
    [Area(AreaSD.CustomerArea)]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        Repositories.Repository<Movie> movieRepo = new Repositories.Repository<Movie>();

        ApplicationDB.DataAccess db = new ApplicationDB.DataAccess();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index(int page = 1,int? SelectedCategoryId = null)
        {
            var moviesQuery =  db.Movies
                .Include(m => m.Category)
                .Include(m => m.Language)
                .AsNoTracking().AsQueryable();
            if (SelectedCategoryId.HasValue && SelectedCategoryId>0)
            {
                moviesQuery = moviesQuery.Where(m => m.CategoryId == SelectedCategoryId.Value);
            }

            int totalItems = await moviesQuery.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / 12.0);
            page = page < 1 ? 1 : page;
            page = page > totalPages ? 1 : page;


            moviesQuery = moviesQuery
                .Skip((page - 1) * 12)
                .Take(12);

            return View(new MoviesVM {
                Movies = moviesQuery.AsEnumerable(),
                CurrentPage = page,
                TotalPages = totalPages,
                SelectedCategoryId = SelectedCategoryId,

                Categories = await db.MoviesCategories.AsNoTracking().ToListAsync()
            } );
        }

        [ActionName("MovieInfo")]
        public async Task<IActionResult> GetMoviebyId([FromRoute] int id)
        {

            var movieInfo = await db.Movies
                .Where(e => e.id == id)
                .Include(e => e.Language)
                .Include(e => e.Category)
                .Include(e => e.MovieSubimgs)
                .Include(e => e.actorMovies)
                .ThenInclude(am => am.Actor)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (movieInfo == null) return NotFound();

            var viewModel = new MovieAllInfo
            {
                movie = movieInfo,
                LanguageName = movieInfo.Language?.name ?? "Unknown",
                CategoryName = movieInfo.Category?.name ?? "General",
                actors = movieInfo.actorMovies?.Select(am => am.Actor).ToList() ?? new List<Actor>(),
                Categories = await db.MoviesCategories.AsNoTracking().ToListAsync()


            };


            return View(viewModel);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
