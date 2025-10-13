
using Microsoft.Data.SqlClient;
namespace _23WebC_Nhom4_TW02.Models
{
    public class Product
    {
        // Mã sản phẩm (khóa chính)
        public int ProductID { get; set; }

        // Mã danh mục (khóa ngoại)
        public int? CategoryID { get; set; }

        // Tên sản phẩm
        public string ProductName { get; set; } = string.Empty;

        // Đường dẫn SEO-friendly
        public string? Slug { get; set; }

        // Mô tả ngắn
        public string? ShortDescription { get; set; }

        // Mô tả chi tiết
        public string? Description { get; set; }

        // Giá gốc
        public decimal Price { get; set; }

        // Giá khuyến mãi
        public decimal? PriceSale { get; set; }

        // Tồn kho
        public int Stock { get; set; }

        // Ảnh đại diện
        public string? ThumbnailUrl { get; set; }

        // Trạng thái (Active / Hidden / OutOfStock)
        public string Status { get; set; } = "Active";

        // Ngày tạo
        public DateTime CreatedAt { get; set; }

        // Ngày cập nhật
        public DateTime? UpdatedAt { get; set; }


        public string CategoryName { get; set; }

        //đánh dấu sản phẩm mới
        public bool IsNew { get; set; } = false;

        public int SalesCount { get; set; } = 0;
        // Thêm Tag
        public List<Tag> Tags { get; set; } = new List<Tag>();

        // thêm danh sách ảnh
        public List<ProductImage> Images { get; set; } = new List<ProductImage>(); 
    }
}
