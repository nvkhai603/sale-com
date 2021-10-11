using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SaleCom.Application.Contracts.Accounts
{
    public interface IAccountService
    {
        /// <summary>
        /// Tạo mới tài khoản truy cập
        /// </summary>
        /// <param name="registerAccount">RegisterAccount</param>
        /// <returns>IdentityResult</returns>
        Task<IdentityResult> RegisterAccountAsync(RegisterAccount registerAccount);
        Task<SignInResult> SignInAsync(Login input);

        /// <summary>
        /// Kích hoạt tài khoản qua email
        /// </summary>
        /// <param name="email">Email</param>
        /// <param name="token">Token</param>
        /// <returns>IdentityResult</returns>
        Task<IdentityResult> ActiveAccountByEmailTokenAsync(string email, string token);
        Task<SessionData> GetSessionDataAsync();
        Task ResetPasswordAsync(string email);
        Task LogOutAsync();
    }
}
