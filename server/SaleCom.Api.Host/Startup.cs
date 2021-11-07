using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SaleCom.Application;
using SaleCom.EntityFramework;
using System.Threading.Tasks;
using Nvk.EntityFrameworkCore.UnitOfWork;
using Nvk.MailKit;
using SaleCom.Domain.Identity;
using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Nvk.Data;
using Nvk.Dapper;
using SaleCom.EntityFramework.Dapper;

namespace SaleCom.Api.Host
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime.
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new AutofacModule());
        }
        private void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var idDbContext = serviceScope.ServiceProvider.GetRequiredService<IdDbContext>();
                idDbContext.Database.Migrate();
                serviceScope.ServiceProvider.GetRequiredService<SaleComDbContext>().Database.Migrate();
            }
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
            // Register config
            services.Configure<EmailOption>(Configuration.GetSection("Email"));
            services.Configure<ConnectionStringOption>(Configuration.GetSection("ConnectionStrings"));
            // End register config
            services.AddAutoMapper(typeof(ApplicationAutoMapperProfile));
            const string DB_VERSION = "5.5.5-10.2.38-MariaDB";
            services.AddDbContext<IdDbContext>(options =>
                options.UseMySql(Configuration.GetConnectionString("IdDb"), ServerVersion.Parse(DB_VERSION)));
            services.AddDbContext<SaleComDbContext>(options =>
                options.UseMySql(Configuration.GetConnectionString("Db"), ServerVersion.Parse(DB_VERSION)));
            services.AddUnitOfWork<SaleComDbContext, IdDbContext>();
            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = true;
            })
                .AddEntityFrameworkStores<IdDbContext>()
                .AddDefaultTokenProviders();
            //Use DI services to configure options
            services.AddScoped<ITicketStore, AuthenticationTicketStore>();
            services.AddOptions<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme)
                .Configure<ITicketStore>((options, store) => {
                    options.SessionStore = store;
                });
            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.Name = "_sid";
                options.Cookie.HttpOnly = true;
                options.Events.OnRedirectToAccessDenied = context =>
                {
                    context.Response.StatusCode = 403;
                    return Task.CompletedTask;
                };
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };
                options.ExpireTimeSpan = TimeSpan.FromDays(14);
            });

            services.AddHttpContextAccessor();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SaleCom", Version = "v1" });
            });
            services.AddTransient<ILazyServiceProvider, LazyServiceProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            InitializeDatabase(app);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SaleCom v1"));
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
