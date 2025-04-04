using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WRRManagement.Infrastructure.Data
{
    public class WRRDbConnection : IWRRDbConnection
    {
        private readonly string _connectionString;

        public WRRDbConnection(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("WRRData");
        }
        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
