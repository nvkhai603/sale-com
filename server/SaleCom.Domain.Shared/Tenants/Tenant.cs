using Nvk.Ddd.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaleCom.Domain.Shared.Tenants
{
    /// <summary>
    /// Định nghĩa lớp công ty.
    /// </summary>
    public class Tenant: Entity<Guid>
    {
        /// <summary>
        /// Tên công ty
        /// </summary>
        public string Name { get; set; }
    }
}
