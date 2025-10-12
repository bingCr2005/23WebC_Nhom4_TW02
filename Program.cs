using static _23WebC_Nhom4_TW02.PostDAO;
using _23WebC_Nhom4_TW02.Models;
namespace _23WebC_Nhom4_TW02
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<IDataProvider, DataProvider>();

            builder.Services.AddSession();

            // DI người dùng
            builder.Services.AddScoped<IUserDao, UserDao>();

            // DI danh mục sản phẩm
            builder.Services.AddScoped<ICategoryDao, CategoryDao>();

            // DI tag sản phẩm
            builder.Services.AddScoped<ITagDao, TagDao>();

            // DI sản phẩm
            builder.Services.AddScoped<IProductDao, ProductDao>();

            // DI liên kết sản phẩm - tag
            builder.Services.AddScoped<IProductTagDao, ProductTagDao>();

            // DI hình ảnh sản phẩm
            builder.Services.AddScoped<IProductImageDao, ProductImageDao>();

            // DI đánh giá sản phẩm
            builder.Services.AddScoped<IReviewDao, ReviewDao>();

            // DI giỏ hàng
            builder.Services.AddScoped<ICartDao, CartDao>();

            // DI chi tiết giỏ hàng
            builder.Services.AddScoped<ICartItemDao, CartItemDao>();

            // DI danh sách yêu thích
            builder.Services.AddScoped<IWishlistDao, WishlistDao>();

            // DI đơn hàng
            builder.Services.AddScoped<IOrderDao, OrderDao>();

            // DI chi tiết đơn hàng
            builder.Services.AddScoped<IOrderItemDao, OrderItemDao>();

            // DI bài viết
            builder.Services.AddScoped<IPostDao, PostDao>();

            // DI mã giảm giá
            builder.Services.AddScoped<ICouponDao, CouponDao>();

            // DI website
            builder.Services.AddScoped<IWebSettingDao, WebSettingDao>();
     
            //  Cấu hình Session
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession(); // ✅ bắt buộc để dùng HttpContext.Session

            app.UseAuthorization();

            DTProvider.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // ✅ ROUTE cho Area (phải đặt trước default)
            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            // ✅ ROUTE mặc định (cho người dùng)
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
