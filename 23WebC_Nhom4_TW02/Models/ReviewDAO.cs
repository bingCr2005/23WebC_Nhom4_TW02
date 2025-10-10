using _23WebC_Nhom4_TW02.Models;
namespace _23WebC_Nhom4_TW02
{
    public interface IReviewDao { }

    public class ReviewDao : IReviewDao
    {
        private readonly IDataProvider _provider;

        public ReviewDao(IDataProvider provider)
        {
            _provider = provider;
        }
    }
}
