using Nvk.Ddd.Domain;
using Nvk.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaleCom.Domain.Transactions
{
    /// <summary>
    /// Giao dịch.
    /// </summary>
    public class MoneyTransaction : AggreateRootMulitiTenantSoftDelete<Guid>
    {
        /// <summary>
        /// Tên giao dịch.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Loại giao dịch.
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// Số dư thay đổi.
        /// </summary>
        public decimal BalanceChange { get; set; }
        /// <summary>
        /// Hình thức thanh toán.
        /// </summary>
        public int PaymentType { get; set; }
        /// <summary>
        /// Nhà cung cấp.
        /// </summary>
        public int SupplyId { get; set; }
        /// <summary>
        /// Thời điểm phát sinh.
        /// </summary>
        public DateTime? TimeChange { get; set; }
        /// <summary>
        /// Khóa.
        /// </summary>
        public bool IsLock { get; set; }
        /// <summary>
        /// Hạch toán tài chính.
        /// </summary>
        public bool FinancialAccounting { get; set; }
        /// <summary>
        /// Ghi chú.
        /// </summary>
        public string Note { get; set; }
    }
}
