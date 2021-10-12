using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SaleCom.Application.Contracts.Accounts
{
    /// <summary>
    /// Thông tin đăng nhập người dùng.
    /// </summary>
    public class Login
    {
        /// <summary>
        /// Email người dùng.
        /// </summary>
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// Mật khẩu người dùng.
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
