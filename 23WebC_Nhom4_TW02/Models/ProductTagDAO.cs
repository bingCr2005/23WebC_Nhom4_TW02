using _23WebC_Nhom4_TW02.Models;
namespace _23WebC_Nhom4_TW02
{
    public interface IProductTagDao { }

    public class ProductTagDao : IProductTagDao
    {
        private readonly IDataProvider _provider;

        public ProductTagDao(IDataProvider provider)
        {
            _provider = provider;
        }
    }
}
