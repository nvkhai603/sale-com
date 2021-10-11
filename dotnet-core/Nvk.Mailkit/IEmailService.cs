using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nvk.MailKit
{
    public interface IEmailService
    {
        /// <summary>
        /// Thực hiện gửi mail
        /// </summary>
        /// <param name="from">Từ</param>
        /// <param name="to">Đến</param>
        /// <param name="subject">Tiêu đề</param>
        /// <param name="html">Nội dung</param>
        Task SendAsync(string from, string to, string subject, string html);
    }
}
