using Microsoft.AspNetCore.Mvc;

namespace _23WebC_Nhom4_TW02.Controllers
{
    public class SingleProductController : Controller
    {
        private readonly IProductDao _productDao;

        public SingleProductController(IProductDao productDao)
        {
            _productDao = productDao;
        }
        // Nhận id và trả về View SingleProduct/Index.cshtml
        public IActionResult Index(int id)
        {
            var product = _productDao.GetProductById(id);
            if (product == null) return NotFound();

            return View(product);
        }

    }
}
