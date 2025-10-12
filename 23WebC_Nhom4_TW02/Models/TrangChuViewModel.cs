namespace _23WebC_Nhom4_TW02.Models
{
    public class TrangChuViewModel
    {
        public List<Product> LatestProducts { get; set; } = new List<Product>();
        public List<Product> TopSellers { get; set; } = new List<Product>();
        public List<Product> RecentlyViewed { get; set; } = new List<Product>();
        public List<Product> TopNew { get; set; } = new List<Product>();
    }
}
