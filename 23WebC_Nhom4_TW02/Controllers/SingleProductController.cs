using _23WebC_Nhom4_TW02.Models;
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

            //Thêm sản phẩm vào Recently Viewed
            AddToRecentlyViewedSession(id);

            // --- LẤY DANH SÁCH ẢNH ---
            product.Images = _productDao.GetImagesByProductId(id);

            // Lấy danh sách Recently Viewed từ session
            var viewedIdsStr = HttpContext.Session.GetString("RecentlyViewed");
            var viewedIds = string.IsNullOrEmpty(viewedIdsStr)
                            ? new List<int>()
                            : viewedIdsStr.Split(',').Select(int.Parse).ToList();

            //  Lấy chi tiết sản phẩm của các ID vừa xem, bỏ sản phẩm hiện tại ---
            // SingleProduct
            var recentProductsDetail = _productDao.GetRecentlyViewedForDetail(viewedIds)
                                                  .Where(p => p.ProductID != id)
                                                  .Take(4)
                                                  .ToList();


            //Lấy danh sách tag của sản phẩm
            var tagNames = _productDao.GetTagsByProductId(id);
            product.Tags = tagNames.Select(t => new Tag { TagName = t }).ToList();

            var model = new TrangChiTietViewModel
            {
                Product = product,
                RelatedProducts = _productDao.LaySanPhamTheoDanhMuc(product.CategoryID ?? 0)
                                             .Where(p => p.ProductID != id)
                                             .Take(6)
                                             .ToList(),
                RecentProducts = recentProductsDetail, // sản phẩm vừa xem
                RecentPosts = _productDao.GetRecentPosts(5) // lấy 5 bài viết mới nhất
            };


            return View(model);
        }
        // Lấy danh sách sản phẩm vừa xem từ session, bỏ sản phẩm hiện tại
        private List<Product> GetRecentlyViewedFromSession(int currentProductId)
        {
            var viewedIdsStr = HttpContext.Session.GetString("RecentlyViewed");
            if (string.IsNullOrEmpty(viewedIdsStr)) return new List<Product>();

            var viewedIds = viewedIdsStr.Split(',').Select(int.Parse).ToList();

            // Bỏ sản phẩm hiện tại
            viewedIds.Remove(currentProductId);

            return _productDao.GetRecentlyViewed(viewedIds);
        }

        // Hàm thêm sản phẩm vào session
        private void AddToRecentlyViewedSession(int id)
        {
            var viewedIdsStr = HttpContext.Session.GetString("RecentlyViewed");
            var viewedIds = string.IsNullOrEmpty(viewedIdsStr) ? new List<int>() : viewedIdsStr.Split(',').Select(int.Parse).ToList();

            viewedIds.Remove(id);        // xóa nếu đã tồn tại
            viewedIds.Insert(0, id);     // thêm đầu

            if (viewedIds.Count > 7)     // giới hạn 7 sản phẩm
                viewedIds = viewedIds.Take(7).ToList();

            HttpContext.Session.SetString("RecentlyViewed", string.Join(",", viewedIds));
        }
    }
}
