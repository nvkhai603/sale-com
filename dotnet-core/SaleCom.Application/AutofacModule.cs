using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using Nvk.MailKit;

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
        }
    }
}
