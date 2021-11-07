using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using Nvk.MailKit;
using Nvk.Data;
using SaleCom.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using SaleCom.EntityFramework;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Nvk.Dapper;
using SaleCom.EntityFramework.Dapper;

namespace SaleCom.Application
{
    public class AutofacModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var dataAccess = System.Reflection.Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(dataAccess)
                   .Where(t => t.Name.EndsWith("Service"))
                   .AsImplementedInterfaces();
            builder.RegisterType<EmailService>().As<IEmailService>();
            builder.RegisterType<CurrentUser>().As<ICurrentUser>();
            builder.RegisterType<AppUserStore<AppUser>>().As<IUserStore<AppUser>>(); 
            builder.RegisterType<IdDbDapper>().As<IIdDbDapper>().InstancePerLifetimeScope(); 
            builder.RegisterType<SaleComDbDapper>().As<ISaleComDbDapper>().InstancePerLifetimeScope(); 
        }
    }
}
