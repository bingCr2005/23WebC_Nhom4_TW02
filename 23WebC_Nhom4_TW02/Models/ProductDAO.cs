using _23WebC_Nhom4_TW02.Models;
using Microsoft.Data.SqlClient;
using System.Data;
namespace _23WebC_Nhom4_TW02
{
    //su dung DI để tách logic dữ liệu
    public interface IProductDao
    {
        List<Product> GetLatestProducts();//lay dssp moi nhat
        List<Product> GetTopSellers();// lay dssp ban chay nhat
        List<Product> GetTopNew();// lay dssp noi bat
        List<Product> GetRecentlyViewed(List<int> productIds);// lay dssp ng dung xem gan day(trang home)
        Product GetProductById(int id);// lay chi tiet sp theo ID

        List<Product> LaySanPhamTheoDanhMuc(int maDanhMuc);

        List<string> GetTagsByProductId(int productId);// Lay ds tag name dua vao productId

        // Lấy danh sách ảnh của sản phẩm theo ProductID
        List<ProductImage> GetImagesByProductId(int productId);

        // lay dssp ng dung xem gan day cho trang single
        List<Product> GetRecentlyViewedForDetail(List<int> productIds);

        // Lấy count bài viết mới nhất (5)
        List<Post> GetRecentPosts(int count);
    }   


    public class ProductDao : IProductDao
    {
        private readonly IDataProvider _provider;

        public ProductDao(IDataProvider provider)
        {
            _provider = provider;
        }

        public List<Product> GetLatestProducts()//lay top 6 sp active, sap xep theo ngày tạo mới nhất
        {
            var products = new List<Product>();
            string sql = @"
                SELECT TOP 6 p.ProductID, p.CategoryID, p.ProductName, p.Slug, p.ShortDescription,
                       p.Price, p.PriceSale, p.Stock, p.ThumbnailUrl, p.Status, p.CreatedAt,
                       c.CategoryName
                FROM Products p
                INNER JOIN Categories c ON p.CategoryID = c.CategoryID
                WHERE p.Status = 'Active' 
                ORDER BY p.CreatedAt DESC";

            using var reader = _provider.ExecuteReader(sql);

            //đọc từng dòng dữ liệu và gán sang đối tượng Product bằng mapProductHome()
            while (reader.Read())
            {
                products.Add(MapProductHome(reader)); // Home page không cần UpdatedAt
            }
            return products;
        }

        public List<Product> GetTopSellers()// đặt productID là 1,2,3 để demo vì trong đó chưa có ng mua hàng nên k biết dc topsellers
        {
            var products = new List<Product>();
            string sql = @"
                SELECT TOP 3 p.ProductID, p.CategoryID, p.ProductName, p.Slug, p.ShortDescription,
                       p.Price, p.PriceSale, p.Stock, p.ThumbnailUrl, p.Status, p.CreatedAt,
                       c.CategoryName
                FROM Products p
                INNER JOIN Categories c ON p.CategoryID = c.CategoryID
                LEFT JOIN OrderItems oi ON p.ProductID = oi.ProductID
                WHERE p.Status = 'Active' AND p.ProductID IN (1,2,3) 
                GROUP BY p.ProductID, p.CategoryID, p.ProductName, p.Slug, p.ShortDescription,
                         p.Price, p.PriceSale, p.Stock, p.ThumbnailUrl, p.Status, p.CreatedAt, c.CategoryName
               ";// ORDER BY SUM(ISNULL(oi.Quantity,0)) DESC

            using var reader = _provider.ExecuteReader(sql);
            while (reader.Read())
            {
                products.Add(MapProductHome(reader)); // Home page không cần UpdatedAt
            }
            return products;
        }

        public List<Product> GetTopNew()
        {
            var products = new List<Product>();
            string sql = @"
                SELECT TOP 3 p.ProductID, p.CategoryID, p.ProductName, p.Slug, p.ShortDescription,
                       p.Price, p.PriceSale, p.Stock, p.ThumbnailUrl, p.Status, p.CreatedAt,
                       c.CategoryName
                FROM Products p
                INNER JOIN Categories c ON p.CategoryID = c.CategoryID
                INNER JOIN ProductTags pt ON p.ProductID = pt.ProductID
                INNER JOIN Tags t ON pt.TagID = t.TagID
                WHERE p.Status = 'Active' AND t.TagName = N'New'
                ORDER BY p.CreatedAt DESC";

            using var reader = _provider.ExecuteReader(sql);
            while (reader.Read())
            {
                var product = MapProductHome(reader); // Home page không cần UpdatedAt
                product.IsNew = true; // đánh dấu sản phẩm mới
                products.Add(product);
            }
            return products;
        }

