namespace _23WebC_Nhom4_TW02
{
    public class Post
    {
        public int PostID { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Slug { get; set; }
        public string? Summary { get; set; }
        public string? Content { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsPublished { get; set; } = true;
        public DateTime? PublishedAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
