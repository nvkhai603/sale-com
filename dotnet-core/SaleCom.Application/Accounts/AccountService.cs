using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SaleCom.Application.Contracts.Accounts;
using SaleCom.Application.Utilities;
using SaleCom.Domain.Shared.Identity;
using System;
using System.Threading.Tasks;
using Nvk.MailKit;

namespace SaleCom.Application.Accounts
{
    public class AccountService : IAccountService
    {
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;
        private IEmailService _emailService;
        private HttpContext _httpContext;
        public AccountService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailService emailService, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _httpContext = httpContextAccessor.HttpContext;
        }

        /// <summary>
        /// Kích hoạt tài khoản qua email
        /// </summary>
        /// <param name="email">Email</param>
        /// <param name="token">Token</param>
        /// <returns>IdentityResult</returns>
        public async Task<IdentityResult> ActiveAccountByEmailTokenAsync(string email,string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                //TODO:
                throw new ApplicationException("USER NOT FOUND");
            }
            return await _userManager.ConfirmEmailAsync(user, token);
        }

        public async Task<SessionData> GetSessionDataAsync()
        {
            var email = _httpContext.User?.Identity.Name;
            var user = await _userManager.FindByEmailAsync(email);
            return new SessionData { Email = email, Phone = user.PhoneNumber };
        }

        public async Task LogOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        /// <summary>
        /// Tạo mới tài khoản truy cập
        /// </summary>
        /// <param name="registerAccount">RegisterAccount</param>
        /// <returns>IdentityResult</returns>
        public async Task<IdentityResult> RegisterAccountAsync(RegisterAccount registerAccount)
        {
            if (!RegexHelper.IsValidEmail(registerAccount.Email))
            {
                var error = new IdentityError[] { new IdentityError { Code = "EmailInvalid", Description = "Email is invalid." } };
                return IdentityResult.Failed(error);
            }
            var user = new AppUser
            {
                UserName = registerAccount.Email,
                Email = registerAccount.Email,
                PhoneNumber = registerAccount.Phone
            };
            var result = await _userManager.CreateAsync(user, registerAccount.Password);
            if (result.Succeeded)
            {
                var emailConfirmToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                _ = _emailService.SendAsync("SALECOM", user.Email, "Kích hoạt tài khoản SALECOM.", $"EMAIL TOKEN: {Uri.EscapeDataString(emailConfirmToken)}");
            }
            return result;
        }

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

        public async Task<SignInResult> SignInAsync(Login login)
        {
            var user = await _userManager.FindByEmailAsync(login.Email);
            if (user == null)
            {
                return SignInResult.Failed;
            }
            return await _signInManager.PasswordSignInAsync(user, login.Password, false, true);
        }
    }
}
