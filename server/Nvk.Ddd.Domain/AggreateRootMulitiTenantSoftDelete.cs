using Nvk.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nvk.Ddd.Domain
{
    /// <summary>
    /// Class implement AggreateRoot<Tkey> SoftDelete and MulitiTenant.
    /// </summary>
    /// <typeparam name="TKey">Khóa chính.</typeparam>
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