        public List<Product> GetRecentlyViewed(List<int> productIds)
        {
            if (!productIds.Any()) return new List<Product>();

            var products = new List<Product>();
            var thong_so_id = string.Join(",", productIds);
            string sql = $@"
                SELECT p.ProductID, p.CategoryID, p.ProductName, p.Slug, p.ShortDescription,
                       p.Price, p.PriceSale, p.Stock, p.ThumbnailUrl, p.Status, p.CreatedAt,
                       c.CategoryName
                FROM Products p
                INNER JOIN Categories c ON p.CategoryID = c.CategoryID
                WHERE p.ProductID IN ({thong_so_id}) AND p.Status = 'Active'
                ORDER BY p.CreatedAt DESC";

            using var reader = _provider.ExecuteReader(sql);
            while (reader.Read())
            {
                products.Add(MapProductHome(reader)); // Home page không cần UpdatedAt
            }
            return products;
        }


        public Product GetProductById(int id)
        {
            string sql = @"
                SELECT p.ProductID, p.CategoryID, p.ProductName, p.Slug, p.ShortDescription, p.Description,
                       p.Price, p.PriceSale, p.Stock, p.ThumbnailUrl, p.Status, p.CreatedAt, p.UpdatedAt,
                       c.CategoryName
                FROM Products p
                INNER JOIN Categories c ON p.CategoryID = c.CategoryID
                WHERE p.ProductID = @Id";

            using var reader = _provider.ExecuteReader(sql, new SqlParameter[] { new SqlParameter("@Id", id) });
            if (reader.Read())
            {
                return MapProductDetail(reader); // chi tiết dùng UpdatedAt
            }
            return null;
        }
        //làm cho trang singleProduct (sản phẩm liên quan)
        public List<Product> LaySanPhamTheoDanhMuc(int maDanhMuc)
        {
            var products = new List<Product>();
            string sql = @"
                SELECT p.ProductID, p.CategoryID, p.ProductName, p.Slug, p.ShortDescription, p.Description,
                       p.Price, p.PriceSale, p.Stock, p.ThumbnailUrl, p.Status, p.CreatedAt, p.UpdatedAt,
                       c.CategoryName
                FROM Products p
                INNER JOIN Categories c ON p.CategoryID = c.CategoryID
                WHERE p.CategoryID = @CategoryID AND p.Status = 'Active'
                ORDER BY p.CreatedAt DESC";

            using var reader = _provider.ExecuteReader(sql, new SqlParameter[] { new SqlParameter("@CategoryID", maDanhMuc) });

            while (reader.Read())
            {
                products.Add(MapProductDetail(reader)); // chi tiết sản phẩm đầy đủ
            }

            return products;
        }
        // Lấy danh sách tên Tag dựa vào ProductID
        public List<string> GetTagsByProductId(int productId)
        {
            string sql = @"
                SELECT t.TagName
                FROM Tags t
                INNER JOIN ProductTags pt ON t.TagID = pt.TagID
                WHERE pt.ProductID = @ProductID";

            var tags = new List<string>();

            using var reader = _provider.ExecuteReader(sql, new SqlParameter[]
            {
                 new SqlParameter("@ProductID", productId)
            });

            while (reader.Read())
            {
                // Đọc từng TagName từ DB và thêm vào danh sách
                tags.Add(reader["TagName"].ToString());
            }

            return tags;
        }
        // Lấy danh sách ảnh của sản phẩm theo ProductID
        public List<ProductImage> GetImagesByProductId(int productId)
        {
            var images = new List<ProductImage>();
            string sql = @"
                SELECT ImageID, ProductID, ImageUrl, SortOrder, IsMain
                FROM ProductImages
                WHERE ProductID = @ProductID
                ORDER BY SortOrder";

            using var reader = _provider.ExecuteReader(sql, new SqlParameter[]
            {
                 new SqlParameter("@ProductID", productId)
            });

            while (reader.Read())
            {
                images.Add(MapProductImage(reader)); // Dùng hàm map
            }

            return images;
        }
        //Lấy sản phẩm xem gần đây vào trang chitiet
        public List<Product> GetRecentlyViewedForDetail(List<int> productIds)
        {
            if (!productIds.Any()) return new List<Product>();

            var products = new List<Product>();
            var thong_so_id = string.Join(",", productIds);
            string sql = $@"
                SELECT p.ProductID, p.CategoryID, p.ProductName, p.Slug, p.ShortDescription,
                       p.Price, p.PriceSale, p.Stock, p.ThumbnailUrl, p.Status, p.CreatedAt, p.UpdatedAt,
                       c.CategoryName
                FROM Products p
                INNER JOIN Categories c ON p.CategoryID = c.CategoryID
                WHERE p.ProductID IN ({thong_so_id}) AND p.Status = 'Active'";

            using var reader = _provider.ExecuteReader(sql);
            while (reader.Read())
            {
                products.Add(MapProductDetail(reader)); // Dùng MapProductDetail để có Images + Tags
            }

            return products;
        }


        public List<Post> GetRecentPosts(int count)
        {
            var posts = new List<Post>();
            string sql = $@"
                SELECT TOP {count} PostID, Title, Slug, Summary, Content, ImageUrl, IsPublished, PublishedAt, CreatedAt
                FROM Posts
                WHERE IsPublished = 1
                ORDER BY CreatedAt DESC";

            using var reader = _provider.ExecuteReader(sql);
            while (reader.Read())
            {
                posts.Add(MapPost(reader));
            }

            return posts;
        }



