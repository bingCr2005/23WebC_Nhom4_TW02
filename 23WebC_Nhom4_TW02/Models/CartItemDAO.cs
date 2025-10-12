using _23WebC_Nhom4_TW02.Models;
namespace _23WebC_Nhom4_TW02
{
    public interface ICartItemDao { }

    public class CartItemDao : ICartItemDao
    {
        private readonly IDataProvider _provider;

        public CartItemDao(IDataProvider provider)
        {
            _provider = provider;
        }
    }
}
