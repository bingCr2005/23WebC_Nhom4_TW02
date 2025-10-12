using _23WebC_Nhom4_TW02.Models;
namespace _23WebC_Nhom4_TW02
{
    public interface IWishlistDao { }

    public class WishlistDao : IWishlistDao
    {
        private readonly IDataProvider _provider;

        public WishlistDao(IDataProvider provider)
        {
            _provider = provider;
        }
    }
}
