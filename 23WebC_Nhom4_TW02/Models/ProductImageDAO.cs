using _23WebC_Nhom4_TW02.Models;
namespace _23WebC_Nhom4_TW02
{
    public interface IProductImageDao { }

    public class ProductImageDao : IProductImageDao
    {
        private readonly IDataProvider _provider;

        public ProductImageDao(IDataProvider provider)
        {
            _provider = provider;
        }
    }
}
