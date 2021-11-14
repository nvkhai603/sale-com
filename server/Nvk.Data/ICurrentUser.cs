using System;
using System.Collections.Generic;
using System.Text;

namespace Nvk.Data
{
    public interface ICurrentUser
    {
        bool IsAuthenticated { get; }
        Guid? Id { get; }
        string IdString { get; }
        Guid? TenantId { get; }
        string TenantIdString { get; }
        Guid? WareHouseId {  get; }
        string WareHouseIdString {  get; }
    }
}
