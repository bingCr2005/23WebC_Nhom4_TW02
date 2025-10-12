namespace _23WebC_Nhom4_TW02
{
    public class ProductImage
    {
        public int ImageID { get; set; }
        public int ProductID { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public int SortOrder { get; set; } = 0;
        public bool IsMain { get; set; } = false;
        public DateTime CreatedAt { get; set; }
    }
}
