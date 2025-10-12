using Microsoft.AspNetCore.Mvc;

namespace _23WebC_Nhom4_TW02.Controllers
{
    public class CartController : Controller
    {

        private readonly ICartDao _cartDao;

        public CartController(ICartDao cartDao)
        {
            _cartDao = cartDao;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
