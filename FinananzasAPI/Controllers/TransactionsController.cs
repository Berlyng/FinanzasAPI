using Microsoft.AspNetCore.Mvc;

namespace FinananzasAPI.API.Controllers
{
    public class TransactionsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
