using Microsoft.AspNetCore.Mvc;
using _23WebC_Nhom4_TW02.Models;
using _23WebC_Nhom4_TW02.Filters;
using System.Data;

namespace _23WebC_Nhom4_TW02.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AdminAuthFilter] // Ép buộc đăng nhập
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var user = HttpContext.Session.GetString("AdminUser");
            ViewBag.AdminName = user;

            List<Product> products = new List<Product>();
            DataTable data = DTProvider.ExecuteQuery("SELECT * FROM Products");

            foreach (DataRow row in data.Rows)
            {
                products.Add(new Product
                {
                    ProductID = Convert.ToInt32(row["ProductID"]),
                    ProductName = row["ProductName"].ToString(),
                    Price = Convert.ToDecimal(row["Price"]),
                    Stock = Convert.ToInt32(row["Stock"]),
                    Status = row["Status"].ToString()
                });
            }

            return View(products);
        }
    }
}
