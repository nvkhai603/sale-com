using System;
using System.Collections.Generic;
using System.Text;

namespace Nvk.Ddd.Domain
{
    public abstract class AggregateRoot : BasicAggregateRoot, IAggregateRoot, IHasConcurrencyStamp
    {
        public string ConcurrencyStamp { get ; set ; }
    }

    public abstract class AggregateRoot<TKey> : BasicAggregateRoot<TKey>, IAggregateRoot<TKey>, IHasConcurrencyStamp
    {
        public string ConcurrencyStamp { get ; set ; }
    }
}
