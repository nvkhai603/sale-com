using Microsoft.AspNetCore.Identity;
using SaleCom.Application.Contracts.Tenants;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SaleCom.Application.Contracts.Accounts
{
    /// <summary>
    /// Định nghĩa interface(s) cho dịch vụ tài khoản, người dùng, đăng nhập & đăng xuất, etc.
    /// </summary>
    public interface IAccountService: IAppService
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
        /// Lấy về tất cả công ty mà người dùng có thể truy cập.
        /// </summary>
        /// <returns>Danh sách thông tin công ty</returns>
        Task<IEnumerable<TenantDto>> GetAllTenantOfUserAsync();

        /// <summary>
        /// Thực hiện đăng xuất tài khoản người dùng khỏi hệ thống.
        /// </summary>
        Task LogOutAsync();

        /// <summary>
        /// Thực hiện thêm xác thực truy cập công ty được yêu cầu.
        /// </summary>
        /// <param name="tenantId">Id công ty</param>
        /// <returns>Cookie sẽ có thêm tenantId trong claim</returns>
        Task<bool> AccessTenantAsync(string tenantId);

        /// <summary>
        /// Thực hiện lấy về tất cả các phiên làm việc của người dùng hiện tại.
        /// </summary>
        /// <returns>Danh sách tất cả các phiên đăng nhập của người dùng hiện tại.</returns>
        Task<IEnumerable<LoginSession>> GetAllSessionLoginOfUserAsync();

        /// <summary>
        /// Thực hiện lấy về tất cả vai trò của người dùng.
        /// </summary>
        /// <returns>Danh sách tất cả vai trò của người dùng hiện tại.</returns>
        Task<IEnumerable<RoleDto>> GetAllRoles();
    }
}
