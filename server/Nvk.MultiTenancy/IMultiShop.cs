using System;
using System.Collections.Generic;
using System.Text;

namespace Nvk.MultiTenancy
{
    public interface IMultiShop
    {
        /// <summary>
        /// Id of the related shop.
        /// </summary>
        int? ShopId { get; set; }
    }
}
