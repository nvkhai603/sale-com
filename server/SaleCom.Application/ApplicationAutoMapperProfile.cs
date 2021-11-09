using AutoMapper;
using SaleCom.Application.Contracts.Accounts;
using SaleCom.Application.Contracts.Products;
using SaleCom.Application.Contracts.Tenants;
using SaleCom.Application.Contracts.Varations;
using SaleCom.Domain.Identity;
using SaleCom.Domain.Products;
using SaleCom.Domain.Tenants;
using SaleCom.Domain.Varations;
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

            // Sản phẩm.
            CreateMap<CreateProductReq, Product>();
            CreateMap<VarationReq, Varation>();
        }
    }
}
