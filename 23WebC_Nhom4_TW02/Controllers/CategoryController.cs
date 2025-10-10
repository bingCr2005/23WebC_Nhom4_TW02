using Microsoft.AspNetCore.Mvc;

namespace _23WebC_Nhom4_TW02.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryDao _categoryDao;

        public CategoryController(ICategoryDao categoryDao)
        {
            _categoryDao = categoryDao;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
