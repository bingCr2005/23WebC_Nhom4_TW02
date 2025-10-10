using Microsoft.AspNetCore.Mvc;

namespace _23WebC_Nhom4_TW02.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponDao _couponDao;

        public CouponController(ICouponDao couponDao)
        {
            _couponDao = couponDao;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
