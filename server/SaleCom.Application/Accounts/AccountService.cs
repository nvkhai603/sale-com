using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SaleCom.Application.Contracts.Accounts;
using System;
using System.Threading.Tasks;
using Nvk.MailKit;
using Nvk.Utilities;
using Nvk.EntityFrameworkCore.UnitOfWork;
using SaleCom.EntityFramework;
using SaleCom.Domain.Identity;
using SaleCom.Domain.Tenants;
using System.Collections.Generic;
using SaleCom.Application.Contracts.Tenants;
using System.Linq;
using AutoMapper;
using Nvk.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using SaleCom.Domain.Shared.Tenants;
using SaleCom.Domain.Shared.Identity;
using SaleCom.EntityFramework.Dapper;
using Dapper;
using System.Security.Principal;

namespace SaleCom.Application.Accounts
{
    /// <summary>
    /// Class implementation mặc định của <see cref="IAccountService"/> interface.<br/>
    /// Dịch vụ về tài khoản, người dùng, đăng nhập, etc.
    /// </summary>
    public class AccountService : AppService, IAccountService
    {
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;
        private IEmailService _emailService;
        private IUnitOfWork<IdDbContext> _uow;
        private readonly IIdDbDapper _idDbDapper;
        public AccountService(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IEmailService emailService,
            IUnitOfWork<IdDbContext> uow,
            ILazyServiceProvider lazyServiceProvider,
            IIdDbDapper idDbDapper) : base(lazyServiceProvider)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _uow = uow;
            _idDbDapper = idDbDapper;
        }

