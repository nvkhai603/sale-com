using Microsoft.AspNetCore.Identity;
using Nvk.Ddd.Domain;
using Nvk.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaleCom.Domain.Identity
{
    public class AppUserClaim : IdentityUserClaim<Guid>, IMultiTenant, IAggregateRoot, ISoftDelete
    {
        public DateTime? CreationTime { get ; set ; }
        public Guid? CreatorId { get ; set ; }
        public DateTime? LastModificationTime { get ; set ; }
        public Guid? LastModifierId { get ; set ; }

        public Guid? TenantId { get; set; }
        public bool IsDeleted { get ; set ; }
        public Guid? DeletedId { get ; set ; }
        public DateTime? DeletionTime { get ; set ; }

        public object[] GetKeys()
        {
            throw new NotImplementedException();
        }
    }
}
