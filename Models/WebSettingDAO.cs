using _23WebC_Nhom4_TW02.Models;
namespace _23WebC_Nhom4_TW02
{
    public interface IWebSettingDao { }

    public class WebSettingDao : IWebSettingDao
    {
        private readonly IDataProvider _provider;

        public WebSettingDao(IDataProvider provider)
        {
            _provider = provider;
        }
    }
}
