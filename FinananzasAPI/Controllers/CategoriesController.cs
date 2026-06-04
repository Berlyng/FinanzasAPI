using Microsoft.AspNetCore.Mvc;

namespace FinananzasAPI.API.Controllers
{
    public class CategoriesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
