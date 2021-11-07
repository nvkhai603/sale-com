using Nvk.Ddd.Domain;
using Nvk.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaleCom.Domain.Customers
{
    /// <summary>
    /// Khách hàng
    /// </summary>
    public class Customer : AggregateRoot<Guid>, ISoftDelete, IMultiTenant
    {
        /// <summary>
        /// Tên khách hàng.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Số điện thoại khách hàng.
        /// </summary>
        public string PhoneNumbers { get; set; }
        /// <summary>
        /// Ngày sinh.
        /// </summary>
        public DateTime Dob { get; set; }
        /// <summary>
        /// Giới tính
        /// </summary>
        public int Gender { get; set; }
        /// <summary>
        /// Email.
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Lần mua hàng gần nhất.
        /// </summary>
        public DateTime? LastOrderAt { get; set; }
        /// <summary>
        /// Cấp độ.
        /// </summary>
        public int Level { get; set; }
        /// <summary>
        /// Bị khóa.
        /// </summary>
        public bool IsBlock { get; set; }
        /// <summary>
        /// Số lần mua hàng.
        /// </summary>
        public int OrderCount { get; set; }
        /// <summary>
        /// Số đơn hàng hoàn thành.
        /// </summary>
        public int SuccessfulOrderCount { get; set; }
        /// <summary>
        /// Số đơn hàng hoàn trả.
        /// </summary>
        public int ReturnedOrderCount { get; set; }
        /// <summary>
        /// Tổng thanh toán.
        /// </summary>
        public decimal PurchasedAmount { get; set; }
        /// <summary>
        /// Điểm thưởng.
        /// </summary>
        public int RewardPoint { get; set; }
        /// <summary>
        /// Gắn thẻ.
        /// </summary>
        public string Tags { get; set; }
        /// <summary>
        /// TODO: Địa chỉ.
        /// </summary>
        public bool IsDeleted { get; set; }
        public Guid? DeletedId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public Guid? TenantId { get; set; }
    }
}
