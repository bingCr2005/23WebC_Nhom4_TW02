using Microsoft.AspNetCore.Mvc;

namespace _23WebC_Nhom4_TW02.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
