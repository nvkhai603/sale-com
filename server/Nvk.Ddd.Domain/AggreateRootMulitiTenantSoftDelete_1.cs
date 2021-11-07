using Nvk.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nvk.Ddd.Domain
{
    public class AggreateRootMulitiTenantSoftDelete<TKey> : AggregateRoot<TKey>, ISoftDelete, IMultiTenant
    {
        /// <summary>
        /// System.
        /// </summary>
        public Guid? TenantId { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? DeletedId { get; set; }
        public DateTime? DeletionTime { get; set; }
    }
}
