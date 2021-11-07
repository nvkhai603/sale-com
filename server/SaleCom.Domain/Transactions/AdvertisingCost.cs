using Nvk.Ddd.Domain;
using Nvk.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaleCom.Domain.Transactions
{
    /// <summary>
    /// Chi phí quảng cáo
    /// </summary>
    public class AdvertisingCost: AggreateRootMulitiTenantSoftDelete<Guid>
    {
        /// <summary>
        /// Nguồn.
        /// </summary>
        public int SourceId { get; set; }
        /// <summary>
        /// Nhân viên đảm nhận.
        /// </summary>
        public Guid StaffId { get; set; }
        /// <summary>
        /// Chi phí.
        /// </summary>
        public decimal Cost { get; set; }
        /// <summary>
        /// PostId
        /// </summary>
        public string PostId { get; set; }
    }
}
