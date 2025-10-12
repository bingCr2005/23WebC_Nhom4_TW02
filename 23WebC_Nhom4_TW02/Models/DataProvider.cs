using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;// Cho IConfiguration để lấy connection string từ appsettings.json

namespace _23WebC_Nhom4_TW02.Models
{
    public interface IDataProvider
    {
        IDbConnection CreateConnection();
        SqlDataReader ExecuteReader(string sql, SqlParameter[] parameters = null);
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

        public SqlDataReader ExecuteReader(string sql, SqlParameter[] parameters = null)
        {
            var conn = new SqlConnection(_connectionString);
            conn.Open();
            var cmd = new SqlCommand(sql, conn);
            if (parameters != null)
            {
                cmd.Parameters.AddRange(parameters);
            }
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }
    }
}