        /// <summary>
        /// Thực hiện thêm xác thực truy cập công ty được yêu cầu.
        /// </summary>
        /// <param name="tenantId">Id công ty</param>
        /// <returns>Cookie sẽ có thêm tenantId trong claim</returns>
        public async Task<bool> AccessTenantAsync(string tenantId)
        {
            var tenantUserRepo = _uow.GetRepository<TenantUser>();
            var hasPermission = await tenantUserRepo.ExistsAsync(x => x.TenantId.Equals(Guid.Parse(tenantId)) && x.UserId.Equals(_currentUser.Id));
            if (!hasPermission)
            {
                return false;
            }
            var roleClaims = await _idDbDapper.Connection.QueryAsync<IdentityRoleClaim<Guid>>(@"
SELECT rc.* FROM user_roles ur 
JOIN role_claims rc ON ur.RoleId = rc.RoleId 
WHERE ur.UserId =@v_userId AND ur.TenantId=@v_tenantId;
", new { v_userId = _currentUser.Id, v_tenantId = tenantId });

            var userClaims = await _idDbDapper.Connection.QueryAsync<AppUserClaim>(@"
SELECT * FROM user_claims uc WHERE uc.UserId =@v_userId AND uc.TenantId =@v_tenantId;
", new { v_userId = _currentUser.Id, v_tenantId = tenantId });

            var claims = new List<Claim>();
            claims.AddRange(roleClaims.Select(x => x.ToClaim()).ToList());
            claims.AddRange(userClaims.Select(x => x.ToClaim()).ToList());
            claims.Add(new Claim(AppClaimTypes.TenantId, tenantId));

            var currentIdentity = _signInManager.Context.User.Identity;
            var identity = new ClaimsIdentity(currentIdentity);
            identity.AddUpdateClaims(claims);
            await _signInManager.Context.SignInAsync(IdentityConstants.ApplicationScheme, new ClaimsPrincipal(identity));
            return true;
        }

        /// <summary>
        /// Kích hoạt tài khoản qua email.
        /// </summary>
        /// <param name="email">Email</param>
        /// <param name="token">Token kích hoạt được gửi về mail.</param>
        /// <returns>Kết quả kích hoạt tài khoản.</returns>
        public async Task<IdentityResult> ActiveAccountByEmailTokenAsync(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                //TODO:
                throw new ApplicationException("USER NOT FOUND");
            }
            return await _userManager.ConfirmEmailAsync(user, token);
        }
        public async Task<IEnumerable<TenantDto>> GetAllTenantOfUserAsync()
        {
            var tenantUserRepo = _uow.GetRepository<TenantUser>();
            var userTenants = await tenantUserRepo.GetAllAsync(x => x.UserId.Equals(_currentUser.Id), null, x => x.Include(i => i.Tenant), true, true);
            var tenants = userTenants.Select(x => x.Tenant);
            return _mapper.Map<IEnumerable<Tenant>, IEnumerable<TenantDto>>(tenants);
        }

        /// <summary>
        /// Lấy về thông tin cần thiết của phiên hoạt động.
        /// </summary>
        /// <returns>Thông tin người dùng, công ty</returns>
        public async Task<SessionData> GetSessionDataAsync()
        {
            var email = _httpContextAccessor.HttpContext.User?.Identity.Name;
            var tenantId = _httpContextAccessor.HttpContext.User?.Claims.FirstOrDefault(x => x.Type == "tenantid").Value;
            var user = await _userManager.FindByEmailAsync(email);
            return new SessionData { Email = email, Phone = user.PhoneNumber, TenantId = tenantId };
        }

        /// <summary>
        /// Thực hiện đăng xuất tài khoản người dùng khỏi hệ thống.
        /// </summary>
        public async Task LogOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        /// <summary>
        /// Tạo mới tài khoản truy cập.
        /// </summary>
        /// <param name="registerAccount">Thông tin đăng ký tài khoản người dùng.</param>
        /// <returns>Kết quả đăng ký tài khoản.</returns>
        public async Task<IdentityResult> RegisterAccountAsync(RegisterAccount registerAccount)
        {
            if (!RegexHelper.IsValidEmail(registerAccount.Email))
            {
                var error = new IdentityError[] { new IdentityError { Code = "EmailInvalid", Description = "Email is invalid." } };
                return IdentityResult.Failed(error);
            }
            var user = new AppUser
            {
                Id = Guid.NewGuid(),
                UserName = registerAccount.Email,
                Email = registerAccount.Email,
                PhoneNumber = registerAccount.Phone
            };
            var result = await _userManager.CreateAsync(user, registerAccount.Password);
            if (result.Succeeded)
            {
                var emailConfirmToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                _ = _emailService.SendAsync("SALECOM", user.Email, "Kích hoạt tài khoản SALECOM.", $"EMAIL TOKEN: {Uri.EscapeDataString(emailConfirmToken)}");

                // Đăng ký tài khoản mới nên cần tạo tenant do chính user này quản lý
                var tenant = new Tenant(registerAccount.TenantName ?? TenantConsts.DefaultTenantName);
                tenant.Id = Guid.NewGuid();
                var tenantUserRepo = _uow.GetRepository<TenantUser>();
                await tenantUserRepo.InsertAsync(new TenantUser { User = user, Tenant = tenant });

                // Tạo quyền hạn cao nhất cho User trên Tenant
                var roleRepo = _uow.GetRepository<AppRole>();
                var rootRole = new AppRole(RoleConsts.AdminTenantName, tenant.Id);
                rootRole.Id = Guid.NewGuid();
                await roleRepo.InsertAsync(rootRole);
                var roleClaimRepo = _uow.GetRepository<IdentityRoleClaim<Guid>>();
                await roleClaimRepo.InsertAsync(new IdentityRoleClaim<Guid> { RoleId = rootRole.Id, ClaimType = "test", ClaimValue = "TestValue" });
                var userRoleRepo = _uow.GetRepository<AppUserRole>();
                await userRoleRepo.InsertAsync(new AppUserRole(user.Id, rootRole.Id, tenant.Id));
                await _uow.SaveChangesAsync();
            }
            return result;
        }

        /// <summary>
        /// Thực hiện yêu cầu đặt lại mật khẩu của người dùng.
        /// </summary>
        /// <param name="email">Email người dùng</param>
        public async Task ResetPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                //TODO:
                throw new ApplicationException("USER NOT FOUND");
            }
            var resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            _ = _emailService.SendAsync("SALECOM", user.Email, "Đặt lại mật khẩu SALECOM.", $"PASSWORD TOKEN: {Uri.EscapeDataString(resetPasswordToken)}");
        }

        /// <summary>
        /// Đăng nhập bằng tài khoản & mật khẩu.
        /// </summary>
        /// <param name="input">Thông tin tài khoản & mật khẩu.</param>
        /// <returns>Kết quả đăng nhập.</returns>
        public async Task<SignInResult> SignInAsync(Login login)
        {
            var user = await _userManager.FindByEmailAsync(login.Email);
            if (user == null)
            {
                return SignInResult.Failed;
            }
            return await _signInManager.PasswordSignInAsync(user, login.Password, false, true);
        }

        /// <summary>
        /// Thực hiện lấy về tất cả các phiên làm việc của người dùng hiện tại.
        /// </summary>
        /// <returns>Danh sách tất cả các phiên đăng nhập của người dùng hiện tại.</returns>
        public async Task<IEnumerable<LoginSession>> GetAllSessionLoginOfUserAsync()
        {
            var authenticationTicketRepo = _uow.GetRepository<AppAuthenticationTicket>();
            var authenticationTickets = await authenticationTicketRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<AppAuthenticationTicket>, IEnumerable<LoginSession>>(authenticationTickets);
        }

        /// <summary>
        /// Thực hiện lấy về tất cả vai trò của người dùng.
        /// </summary>
        /// <returns>Danh sách tất cả vai trò của người dùng hiện tại.</returns>
        public async Task<IEnumerable<RoleDto>> GetAllRoles()
        {
            var roleRepo = _uow.GetRepository<AppRole>();
            var roles = await roleRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<AppRole>, IEnumerable<RoleDto>>(roles);
        }
    }

    public static class IdentityHelper
    {
        /// <summary>
        /// Thêm hoặc cập nhật claim vào identity.
        /// </summary>
        /// <param name="claimsIdentity">Identity gần nhất.</param>
        /// <param name="claims">Các claim được thêm.</param>
        public static void AddUpdateClaims(this ClaimsIdentity claimsIdentity, List<Claim> claims)
        {
            if (claimsIdentity == null)
                return;
            foreach (var claim in claims)
            {
                var existingClaim = claimsIdentity.FindFirst(claim.Type);
                if (existingClaim != null)
                    claimsIdentity.RemoveClaim(existingClaim);
                claimsIdentity.AddClaim(claim);
            }
        }
    }
}
