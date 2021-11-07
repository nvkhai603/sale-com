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
    /// Lớp base liên quan đến nhập/xuất hàng.
    /// </summary>
    public class Good : AggregateRoot<Guid>, ISoftDelete, IMultiTenant
    {
        /// <summary>
        /// Trạng thái.
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// Ngày tạo phiếu.
        /// </summary>
        public DateTime? TimeWrite { get; set; }
        /// <summary>
        /// Kho hàng.
        /// </summary>
        public WareHouse WareHouse { get; set; }
        /// <summary>
        /// Hàng hóa.
        /// </summary>
        public virtual ICollection<Varation> Varations { get; set; }
        /// <summary>
        /// Tổng số lương.
        /// </summary>
        public int TotalVaration { get; set; }
        /// <summary>
        /// Tổng tiền.
        /// </summary>
        public double TotalCost { get; set; }
        /// <summary>
        /// Ghi chú.
        /// </summary>
        public string Note { get; set; }
        /// <summary>
        /// Hình ảnh.
        /// </summary>
        public string Images { get; set; }
        /// <summary>
        /// Hệ thống.
        /// </summary>
        public Guid? TenantId { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? DeletedId { get; set; }
        public DateTime? DeletionTime { get; set; }
    }
}
