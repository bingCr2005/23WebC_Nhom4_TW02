using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;
using _23WebC_Nhom4_TW02;
using System;

namespace _23WebC_Nhom4_TW02.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductManagementController : Controller
    {
        // Chuỗi kết nốis
        private readonly string _connectionString = "Server=(localdb)\\MSSQLLocalDB.;Database=UstoraDB;Trusted_Connection=True;";

        // ✅ Hiển thị danh sách (sẽ làm sau)
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Username") == null)
                return RedirectToAction("Login", "Account");

            return View();
        }

        // ✅ Form thêm sản phẩm
        [HttpGet]
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("Username") == null)
                return RedirectToAction("Login", "Account");

            return View();
        }

        // ✅ Xử lý lưu sản phẩm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            if (HttpContext.Session.GetString("Username") == null)
                return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(_connectionString))
                    {
                        conn.Open();

                        string sql = @"
                            INSERT INTO Product 
                            (CategoryID, ProductName, Slug, ShortDescription, Description, Price, PriceSale, Stock, ThumbnailUrl, Status, CreatedAt)
                            VALUES 
                            (@CategoryID, @ProductName, @Slug, @ShortDescription, @Description, @Price, @PriceSale, @Stock, @ThumbnailUrl, @Status, @CreatedAt)";

                        using (SqlCommand cmd = new SqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@CategoryID", (object?)product.CategoryID ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
                            cmd.Parameters.AddWithValue("@Slug", (object?)product.Slug ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@ShortDescription", (object?)product.ShortDescription ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@Description", (object?)product.Description ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@Price", product.Price);
                            cmd.Parameters.AddWithValue("@PriceSale", (object?)product.PriceSale ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@Stock", product.Stock);
                            cmd.Parameters.AddWithValue("@ThumbnailUrl", (object?)product.ThumbnailUrl ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@Status", product.Status);
                            cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);

                            cmd.ExecuteNonQuery();
                        }
                    }

                    TempData["Success"] = "Thêm sản phẩm thành công!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Lỗi khi thêm sản phẩm: " + ex.Message;
                }
            }

            return View(product);
        }
    }
}
