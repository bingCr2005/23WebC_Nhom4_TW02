using _23WebC_Nhom4_TW02.Models;
namespace _23WebC_Nhom4_TW02
{
    public class PostDAO
    {
        public interface IPostDao { }

        public class PostDao : IPostDao
        {
            private readonly IDataProvider _provider;

            public PostDao(IDataProvider provider)
            {
                _provider = provider;
            }
        }
    }
}
