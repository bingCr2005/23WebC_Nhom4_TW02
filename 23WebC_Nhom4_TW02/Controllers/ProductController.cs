using Microsoft.AspNetCore.Mvc;

namespace _23WebC_Nhom4_TW02.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductDao _productDao;

        public ProductController(IProductDao productDao)
        {
            _productDao = productDao;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
