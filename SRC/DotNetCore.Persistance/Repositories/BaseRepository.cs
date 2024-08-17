using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCore.Persistance.Repositories
{
    public abstract class BaseRepository
    {
        private readonly string _connectionString;
        protected BaseRepository(string connectionstring)
        {
            _connectionString = connectionstring;
        }

        protected async Task<SqlConnection> GetOpenConnectionAsync()
        {
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }
    }
}