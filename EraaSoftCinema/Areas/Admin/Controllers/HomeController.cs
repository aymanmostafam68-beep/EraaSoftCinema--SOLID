using Microsoft.AspNetCore.Mvc;

namespace EraaSoftCinema.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        [Area(AreaSD.AdminArea)]
       
        public IActionResult Index()
        {
            return View();
        }
    }
}
