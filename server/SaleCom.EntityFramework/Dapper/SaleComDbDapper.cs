using Microsoft.Extensions.Options;
using Nvk.Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaleCom.EntityFramework.Dapper
{
    public interface ISaleComDbDapper: IDbDapper
    { 
        
    }
    public class SaleComDbDapper : DbDapper, ISaleComDbDapper
    {
        public SaleComDbDapper(IOptions<ConnectionStringOption> options) : base(options.Value.Db)
        {
        }
    }
}
