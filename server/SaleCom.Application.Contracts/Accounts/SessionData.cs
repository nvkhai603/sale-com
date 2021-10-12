using System;
using System.Collections.Generic;
using System.Text;

namespace SaleCom.Application.Contracts.Accounts
{
    /// <summary>
    /// Thông tin phiên làm việc của người dùng.
    /// </summary>
    public class SessionData
    {
        /// <summary>
        /// Email người dùng.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Số điện thoại người dùng.
        /// </summary>
        public string Phone { get; set; }
    }
}
