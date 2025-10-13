using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;
using System;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;

namespace _23WebC_Nhom4_TW02.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductManagementController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _env;
        public ProductManagementController(IConfiguration config, IWebHostEnvironment env)
        {
            _config = config;
            _env = env;
        }

        // GET: /Admin/ProductManagement
        public IActionResult Index()
        {
            return View();
        }

        // GET: Create
        [HttpGet]
        public IActionResult Create()
        {
            var categories = new List<(int Id, string Name)>();
            var connStr = _config.GetConnectionString("DefaultConnection");
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (var cmd = new SqlCommand("SELECT CategoryID, CategoryName FROM Categories WHERE IsActive = 1", conn))
                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        categories.Add((Convert.ToInt32(rd["CategoryID"]), rd["CategoryName"].ToString()));
                    }
                }
            }
            ViewBag.Categories = categories;
            return View();
        }

        // POST Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IFormCollection form, List<IFormFile> Images)
        {
            string productName = form["ProductName"];
            string slug = form["Slug"];
            string shortDesc = form["ShortDescription"];
            string desc = form["Description"];
            string priceStr = form["Price"];
            string priceSaleStr = form["PriceSale"];
            string stockStr = form["Stock"];
            string status = form["Status"];
            string categoryIdStr = form["CategoryID"];
            string newCategoryName = form["NewCategoryName"];

            if (string.IsNullOrWhiteSpace(productName))
            {
                ViewBag.Error = "Tên sản phẩm không được để trống.";
                return Create();
            }

            if (string.IsNullOrWhiteSpace(slug))
            {
                slug = productName.ToLower().Trim().Replace(" ", "-");
            }

            if (!decimal.TryParse(priceStr, out decimal price)) price = 0;
            if (!decimal.TryParse(priceSaleStr, out decimal priceSale)) priceSale = price;
            if (!int.TryParse(stockStr, out int stock)) stock = 0;

            int categoryId = 0;
            var connStr = _config.GetConnectionString("DefaultConnection");
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                if (!string.IsNullOrWhiteSpace(newCategoryName))
                {
                    string insertCat = "INSERT INTO Category (CategoryName, Slug, Description, IsActive, CreatedAt) VALUES (@name, @slug, @desc, 1, GETDATE()); SELECT SCOPE_IDENTITY();";
                    using (var cmd = new SqlCommand(insertCat, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", newCategoryName);
                        cmd.Parameters.AddWithValue("@slug", newCategoryName.ToLower().Trim().Replace(" ", "-"));
                        cmd.Parameters.AddWithValue("@desc", newCategoryName);
                        var idObj = cmd.ExecuteScalar();
                        categoryId = Convert.ToInt32(idObj);
                    }
                }
                else
                {
                    int.TryParse(categoryIdStr, out categoryId);
                }

                // check duplicates
                using (var chk = new SqlCommand("SELECT COUNT(*) FROM Products WHERE ProductName = @name OR Slug = @slug", conn))
                {
                    chk.Parameters.AddWithValue("@name", productName);
                    chk.Parameters.AddWithValue("@slug", slug);
                    int exists = Convert.ToInt32(chk.ExecuteScalar());
                    if (exists > 0)
                    {
                        ViewBag.Error = "ProductName hoặc Slug đã tồn tại.";
                        return Create();
                    }
                }

                string thumb = "/img/default-product.jpg";
                string insertProd = @"INSERT INTO Products (CategoryID, ProductName, Slug, ShortDescription, Description, Price, PriceSale, Stock, ThumbnailUrl, Status, CreatedAt)
                                      VALUES (@cat, @name, @slug, @shortDesc, @desc, @price, @priceSale, @stock, @thumb, @status, GETDATE());
                                      SELECT SCOPE_IDENTITY();";
                int productId = 0;
                using (var cmd = new SqlCommand(insertProd, conn))
                {
                    cmd.Parameters.AddWithValue("@cat", categoryId);
                    cmd.Parameters.AddWithValue("@name", productName);
                    cmd.Parameters.AddWithValue("@slug", slug);
                    cmd.Parameters.AddWithValue("@shortDesc", (object)shortDesc ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@desc", (object)desc ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@price", price);
                    cmd.Parameters.AddWithValue("@priceSale", priceSale);
                    cmd.Parameters.AddWithValue("@stock", stock);
                    cmd.Parameters.AddWithValue("@thumb", thumb);
                    cmd.Parameters.AddWithValue("@status", string.IsNullOrWhiteSpace(status) ? "Active" : status);
                    var idObj = cmd.ExecuteScalar();
                    productId = Convert.ToInt32(idObj);
                }

                // save images
                if (Images != null && Images.Count > 0)
                {
                    string wwwRoot = Directory.GetCurrentDirectory();
                    string imgFolder = Path.Combine(wwwRoot, "wwwroot", "img");
                    if (!Directory.Exists(imgFolder)) Directory.CreateDirectory(imgFolder);

                    int order = 0;
                    foreach (var file in Images)
                    {
                        if (file == null || file.Length == 0) continue;
                        order++;
                        string ext = Path.GetExtension(file.FileName);
                        string fileName = $"product-{productId}-{Guid.NewGuid().ToString().Substring(0,8)}{ext}";
                        string savePath = Path.Combine(imgFolder, fileName);
                        using (var stream = new FileStream(savePath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                        string url = "/img/" + fileName;

                        string insertImg = "INSERT INTO ProductImages (ProductID, ImageUrl, SortOrder, IsMain, CreatedAt) VALUES (@pid, @url, @sort, @isMain, GETDATE())";
                        using (var icmd = new SqlCommand(insertImg, conn))
                        {
                            icmd.Parameters.AddWithValue("@pid", productId);
                            icmd.Parameters.AddWithValue("@url", url);
                            icmd.Parameters.AddWithValue("@sort", order);
                            icmd.Parameters.AddWithValue("@isMain", order == 1 ? 1 : 0);
                            icmd.ExecuteNonQuery();
                        }

                        if (order == 1)
                        {
                            using (var up = new SqlCommand("UPDATE Products SET ThumbnailUrl = @t WHERE ProductID = @pid", conn))
                            {
                                up.Parameters.AddWithValue("@t", url);
                                up.Parameters.AddWithValue("@pid", productId);
                                up.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }

            TempData["Success"] = "Thêm sản phẩm thành công!";
            return RedirectToAction("Index");
        }
    }
}
