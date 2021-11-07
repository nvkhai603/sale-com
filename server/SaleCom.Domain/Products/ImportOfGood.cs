using Nvk.Ddd.Domain;
using Nvk.MultiTenancy;
using SaleCom.Domain.Varations;
using SaleCom.Domain.WareHouses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaleCom.Domain.Products
{
    /// <summary>
    /// Phiếu nhập hàng
    /// </summary>
    public class ImportOfGood : Good
    {
        /// <summary>
        /// Phí vận chuyển.
        /// </summary>
        public double ShippingFee { get; set; }
        /// <summary>
        /// Ngày về dự kiến.
        /// </summary>
        public DateTime? TimeExpectedReturn { get; set; }
    }
}
