﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Nvk.Dapper
{
    public interface IDapperRepository
    {
        Task<IDbConnection> GetDbConnectionAsync();

        Task<IDbTransaction> GetDbTransactionAsync();
    }
}