        // Hàm ánh xạ SqlDataReader -> Product
        public static Product MapProduct(SqlDataReader reader)
        {
            // Hàm check xem cột có tồn tại không
            bool HasColumn(string name)
            {
                try { return reader.GetOrdinal(name) >= 0; }
                catch { return false; }
            }

            return new Product
            {
                ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),

                CategoryID = reader.IsDBNull(reader.GetOrdinal("CategoryID"))
                             ? (int?)null
                             : reader.GetInt32(reader.GetOrdinal("CategoryID")),

                ProductName = reader.GetString(reader.GetOrdinal("ProductName")),

                Slug = reader.IsDBNull(reader.GetOrdinal("Slug"))
                       ? null
                       : reader.GetString(reader.GetOrdinal("Slug")),

                ShortDescription = reader.IsDBNull(reader.GetOrdinal("ShortDescription"))
                                   ? null
                                   : reader.GetString(reader.GetOrdinal("ShortDescription")),

                Price = reader.GetDecimal(reader.GetOrdinal("Price")),

                PriceSale = reader.IsDBNull(reader.GetOrdinal("PriceSale"))
                            ? (decimal?)null
                            : reader.GetDecimal(reader.GetOrdinal("PriceSale")),

                Stock = reader.GetInt32(reader.GetOrdinal("Stock")),

                ThumbnailUrl = reader.IsDBNull(reader.GetOrdinal("ThumbnailUrl"))
                               ? null
                               : reader.GetString(reader.GetOrdinal("ThumbnailUrl")),

                Status = reader.GetString(reader.GetOrdinal("Status")),

                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),

                UpdatedAt = HasColumn("UpdatedAt") && !reader.IsDBNull(reader.GetOrdinal("UpdatedAt"))
                            ? reader.GetDateTime(reader.GetOrdinal("UpdatedAt"))
                            : (DateTime?)null, // an toàn

                CategoryName = reader.IsDBNull(reader.GetOrdinal("CategoryName"))? null: reader.GetString(reader.GetOrdinal("CategoryName"))
            };
        }

        // Hàm ánh xạ SqlDataReader -> Post
        private Post MapPost(SqlDataReader reader)
        {
            bool HasColumn(string name)
            {
                try { return reader.GetOrdinal(name) >= 0; }
                catch { return false; }
            }

            return new Post
            {
                PostID = reader.GetInt32(reader.GetOrdinal("PostID")),
                Title = reader.GetString(reader.GetOrdinal("Title")),
                Slug = reader.IsDBNull(reader.GetOrdinal("Slug")) ? null : reader.GetString(reader.GetOrdinal("Slug")),
                Summary = HasColumn("Summary") && !reader.IsDBNull(reader.GetOrdinal("Summary")) ? reader.GetString(reader.GetOrdinal("Summary")) : null,
                Content = HasColumn("Content") && !reader.IsDBNull(reader.GetOrdinal("Content")) ? reader.GetString(reader.GetOrdinal("Content")) : null,
                ImageUrl = HasColumn("ImageUrl") && !reader.IsDBNull(reader.GetOrdinal("ImageUrl")) ? reader.GetString(reader.GetOrdinal("ImageUrl")) : null,
                IsPublished = HasColumn("IsPublished") ? reader.GetBoolean(reader.GetOrdinal("IsPublished")) : true,
                PublishedAt = HasColumn("PublishedAt") && !reader.IsDBNull(reader.GetOrdinal("PublishedAt")) ? reader.GetDateTime(reader.GetOrdinal("PublishedAt")) : null,
                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
            };
        }

        private ProductImage MapProductImage(SqlDataReader reader)
        {
            return new ProductImage
            {
                ImageID = reader.GetInt32(reader.GetOrdinal("ImageID")),
                ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                SortOrder = reader.GetInt32(reader.GetOrdinal("SortOrder")),
                IsMain = reader.GetBoolean(reader.GetOrdinal("IsMain"))
            };
        }


        // Optional: Map riêng cho Home page (không có UpdatedAt)
        private Product MapProductHome(SqlDataReader reader)
        {
            var product = MapProduct(reader);
            product.UpdatedAt = null;

            // Gán ThumbnailUrl chính từ ProductImages nếu có
            var images = GetImagesByProductId(product.ProductID);
            var mainImage = images.FirstOrDefault(img => img.IsMain);
            if (mainImage != null)
                product.ThumbnailUrl = mainImage.ImageUrl;

            return product;
        }

        // Map chi tiết dùng đầy đủ UpdatedAt
        private Product MapProductDetail(SqlDataReader reader)
        {
            var product = MapProduct(reader);

            // Gán danh sách tag
            product.Tags = GetTagsByProductId(product.ProductID)
                            .Select(t => new Tag { TagName = t })
                            .ToList();

            // Gán danh sách ảnh
            product.Images = GetImagesByProductId(product.ProductID);

            // Gán ThumbnailUrl nếu cần (dùng ảnh chính)
            var mainImage = product.Images.FirstOrDefault(img => img.IsMain);
            if (mainImage != null)
                product.ThumbnailUrl = mainImage.ImageUrl;

            return product;
        }
    }
}
