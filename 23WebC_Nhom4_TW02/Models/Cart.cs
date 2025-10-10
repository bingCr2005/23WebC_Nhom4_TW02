namespace _23WebC_Nhom4_TW02
{
    public class Cart
    {
        public int CartID { get; set; }
        public int? UserID { get; set; }
        public string? SessionToken { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
