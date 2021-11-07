using Nvk.Ddd.Domain;
using Nvk.MultiTenancy;
using SaleCom.Domain.Customers;
using SaleCom.Domain.Varations;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaleCom.Domain.Orders
{
    /// <summary>
    /// Đơn hàng.
    /// </summary>
    public class Order: AggreateRootMulitiTenantSoftDelete<Guid>
    {
        /// <summary>
        /// Mã đơn hàng.
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Trạng thái đơn hàng. // TODO
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// Gắn thẻ.
        /// </summary>
        public string Tags { get; set; }
        /// <summary>
        /// Miễn phí giao hàng.
        /// </summary>
        public bool IsFreeShip { get; set; }
        /// <summary>
        /// Thu phí do hoàn đơn
        /// </summary>
        public bool IsFeeForRefund { get; set; }
        /// <summary>
        /// Phí vận chuyển.
        /// </summary>
        public decimal ShippingFee { get; set; }
        /// <summary>
        /// Giảm giá cho đơn hàng.
        /// </summary>
        public int DisCount { get; set; }
        /// <summary>
        /// Tiền chuyển khoản.
        /// </summary>
        public decimal TranferMoney { get; set; }
        /// <summary>
        /// Tiền quẹt thẻ.
        /// </summary>
        public decimal CashMoney { get; set; }
        /// <summary>
        /// Tiền chuyển qua Momo.
        /// </summary>
        public decimal MomoMoney { get; set; }
        /// <summary>
        /// Tiền chuyển qua QrPay.
        /// </summary>
        public decimal QrPayMoney { get; set; }
        /// <summary>
        /// Tiền khách đưa trực tiếp.
        /// </summary>
        public decimal NomalMoney { get; set; }
        /// <summary>
        /// Tiền phụ thu.
        /// </summary>
        public decimal SurchargeMoney { get; set; }
        /// <summary>
        /// Ghi chú nội bộ.
        /// </summary>
        public string LocalNote { get; set; }
        /// <summary>
        /// Ghi chú để in.
        /// </summary>
        public string PrintNote { get; set; }
        /// <summary>
        /// Nhân viên xử lý.
        /// </summary>
        public Guid? HandlingStaff { get; set; }
        /// <summary>
        /// Nhân viên chăm sóc.
        /// </summary>
        public Guid? CaringStaff { get; set; }
        /// <summary>
        /// Loại đơn hàng: Tại quầy/Online
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// Thông tin khách hàng.
        /// </summary>
        public virtual Customer Customer {  get; set; }
        
        /// <summary>
        /// Thông tin nhận hàng, cho loại đơn hàng Online.
        /// </summary>
        /// ==============================================
        /// <summary>
        /// Tên người nhận hàng.
        /// </summary>
        public string DeliveryCustomerName { get; set; }
        /// <summary>
        /// Dự kiến ngày nhận hàng.
        /// </summary>
        public DateTime? DeliveryExpectedDate { get; set; }
        /// <summary>
        /// Số điện thoại nhận hàng.
        /// </summary>
        public string DeliveryPhoneNumber { get; set; }
        /// <summary>
        /// Địa chỉ nhận hàng.
        /// </summary>
        public string DeliveryAddress { get; set; }
        /// <summary>
        /// Id tỉnh thành nhận hàng.
        /// </summary>
        public int DeliveryProvinceId { get; set; }
        /// <summary>
        /// Id quận huyện nhận hàng.
        /// </summary>
        public int DeliveryDistrictId { get; set; }
        /// <summary>
        /// Id phường xã nhận hàng.
        /// </summary>
        public int DeliveryWardId { get; set; }

        /// <summary>
        /// Thông tin vận chuyển, cho loại đơn hàng Online.
        /// </summary>
        /// ==============================================
        /// <summary>
        /// Mã vận đơn.
        /// </summary>
        public string ShippingCode { get; set; }
        /// <summary>
        /// Phí vận chuyển.
        /// </summary>
        public double ShippingCost { get; set; }
        /// <summary>
        /// Các sản phẩm trong giỏ hàng. (Biên thể của sản phẩm)
        /// </summary>
        public virtual ICollection<Varation> Varations { get; set; }
    }
}
