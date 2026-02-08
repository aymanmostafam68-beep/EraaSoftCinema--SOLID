using System.Threading.Tasks;
using Azure;
using EraaSoftCinema.Models;
using EraaSoftCinema.Models.StrongVMs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EraaSoftCinema.Areas.Admin.Controllers
{
    [Area(AreaSD.AdminArea)]
    public class ActorController : Controller
    {
        private IRepo<Actor> _repository;//= new Repositories.Repository<Actor>();

        public ActorController(IRepo<Actor> repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Index(string? ActorName, int page = 1)
        {
            var Actores = await _repository.GetAll(tracked: false);
            int totalItems = Actores.Count();
            var totalPages = (int)Math.Ceiling(totalItems / 12.0);



            if (ActorName is not null)
            {
                Actores = Actores.Where(e => e.name.Contains(ActorName)).ToList();
            }

            page = page < 1 ? 1 : page;
            page = page > totalPages ? 1 : page;


            Actores = Actores
                .Skip((page - 1) * 12)
                .Take(12).ToList();




            return View(new ActorVM
            {
                Actors = Actores.AsEnumerable(),
                CurrentPage = page,
                TotalPages = totalPages
            }

            );
        }
        [ActionName("Edit")]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            var category = await _repository.GetOne(c => c.id == id, tracked: false);
            return View(category);
           
        }
        [HttpPost]
        [ActionName("Edit")]
        public async Task<IActionResult> Edit(Actor actor, IFormFile file)
        {
            if (actor is not null)
            {
                if (!ModelState.IsValid)
                {
                    if (file == null)
                    {
                        ModelState.Remove("imgUrl");
                        ModelState.Remove("file");

                    }


                    if (!ModelState.IsValid) return View(actor);

                }
                var existingActor = await _repository.GetOne(c => c.id == actor.id, tracked: false);



                if (file is not null && file.Length > 0)
                {
                    _repository.fileUpload(file, "Actors", out string newFileName);


                    var fullpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Actors", existingActor.imgUrl);

                    if (System.IO.File.Exists(fullpath))
                    {
                        System.IO.File.Delete(fullpath);

                    }
                    actor.imgUrl = newFileName;

                }
                else
                {
                    actor.imgUrl = existingActor.imgUrl;
                }

                _repository.update(actor);

                await _repository.Comment();
            }
            return RedirectToAction(nameof(Index));

        }

        [ActionName("Create")]
        public async Task<IActionResult> Create(Actor actor, IFormFile file)
        {
            if (actor is null) return NotFound();
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("ImgUrl", "Image file is required");
            }

            if (!ModelState.IsValid)
            {
                return View(actor);
            }



            _repository.fileUpload(file, "Actors", out string newFileName);
            actor.imgUrl = newFileName;

            await _repository.Create(actor);
            await _repository.Comment();

            return RedirectToAction("Index", "Actor");

        }
        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var actor = await _repository.GetOne(c => c.id == id, tracked: false);
            if (actor is null) return NotFound();


            var fullpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Actors", actor.imgUrl);
            if (System.IO.File.Exists(fullpath))
            {
                System.IO.File.Delete(fullpath);
            }
            _repository.Delete(actor);
            await _repository.Comment();

            return RedirectToAction("Index", "Actor");

        }


    }
}
