﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SaleCom.Domain.Shared.Varations
{
    public interface IVarationCore
    {
        /// <summary>
        /// Hình ảnh.
        /// </summary>
        public string Images { get; set; }
        /// <summary>
        /// Khóa.
        /// </summary>
        public bool IsLock { get; set; }
        /// <summary>
        /// BarCode.
        /// </summary>
        public string BarCode { get; set; }
        /// <summary>
        /// Giá nhập trung bình.
        /// </summary>
        public decimal AverageImportPrice { get; set; }
        /// <summary>
        /// Giá nhập cuối.
        /// </summary>
        public decimal LastImportPrice { get; set; }
        /// <summary>
        /// Giá bán.
        /// </summary>
        public decimal RetailPrice { get; set; }
        /// <summary>
        /// Trọng lượng.
        /// </summary>
        public decimal Weight { get; set; }
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
        public decimal TotalPurchasePrice { get; set; }
    }
}
