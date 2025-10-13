using _23WebC_Nhom4_TW02.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;

namespace _23WebC_Nhom4_TW02.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminLoginController : Controller
    {
        private readonly IConfiguration _config;

        public AdminLoginController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public IActionResult Index()
        {
            // Nếu đã đăng nhập thì chuyển luôn về trang admin home
            if (HttpContext.Session.GetString("AdminUser") != null)
                return RedirectToAction("Index", "Home", new { area = "Admin" });

            return View();
        }

        [HttpPost]
        public IActionResult Index(UserLogin model)
        {
            if (!ModelState.IsValid)
                return View(model);

            string connectionString = _config.GetConnectionString("DefaultConnection");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"SELECT * FROM Users 
                                 WHERE Username = @Username 
                                 AND PasswordHash = @Password 
                                 AND Role = 'Admin' 
                                 AND IsActive = 1";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", model.Username);
                    cmd.Parameters.AddWithValue("@Password", model.Password);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Debug.WriteLine("✅ Đăng nhập thành công: " + model.Username);
                            HttpContext.Session.SetString("AdminUser", model.Username);
                            return RedirectToAction("Index", "Home", new { area = "Admin" });
                        }
                    }
                }
            }

            ViewBag.Error = "Sai tài khoản hoặc mật khẩu!";
            return View(model);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("AdminUser");
            return RedirectToAction("Index", "AdminLogin", new { area = "Admin" });
        }
    }
}
