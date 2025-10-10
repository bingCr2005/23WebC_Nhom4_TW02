using _23WebC_Nhom4_TW02.Models;
namespace _23WebC_Nhom4_TW02
{
    public interface ITagDao { }

    public class TagDao : ITagDao
    {
        private readonly IDataProvider _provider;

        public TagDao(IDataProvider provider)
        {
            _provider = provider;
        }
    }
}
