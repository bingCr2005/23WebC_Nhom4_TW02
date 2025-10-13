using _23WebC_Nhom4_TW02.Models;
namespace _23WebC_Nhom4_TW02
{
    public interface IOrderItemDao { }

    public class OrderItemDao : IOrderItemDao
    {
        private readonly IDataProvider _provider;

        public OrderItemDao(IDataProvider provider)
        {
            _provider = provider;
        }
    }
}
