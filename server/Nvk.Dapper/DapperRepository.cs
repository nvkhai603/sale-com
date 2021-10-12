using System;
using System.Data;
using System.Threading.Tasks;

namespace Nvk.Dapper
{
    public class DapperRepository : IDapperRepository
    {
        public Task<IDbConnection> GetDbConnectionAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IDbTransaction> GetDbTransactionAsync()
        {
            throw new NotImplementedException();
        }
    }
}
