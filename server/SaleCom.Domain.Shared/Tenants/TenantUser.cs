using Nvk.Ddd.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaleCom.Domain.Shared.Tenants
{
    /// <summary>
    /// Định nghĩa lớp người dùng trong công ty.
    /// </summary>
    public class TenantUser: Entity
    {
        /// <summary>
        /// Id người dùng.
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// Id công ty.
        /// </summary>
        public Guid TenantId { get; set; }
    }
}
