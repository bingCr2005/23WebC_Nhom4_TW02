using _23WebC_Nhom4_TW02.Models;
namespace _23WebC_Nhom4_TW02
{
    public interface ICartDao { }

    public class CartDao : ICartDao
    {
        private readonly IDataProvider _provider;

        public CartDao(IDataProvider provider)
        {
            _provider = provider;
        }
    }
}
