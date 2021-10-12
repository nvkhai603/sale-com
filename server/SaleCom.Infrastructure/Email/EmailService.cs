using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using SaleCom.Infrastructure.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SaleCom.Infrastructure.Email
{
    public class EmailService: IEmailService
    {
        private readonly EmailOption _emailOption;

        public EmailService()
        {

        }

        public EmailService(IOptions<EmailOption> options)
        {
            _emailOption = options.Value;
        }

        /// <summary>
        /// Thực hiện gửi mail
        /// </summary>
        /// <param name="from">Từ</param>
        /// <param name="to">Đến</param>
        /// <param name="subject">Tiêu đề</param>
        /// <param name="html">Nội dung</param>
        public async Task SendAsync(string from, string to, string subject, string html)
        {
            // create message
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(from));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = html };

            // send email
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_emailOption.Server, _emailOption.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_emailOption.UserName, _emailOption.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
