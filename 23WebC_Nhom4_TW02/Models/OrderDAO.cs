using _23WebC_Nhom4_TW02.Models;
namespace _23WebC_Nhom4_TW02
{
    public interface IOrderDao { }

    public class OrderDao : IOrderDao
    {
        private readonly IDataProvider _provider;

        public OrderDao(IDataProvider provider)
        {
            _provider = provider;
        }
    }
}
