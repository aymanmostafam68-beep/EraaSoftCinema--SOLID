using System.Threading.Tasks;
using EraaSoftCinema.Areas.Admin.Models;
using EraaSoftCinema.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace EraaSoftCinema.Areas.Admin.Controllers
{
    [Area(AreaSD.AdminArea)]

    public class CategoryController : Controller
    {
        private IRepo<Category> _repository; //= new Repository<Category>();
        private readonly IStringLocalizer<LocalizationController> _localizer;

        public CategoryController(IRepo<Category> repository, IStringLocalizer<LocalizationController> localizer)
        {
            _repository = repository;
            _localizer = localizer;
        }

        public async Task<IActionResult> Index(string? CategoryName, int page = 1)
        {
            var _categories = await _repository.GetAll(tracked: false);
            int totalItems = _categories.Count();
            var totalPages = (int)Math.Ceiling(totalItems / 12.0);



            if (CategoryName is not null)
            {
                _categories = _categories.Where(e => e.name.Contains(CategoryName)).ToList();
            }

            page = page < 1 ? 1 : page;
            page = page > totalPages ? 1 : page;


            _categories = _categories
                .Skip((page - 1) * 12)
                .Take(12).ToList();




            return View(new CategoryVM
            {
                Categories = _categories.AsEnumerable(),
                CurrentPage = page,
                TotalPages = totalPages
            }

            );
        }
        [ActionName("AddCategory")]
        public IActionResult AddCategory()
        {

            return View(new Category());
        }

        [HttpPost]

        [ActionName("AddCategory")]
        [HttpPost]
        public async Task<IActionResult> AddCategory(Category category, IFormFile file,string? location)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("ImgUrl", "Image file is required");
            }

            if (!ModelState.IsValid)
            {


                return View(category);

            }
            
            _repository.fileUpload(file, "Category", out string newFileName); 
            category.imgUrl = newFileName;

           await _repository.Create(category);
            int result = await _repository.Comment();
            if(result> 0)
            {
                TempData["Notification-success"] = _localizer["AddCategory-success"].Value;

            }
            else
            {
                TempData["Notification-error"] = _localizer["AddCategory-error"].Value;
            }



            return RedirectToAction("Index", "Category");
        }

        [ActionName("EditCategory")]
        public async Task<IActionResult> EditCategory([FromRoute] int id)
        {
            var category = await _repository.GetOne(c => c.id == id, tracked: false);
            return View(category);
        }

        [HttpPost]
        [ActionName("EditCategory")]
        public async Task<IActionResult> EditCategory(Category category , IFormFile? file,string? location)
        {
            if (category == null) return NotFound();

            if (!ModelState.IsValid)
            {
                if (file == null)
                {
                    ModelState.Remove("imgUrl");
                    ModelState.Remove("file");

                }
              

                if (!ModelState.IsValid)
                    TempData["Notification-error"] = _localizer["UpdateCategory-error"].Value;

                return View(category);


            }


            var existingCategory = await _repository.GetOne(c => c.id == category.id, tracked: false);




            if (file is not null && file.Length > 0)
            {
                _repository.fileUpload(file, "Category", out string newFileName);


                var fullpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Category", existingCategory.imgUrl);

                if (System.IO.File.Exists(fullpath))
                {
                    System.IO.File.Delete(fullpath);

                }
                    category.imgUrl = newFileName;

            }
            else
            {
                category.imgUrl = existingCategory.imgUrl;
            }
            
             await  _repository.update(category);

            int result = await _repository.Comment();
            if (result> 0)
            {
                TempData["Notification-success"] = _localizer["UpdateCategory-success"].Value;


            }
            else
            {
                TempData["Notification-error"] = _localizer["UpdateCategory-error"].Value;

            }




            return RedirectToAction(nameof(Index));

        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
        

            var category =await _repository.GetOne(c => c.id == id, tracked: false);
            if (category is null)  return NotFound();


            var fullpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot","img", "Category", category.imgUrl);
                if (System.IO.File.Exists(fullpath))
                {
                    System.IO.File.Delete(fullpath);
                }
             await _repository.Delete(category);
       int result =   await  _repository.Comment();
            if (result > 0)
            {

                TempData["Notification-success"] = _localizer["DeleteCategory-success"].Value;
            } else
            {
                TempData["Notification-error"] = _localizer["DeleteCategory-error"].Value;
            }

            return RedirectToAction("Index", "Category");

        }





    }
}
