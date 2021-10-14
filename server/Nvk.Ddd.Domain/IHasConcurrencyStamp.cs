using System;
using System.Collections.Generic;
using System.Text;

namespace Nvk.Ddd.Domain
{
    public interface IHasConcurrencyStamp
    {
        string ConcurrencyStamp { get; set; }
    }
}
