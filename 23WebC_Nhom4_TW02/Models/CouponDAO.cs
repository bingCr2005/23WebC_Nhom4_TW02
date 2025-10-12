using _23WebC_Nhom4_TW02.Models;
namespace _23WebC_Nhom4_TW02
{
    public interface ICouponDao { }

    public class CouponDao : ICouponDao
    {
        private readonly IDataProvider _provider;

        public CouponDao(IDataProvider provider)
        {
            _provider = provider;
        }
    }
}
