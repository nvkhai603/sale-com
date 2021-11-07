using Microsoft.AspNetCore.Identity;
using Nvk.Ddd.Domain;
using Nvk.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaleCom.Domain.Identity
{
    public class AppRole : IdentityRole<Guid>, IMultiTenant, IAggregateRoot, IHasConcurrencyStamp, ISoftDelete
    {
        public AppRole(): base()
        {

        }
        public AppRole(string name, Guid tenantId): base(name)
        {
            TenantId = tenantId;
            NormalizedName = name.ToUpper();
        }

        public Guid? TenantId { get; set; }
        public DateTime? CreationTime { get; set; }
        public Guid? CreatorId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public Guid? LastModifierId { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? DeletedId { get ; set ; }
        public DateTime? DeletionTime { get ; set ; }

        public object[] GetKeys()
        {
            throw new NotImplementedException();
        }
    }
}
