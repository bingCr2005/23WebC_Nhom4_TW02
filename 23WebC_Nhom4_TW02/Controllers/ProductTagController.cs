using Microsoft.AspNetCore.Mvc;

namespace _23WebC_Nhom4_TW02.Controllers
{
    public class ProductTagController : Controller
    {
        private readonly IProductTagDao _productTagDao;

        public ProductTagController(IProductTagDao productTagDao)
        {
            _productTagDao = productTagDao;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
