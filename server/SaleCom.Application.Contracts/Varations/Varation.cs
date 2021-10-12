using System;
using System.Collections.Generic;
using System.Text;

namespace SaleCom.Application.Contracts.Varations
{
    /// <summary>
    /// Mẫu mã.
    /// </summary>
    public class Varation
    {
        /// <summary>
        /// Id.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Hình ảnh.
        /// </summary>
        public string[] Images { get; set; }
        /// <summary>
        /// Khóa.
        /// </summary>
        public bool IsLock { get; set; } = false;
        /// <summary>
        /// BarCode.
        /// </summary>
        public string BarCode { get; set; }
        /// <summary>
        /// Giá nhập trung bình.
        /// </summary>
        public int AverageImportPrice { get; set; }
        /// <summary>
        /// Giá nhập cuối.
        /// </summary>
        public int LastImportPrice { get; set; }
        /// <summary>
        /// Giá bán.
        /// </summary>
        public int RetailPrice { get; set; }
        /// <summary>
        /// Trọng lượng.
        /// </summary>
        public int Weight { get; set; }
        /// <summary>
        /// Tổng nhập.
        /// </summary>
        public int TotalQuantity { get; set; }
        /// <summary>
        /// Tồn kho.
        /// </summary>
        public int RemainQuantity { get; set; }
        /// <summary>
        /// Tổng tiền chi nhập hàng.
        /// </summary>
        public int TotalPurchasePrice { get; set; }
        /// <summary>
        /// Id sản phẩm gốc.
        /// </summary>
        public Guid ProductId { get; set; }
    }
}
