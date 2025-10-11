using static System.Net.Mime.MediaTypeNames;
using System.Text.Json;

namespace _23WebC_Nhom4_TW02.Middleware
{
    public class DataLoader
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _env;
        public static dynamic? JsonData { get; private set; }

        public DataLoader(RequestDelegate next, IWebHostEnvironment env)
        {
            _next = next;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (JsonData == null)
            {
                string filePath = Path.Combine(_env.ContentRootPath, "db.json");
                if (File.Exists(filePath))
                {
                    string jsonContent = await File.ReadAllTextAsync(filePath);
                    JsonData = JsonSerializer.Deserialize<dynamic>(jsonContent);
                    Console.WriteLine("✅ DataLoaderMiddleware: Dữ liệu đã được nạp từ db.json");
                }
                else
                {
                    Console.WriteLine("⚠️ Không tìm thấy file db.json");
                }
            }

            await _next(context);
        }
    }
    public static class DataLoaderMiddlewareExtensions
    {
        public static IApplicationBuilder UseDataLoader(this IApplicationBuilder app)
        {
            return app.UseMiddleware<DataLoader>();
        }
    }
}
