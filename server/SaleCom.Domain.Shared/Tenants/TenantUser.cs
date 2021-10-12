using System;
using System.Collections.Generic;
using System.Text;

namespace SaleCom.Domain.Shared.Tenants
{
    public class TenantUser
    {
        public Guid UserId { get; set; }
        public Guid TenantId { get; set; }
    }
}
