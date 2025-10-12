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
        List<Product> GetRecentlyViewed(List<int> productIds);// lay dssp ng dung xem gan day
        Product GetProductById(int id);// lay chi tiet sp theo ID
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

        // Optional: Map riêng cho Home page (không có UpdatedAt)
        private Product MapProductHome(SqlDataReader reader)
        {
            var product = MapProduct(reader);
            product.UpdatedAt = null;
            return product;
        }

        // Map chi tiết dùng đầy đủ UpdatedAt
        private Product MapProductDetail(SqlDataReader reader)
        {
            return MapProduct(reader);
        }
    }
}
