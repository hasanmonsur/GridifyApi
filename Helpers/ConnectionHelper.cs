using System.Data;
using System.Data.SqlClient;

namespace WebApiGridify.Helpers
{
    public class DapperContext
    {

        private readonly string _connectionString;

        public DapperContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
