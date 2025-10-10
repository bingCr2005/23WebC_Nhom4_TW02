using _23WebC_Nhom4_TW02.Models;
namespace _23WebC_Nhom4_TW02
{
    public interface ICategoryDao { }

    public class CategoryDao : ICategoryDao
    {
        private readonly IDataProvider _provider;

        public CategoryDao(IDataProvider provider)
        {
            _provider = provider;
        }
    }
}
