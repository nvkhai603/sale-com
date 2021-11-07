using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Nvk.Dapper
{
    public class DbDapper : IDbDapper
    {
        private readonly IDbConnection _connection;
        private readonly string _connectionString;
        private bool _disposed = false;
        public DbDapper(string connectionString)
        {
            _connectionString = connectionString;
            _connection = new MySqlConnection(_connectionString);
            _connection.Open();
        }

        public IDbConnection Connection 
        { 
            get {  return _connection; }
        }


        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _connection.Dispose();
            }

            _disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
