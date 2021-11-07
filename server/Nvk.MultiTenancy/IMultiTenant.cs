using System;
using System.Collections.Generic;
using System.Text;

namespace Nvk.MultiTenancy
{
    public interface IMultiTenant
    {
        /// <summary>
        /// Id of the related tenant.
        /// </summary>
        Guid? TenantId { get; set; }
    }
}
