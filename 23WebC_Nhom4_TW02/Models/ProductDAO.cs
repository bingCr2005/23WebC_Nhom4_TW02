using _23WebC_Nhom4_TW02.Models;
namespace _23WebC_Nhom4_TW02
{
    public interface IProductDao { }

    public class ProductDao : IProductDao
    {
        private readonly IDataProvider _provider;

        public ProductDao(IDataProvider provider)
        {
            _provider = provider;
        }
    }
}
