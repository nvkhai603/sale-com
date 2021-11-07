using Microsoft.Extensions.Options;
using Nvk.Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaleCom.EntityFramework.Dapper
{
    public interface IIdDbDapper : IDbDapper
    {

    }
    public class IdDbDapper : DbDapper, IIdDbDapper
    {
        public IdDbDapper(IOptions<ConnectionStringOption> options) : base(options.Value.IdDb)
        {
        }
    }
}
