using AutoMapper;
using SaleCom.Application.Contracts.Accounts;
using SaleCom.Application.Contracts.Tenants;
using SaleCom.Domain.Identity;
using SaleCom.Domain.Tenants;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaleCom.Application
{
    /// <summary>
    /// Lớp định nghĩa Profile cho AutoMapper.
    /// </summary>
    public class ApplicationAutoMapperProfile : Profile
    {
        public ApplicationAutoMapperProfile()
        {
            CreateMap<Tenant, TenantDto>();
            CreateMap<AppAuthenticationTicket, LoginSession>();
            CreateMap<AppRole, RoleDto>();
        }
    }
}
