using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SaleCom.Application.Contracts.Accounts
{
    /// <summary>
    /// Định nghĩa interface(s) cho dịch vụ tài khoản, người dùng, đăng nhập & đăng xuất, etc.
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// Tạo mới tài khoản truy cập.
        /// </summary>
        /// <param name="registerAccount">Thông tin đăng ký tài khoản người dùng.</param>
        /// <returns>Kết quả đăng ký tài khoản.</returns>
        Task<IdentityResult> RegisterAccountAsync(RegisterAccount registerAccount);

        /// <summary>
        /// Đăng nhập bằng tài khoản & mật khẩu.
        /// </summary>
        /// <param name="input">Thông tin tài khoản & mật khẩu.</param>
        /// <returns>Kết quả đăng nhập.</returns>
        Task<SignInResult> SignInAsync(Login input);

        /// <summary>
        /// Kích hoạt tài khoản qua email.
        /// </summary>
        /// <param name="email">Email</param>
        /// <param name="token">Token kích hoạt được gửi về mail.</param>
        /// <returns>Kết quả kích hoạt tài khoản.</returns>
        Task<IdentityResult> ActiveAccountByEmailTokenAsync(string email, string token);

        /// <summary>
        /// Lấy về thông tin cần thiết của phiên hoạt động.
        /// </summary>
        /// <returns>Thông tin người dùng, công ty</returns>
        Task<SessionData> GetSessionDataAsync();

        /// <summary>
        /// Thực hiện yêu cầu đặt lại mật khẩu của người dùng.
        /// </summary>
        /// <param name="email">Email người dùng</param>
        Task ResetPasswordAsync(string email);

        /// <summary>
        /// Thực hiện đăng xuất tài khoản người dùng khỏi hệ thống.
        /// </summary>
        Task LogOutAsync();
    }
}
