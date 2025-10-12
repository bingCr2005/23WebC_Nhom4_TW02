using Microsoft.AspNetCore.Mvc;

namespace _23WebC_Nhom4_TW02.Controllers
{
    public class CartItemController : Controller
    {
        private readonly ICartItemDao _cartItemDao;

        public CartItemController(ICartItemDao cartItemDao)
        {
            _cartItemDao = cartItemDao;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
