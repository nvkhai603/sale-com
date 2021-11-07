using Microsoft.AspNetCore.Identity;
using Nvk.Ddd.Domain;
using Nvk.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaleCom.Domain.Identity
{
    public class AppUserRole : IdentityUserRole<Guid>, IMultiTenant, IAggregateRoot, IHasConcurrencyStamp
    {
        public AppUserRole()
        {

        }
        public AppUserRole(Guid userId, Guid roleId, Guid tenantId)
        {
            UserId = userId;
            RoleId = roleId;
            TenantId = tenantId;
        }
        public DateTime? CreationTime { get; set; }
        public Guid? CreatorId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public Guid? LastModifierId { get; set; }
        public Guid? TenantId { get; set; }
        public string ConcurrencyStamp { get; set; }

        public object[] GetKeys()
        {
            throw new NotImplementedException();
        }
    }
}
