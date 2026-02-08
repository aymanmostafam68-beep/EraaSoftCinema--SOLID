using System.Runtime.InteropServices;
using System.Threading.Tasks;
using EraaSoftCinema.Models;
using EraaSoftCinema.Models.StrongVMs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace EraaSoftCinema.Areas.Admin.Controllers
{
    [Area(AreaSD.AdminArea)]

    public class MovieController : Controller
    {
        private IRepo<Movie> _repository;//= new Repositories.Repository<Movie>();
        private IRepo<Category> _categories;// = new Repositories.Repository<Category>();
        private IRepo<Language> _Languages;//= new Repositories.Repository<Language>();
        private IRepo<Actor> _actors;//= new Repositories.Repository<Actor>();
        private IRepo<MoviesActors> _MoviesActors;//= new Repositories.Repository<MoviesActors>();
        private IRepo<MovieSubimg> _MovieSubimg;// = new Repositories.Repository<MovieSubimg>();

        public MovieController(IRepo<Movie> repository, IRepo<Category> categories, IRepo<Language> languages, IRepo<Actor> actors, IRepo<MoviesActors> moviesActors, IRepo<MovieSubimg> movieSubimg)
        {
            _repository = repository;
            _categories = categories;
            _Languages = languages;
            _actors = actors;
            _MoviesActors = moviesActors;
            _MovieSubimg = movieSubimg;
        }

        public async Task<IActionResult> Index(string? MovieName, int page = 1)
        {
            var _Movies = await _repository.GetAll(null, includeFunc:q=>q.Include( c=>c.Category).Include(l => l.Language),tracked:false);

            if (!string.IsNullOrEmpty(MovieName))
            {
                _Movies = _Movies.Where(e => e.title.Contains(MovieName)).ToList();
            }



            int totalItems = _Movies.Count();
            var totalPages = (int)Math.Ceiling(totalItems / 20.0);
            page = page < 1 ? 1 : page;
            page = page > totalPages ? 1 : page;


            _Movies = _Movies
                .Skip((page - 1) * 20)
                .Take(20).ToList();

            return View(new MoviesVM
            {
                Movies = _Movies.AsEnumerable(),
                CurrentPage = page,
                TotalPages = totalPages,
                Categories = await _categories.GetAll(null, tracked: false)

            });

        }

        [ActionName("Edit")]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            var movieInfo = await _repository.GetOne(
      m => m.id == id,
      q => q
              .Include(e => e.Category)
              .Include(e => e.Language)
              .Include(e => e.MovieSubimgs)
              .Include(e => e.actorMovies)
              .ThenInclude(am => am.Actor), tracked: true);




            if (movieInfo == null) return NotFound();

            var viewModel = new MovieAllInfo
            {
                movie = movieInfo,
                CategoryName = movieInfo.Category?.name ?? "General",

                Categories = await _categories.GetAll(null, tracked: false),
             
                Languages = await _Languages.GetAll(null, tracked: false),

                actors = await _actors.GetAll(null, tracked: false)
            
            };

            return View(viewModel);

        }
        [HttpPost]
        public async Task<IActionResult> Edit(MovieAllInfo model, IFormFile? Mainfile, IFormFile[]? Subfiles, int[]? ActorsIds)
        {
            if (!ModelState.IsValid)
            {

                ModelState.Remove("movie.Category");
                ModelState.Remove("movie.Language");
                if (!ModelState.IsValid)
                {
                    model.Categories = await _categories.GetAll(null, tracked: false);

                    model.Languages = await _Languages.GetAll(null, tracked: false);


                    return View(model);
                }
            }

            if (model == null) return NotFound();

            var movie1 = await _repository.GetOne(
     m => m.id == model.movie.id, q => q
              .Include(e => e.Category)
              .Include(e => e.Language)
              .Include(e => e.MovieSubimgs)
              .Include(e => e.actorMovies)
              .ThenInclude(am => am.Actor), tracked: true
  );


            if (movie1 == null) return NotFound();

            movie1.title = model.movie.title;
            movie1.description = model.movie.description;
            movie1.director = model.movie.director;
            movie1.releaseYear = model.movie.releaseYear;
            movie1.Price = model.movie.Price;
            movie1.Duration = model.movie.Duration;
            movie1.genre = model.movie.genre;
            movie1.CategoryId = model.movie.CategoryId;
            movie1.status = model.movie.status;
            movie1.languageId = model.movie.languageId;


            // get the old actores imgurl name
            var oldactorMovies = movie1.actorMovies?
                                         .Select(x => x.Actor.id)
                                         .ToList()?? new List<int>();
            if (oldactorMovies.Any())
            {
                _MoviesActors.DeleteRange( movie1.actorMovies);
                await _repository.Comment();
            }


            _repository.fileUpload(Mainfile, "Movies\\Mimgs", out string newFileName);
            movie1.MainImgUrl = newFileName;
            await _repository.Comment();
            // add the new actores
            foreach (var actor in ActorsIds)
                {
             await   _MoviesActors.Create(new MoviesActors
                    {
                        MovieId = movie1.id,
                        ActorId = actor
                    });
                }

           await _MoviesActors.Comment();




            if (Subfiles != null && Subfiles.Any())
            {

                foreach (var subfile in Subfiles)
                {
                    foreach (var old in movie1.MovieSubimgs)
                    {
                        var oldPath = Path.Combine(
                            Directory.GetCurrentDirectory(), "wwwroot","img","Movies","Subimgs", old.SubImgUrl);

                        if (System.IO.File.Exists(oldPath))
                            System.IO.File.Delete(oldPath);
                    }

                }
                _MovieSubimg.DeleteRange(movie1.MovieSubimgs);


                _repository.filesUpload(Subfiles, location: "Movies\\Subimgs", newFileNames: out List<string> newFileNames);

                for (int i = 0; i < newFileNames.Count; i++)
                {
                    var movieSubimg = new MovieSubimg
                    {
                        MovieId = movie1.id,
                        SubImgUrl = newFileNames[i]
                    };
                  await  _MovieSubimg.Create(movieSubimg);
                }
                await _MovieSubimg.Comment();



            }



            return RedirectToAction(actionName: "Index", "Movie");

        }



        [ActionName("Create")]

        [HttpGet]
        public async Task<IActionResult> Create()
        {
          

          var model = new MovieAllInfo
            {
                movie = new Movie
                {
                    Category = new Category(),        
                    Language = new Language(),       
                    MovieSubimgs = new List<MovieSubimg>(),
                    actorMovies = new List<MoviesActors>()
                },

              Categories = await _categories.GetAll(null, tracked: false),

              Languages = await _Languages.GetAll(null, tracked: false),

              actors = await _actors.GetAll(null, tracked: false)

                         
                };


            
               return View(model);

            

        }





        [ActionName("Create")]
        [HttpPost]
        public async Task<IActionResult> Create(MovieAllInfo model, IFormFile? Mainfile, IFormFile[]? Subfiles, int[]? ActorsIds)
        {

           
           
            var movie = new Movie
            {
                title = model.movie.title,
                description = model.movie.description,
                director = model.movie.director,
                releaseYear = model.movie.releaseYear,
                Price = model.movie.Price,
                Duration = model.movie.Duration,
                genre = model.movie.genre,
                CategoryId = model.movie.CategoryId,
                status = model.movie.status,
                languageId = model.movie.languageId,
                MovieSubimgs = new List<MovieSubimg>()
            };

   

            _repository.fileUpload(Mainfile, "Movies\\Mimgs", out string newFileName);
             movie.MainImgUrl = newFileName;

                    await _repository.Create(movie);
            await _repository.Comment();

             _repository.filesUpload(Subfiles, location: "Movies\\Subimgs", newFileNames: out List<string> newFileNames);
            for (int i = 0; i < newFileNames.Count; i++)
            {
                var movieSubimg = new MovieSubimg
                {
                    MovieId = movie.id,
                    SubImgUrl = newFileNames[i]
                };
                await _MovieSubimg.Create(movieSubimg);
            }

          await _MovieSubimg.Comment();


          


            if (ActorsIds.Length>0)
            {
                foreach (var actor in ActorsIds)
                {
                  await  _MoviesActors.Create(new MoviesActors
                    {
                        MovieId = movie.id,
                        ActorId = actor
                    });
                }
            }




            await _MoviesActors.Comment();


            return RedirectToAction(actionName: "Index", "Movie");


        }


        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var movieInfo = await _repository.GetOne(m => m.id == id, includeFunc: q=>q.Include( m => m.MovieSubimgs).Include( m => m.actorMovies), tracked: false);

       
            if (movieInfo != null) {
               await _repository.Delete(movieInfo);
               await _repository.Comment();
            }

            return RedirectToAction("Index", "Movie");

        }

    }
}
