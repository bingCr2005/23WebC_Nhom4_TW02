using Microsoft.AspNetCore.Mvc;

namespace _23WebC_Nhom4_TW02.Controllers
{
    public class ProductImageController : Controller
    {
        private readonly IProductImageDao _productImageDao;

        public ProductImageController(IProductImageDao productImageDao)
        {
            _productImageDao = productImageDao;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
