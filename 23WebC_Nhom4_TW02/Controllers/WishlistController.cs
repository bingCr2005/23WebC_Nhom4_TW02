using Microsoft.AspNetCore.Mvc;

namespace _23WebC_Nhom4_TW02.Controllers
{
    public class WishlistController : Controller
    {
        private readonly IWishlistDao _wishlistDao;

        public WishlistController(IWishlistDao wishlistDao)
        {
            _wishlistDao = wishlistDao;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
