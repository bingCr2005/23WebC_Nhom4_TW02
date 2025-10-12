using _23WebC_Nhom4_TW02.Models;
using System.Data;
namespace _23WebC_Nhom4_TW02
{
    public interface IUserDao { }

    public class UserDao : IUserDao
    {
        private readonly IDataProvider _provider;

        public UserDao(IDataProvider provider)
        {
            _provider = provider;
        }
    }
}
