using Microsoft.AspNetCore.Identity;
using Nvk.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaleCom.Domain.Shared.Identity
{
    public class AppRole : IdentityRole<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; set; }
    }
}
