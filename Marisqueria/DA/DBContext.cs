using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace DA
{
    public class DBContext
    {
        private readonly IConfiguration _configuration;

        public DBContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection CrearConexion()
        {
            return new SqlConnection(_configuration.GetConnectionString("BD"));
        }
    }
}