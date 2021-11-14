using Nvk.Ddd.Domain;
using Nvk.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaleCom.Domain.WareHouses
{
    /// <summary>
    /// Kho hàng.
    /// </summary>
    public class WareHouse : AggreateRootMulitiTenantSoftDelete<Guid>
    {
        public WareHouse(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Tên kho hàng.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Mã tùy chỉnh.
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Địa chỉ.
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Id tỉnh thành.
        /// </summary>
        public int ProvinceId { get; set; }
        /// <summary>
        /// Id quận huyện.
        /// </summary>
        public int DistrictId { get; set; }
        /// <summary>
        /// Id xã phường.
        /// </summary>
        public int WardId { get; set; }
        /// <summary>
        /// Nhân viên thuộc kho.
        /// </summary>
        public Guid Staff { get; set; }
    }
}
