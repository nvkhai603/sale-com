using Nvk.Ddd.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaleCom.Domain.Licenses
{
    /// <summary>
    /// Gói cước công ty.
    /// </summary>
    public class DomainLicense : AggreateRootMulitiTenantSoftDelete<Guid>
    {
        /// <summary>
        /// Miền, cha của các shop (Tenant).
        /// </summary>
        public virtual DomainTenant DomainTenant { get; set; }
        /// <summary>
        /// Thời gian hiệu lực.
        /// </summary>
        public DateTime? EffectiveTime { get; set; }
        /// <summary>
        /// Số ngày được cấp phép.
        /// </summary>
        public int TotalDay { get; set; }
    }
}
