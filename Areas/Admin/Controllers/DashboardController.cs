using Microsoft.AspNetCore.Mvc;

namespace _23WebC_Nhom04_BTNhom02.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            var user = HttpContext.Session.GetString("AdminUser");
            if (string.IsNullOrEmpty(user))
                return RedirectToAction("Index", "AdminLogin");

            ViewBag.AdminName = user;
            return View();
        }
    }
}
