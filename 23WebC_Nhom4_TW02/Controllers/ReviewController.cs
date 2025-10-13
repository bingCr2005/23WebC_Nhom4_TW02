using Microsoft.AspNetCore.Mvc;

namespace _23WebC_Nhom4_TW02.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IReviewDao _reviewDao;

        public ReviewController(IReviewDao reviewDao)
        {
            _reviewDao = reviewDao;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
