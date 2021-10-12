using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SaleCom.Application.Contracts.Accounts
{
    /// <summary>
    /// Thông tin đăng ký người dùng.
    /// </summary>
    public class RegisterAccount
    {
        /// <summary>
        /// Email người dùng.
        /// </summary>
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        /// <summary>
        /// Mật khẩu người dùng.
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// Tên công ty khời tạo.
        /// </summary>
        [Required]
        public string TenantName { get; set; }

        /// <summary>
        /// Số điện thoại liên hệ.
        /// </summary>
        [Required]
        public string Phone { get; set; }
    }
}
