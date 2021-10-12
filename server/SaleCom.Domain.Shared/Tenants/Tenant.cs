using System;
using System.Collections.Generic;
using System.Text;

namespace SaleCom.Domain.Shared.Tenants
{
    public class Tenant
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
