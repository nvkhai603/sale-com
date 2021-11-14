using Microsoft.AspNetCore.Http;
using Nvk.Utilities;
using System;
using System.Security.Claims;

namespace Nvk.Data
{
    public class CurrentUser : ICurrentUser
    {
        private IHttpContextAccessor _httpContextAccessor;
        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public bool IsAuthenticated => _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
        public Guid? Id => _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value.GetGuid();
        public string IdString => _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        public Guid? TenantId => _httpContextAccessor.HttpContext.User?.FindFirst(AppClaimTypes.TenantId)?.Value.GetGuid();
        public string TenantIdString => _httpContextAccessor.HttpContext.User?.FindFirst(AppClaimTypes.TenantId)?.Value;
        public Guid? WareHouseId => _httpContextAccessor.HttpContext.User?.FindFirst(AppClaimTypes.WareHouseId)?.Value.GetGuid();
        public string WareHouseIdString => _httpContextAccessor.HttpContext.User?.FindFirst(AppClaimTypes.WareHouseId)?.Value;
    }
}