using System.Data;
using System.Data.SqlClient;
namespace _23WebC_Nhom4_TW02.Models
{
 
 
        public interface IDataProvider
        {
            IDbConnection CreateConnection();
        }

        public class DataProvider : IDataProvider
        {
            private readonly string _connectionString;

            public DataProvider(IConfiguration configuration)
            {
                _connectionString = configuration.GetConnectionString("DefaultConnection");
            }

            public IDbConnection CreateConnection()
            {
                return new SqlConnection(_connectionString);
            }
        }
  }

