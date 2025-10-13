namespace _23WebC_Nhom4_TW02.Models
{
    public class TrangChiTietViewModel
    {
        public Product Product { get; set; }  // sản phẩm chính
        public List<Product> RecentProducts { get; set; } = new List<Product>(); // sản phẩm gần đây

        public List<Post> RecentPosts { get; set; }       // Bài viết mới
        public List<Product> RelatedProducts { get; set; } = new List<Product>();  // sản phẩm liên quan


    }
}
