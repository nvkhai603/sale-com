﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SaleCom.Domain.Shared.Products
{
    public interface IProductCore
    {
        /// <summary>
        /// Tên sản phẩm.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Mã sản phẩm.
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Mô tả sản phẩm.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Ghi chú sản phẩm
        /// </summary>
        public string Note { get; set; }
        /// <summary>
        /// Link nhập sản phẩm.
        /// </summary>
        public string Links { get; set; }
        /// <summary>
        /// Giới hạn về số lượng để cảnh báo sắp hết hàng.
        /// </summary>
        public int LimitQuantityToWarn { get; set; }
        /// <summary>
        /// Cho phép bán tồn kho âm.
        /// </summary>
        public bool IsSellNegative { get; set; }
        /// <summary>
        /// Thẻ.
        /// </summary>
        public string Tags { get; set; }
        /// <summary>
        /// Cảnh báo hết hàng theo từng mẫu mã.
        /// </summary>
        public bool IsWarningByVariation { get; set; }
    }
}
