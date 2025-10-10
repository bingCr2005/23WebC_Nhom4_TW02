namespace _23WebC_Nhom4_TW02
{
    public class Coupon
    {
        public int CouponID { get; set; }
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal? DiscountPercent { get; set; }
        public decimal? DiscountAmount { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
