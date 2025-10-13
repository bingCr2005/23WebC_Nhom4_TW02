namespace _23WebC_Nhom4_TW02
{
    public class Review
    {
        public int ReviewID { get; set; }
        public int ProductID { get; set; }
        public int? UserID { get; set; }
        public string? GuestName { get; set; }
        public string? GuestEmail { get; set; }
        public byte? Rating { get; set; }
        public string? Content { get; set; }
        public bool IsApproved { get; set; } = false;
        public DateTime CreatedAt { get; set; }
    }
}
