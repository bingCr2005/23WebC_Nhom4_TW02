using _23WebC_Nhom4_TW02.Models;  
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;  // Để sử dụng List<Product>
using System.Diagnostics;  // Để sử dụng Activity.Current trong Error

namespace _23WebC_Nhom4_TW02.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductDao _productDao;
        public HomeController(ILogger<HomeController> logger, IProductDao productDao)
        {
            _logger = logger;
            _productDao = productDao;
        }

        // Index: Load dữ liệu dynamic từ DB và truyền vào View
        public IActionResult Index()
        {
            var model = new TrangChuViewModel
            {
                LatestProducts = _productDao.GetLatestProducts(),  // TOP 6 sản phẩm mới
                TopSellers = _productDao.GetTopSellers(),          // TOP 3 sản phẩm bán chạy
                TopNew = _productDao.GetTopNew(),                  // TOP 3 sản phẩm mới (với tag 'New')
                RecentlyViewed = GetRecentlyViewedFromSession()    // Từ Session (ban đầu rỗng)
            };
            return View(model);
        }

        // Details: Load sản phẩm đơn lẻ theo ID, thêm vào Session RecentlyViewed
        //public IActionResult Details(int id)
        //{
        //    var product = _productDao.GetProductById(id);
        //    if (product == null)
        //    {
        //        return NotFound("Sản phẩm không tồn tại!");
        //    }

        //    // Thêm vào Session RecentlyViewed (giới hạn 3)
        //    AddToRecentlyViewedSession(id);

        //    return View(product);  // Tạo Views/Home/Details.cshtml nếu cần
        //}

        // Helper: Load RecentlyViewed từ Session

        //Chỉ cần hàm lấy Recently Viewed từ session để hiển thị,
        private List<Product> GetRecentlyViewedFromSession()
        {
            var viewedIdsStr = HttpContext.Session.GetString("RecentlyViewed");
            if (string.IsNullOrEmpty(viewedIdsStr)) return new List<Product>();

            var viewedIds = viewedIdsStr.Split(',').Select(int.Parse).ToList();
            return _productDao.GetRecentlyViewed(viewedIds);
        }

        // Helper: Thêm sản phẩm vào Session RecentlyViewed (thêm đầu, giới hạn 3)
        //private void AddToRecentlyViewedSession(int id)
        //{
        //    var viewedIdsStr = HttpContext.Session.GetString("RecentlyViewed");
        //    var viewedIds = string.IsNullOrEmpty(viewedIdsStr) ? new List<int>() : viewedIdsStr.Split(',').Select(int.Parse).ToList();

        //    // Xóa nếu đã tồn tại
        //    viewedIds.Remove(id);

        //    // Thêm vào đầu
        //    viewedIds.Insert(0, id);

        //    // Giới hạn 3
        //    if (viewedIds.Count > 3)
        //    {
        //        viewedIds = viewedIds.Take(3).ToList();
        //    }

        //    HttpContext.Session.SetString("RecentlyViewed", string.Join(",", viewedIds));
        //}

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}