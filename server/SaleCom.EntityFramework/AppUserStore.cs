using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SaleCom.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaleCom.EntityFramework
{
    public class AppUserStore<TUser> : UserStore<TUser, AppRole, IdDbContext, Guid, AppUserClaim, AppUserRole, IdentityUserLogin<Guid>, IdentityUserToken<Guid>, IdentityRoleClaim<Guid>>, IUserStore<TUser> where TUser : AppUser
    {
        public AppUserStore(IdDbContext context, IdentityErrorDescriber describer = null) : base(context, describer)
        {
        }
    }
}
