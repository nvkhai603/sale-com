using Nvk.Ddd.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaleCom.Application.Contracts.Tenants
{
    public class TenantDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
