using Nvk.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaleCom.Domain.Identity
{
    /// <summary>
    /// Ticket cho SessionStore
    /// </summary>
    public class AppAuthenticationTicket
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public byte[] Value { get; set; }

        public DateTime? LastActivity { get; set; }

        public DateTime? Expires { get; set; }
    }
}
