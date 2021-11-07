using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Nvk.Utilities;
using SaleCom.Domain.Identity;
using SaleCom.EntityFramework;
using System;
using System.Threading.Tasks;

namespace SaleCom.Application
{
    public class AuthenticationTicketStore : ITicketStore
    {
        private readonly IdDbContext _context;
        public AuthenticationTicketStore(IdDbContext context)
        {
            _context = context;
        }

        public async Task RemoveAsync(string key)
        {
            if (Guid.TryParse(key, out var id))
            {
                var ticket = await _context.AuthenticationTickets.SingleOrDefaultAsync(x => x.Id == id);
                if (ticket != null)
                {
                    _context.AuthenticationTickets.Remove(ticket);
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task RenewAsync(string key, AuthenticationTicket ticket)
        {
            if (Guid.TryParse(key, out var id))
            {
                var authenticationTicket = await _context.AuthenticationTickets.FindAsync(id);
                if (authenticationTicket != null)
                {
                    authenticationTicket.Value = SerializeToBytes(ticket);
                    authenticationTicket.LastActivity = DateTime.Now;
                    authenticationTicket.Expires = ticket.Properties.ExpiresUtc.Value.UtcDateTime.ToLocalTime();
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task<AuthenticationTicket> RetrieveAsync(string key)
        {
            if (Guid.TryParse(key, out var id))
            {
                var authenticationTicket = await _context.AuthenticationTickets.FindAsync(id);
                if (authenticationTicket != null)
                {
                    authenticationTicket.LastActivity = DateTime.Now;
                    await _context.SaveChangesAsync();
                    return DeserializeFromBytes(authenticationTicket.Value);
                }
            }
            return null;
        }

        public async Task<string> StoreAsync(AuthenticationTicket ticket)
        {

            //if (ticket.AuthenticationScheme == "Identity.Application")
            //{
            //    userId = nameIdentifier;
            //}
            //// If using a external login provider like google we need to resolve the userid through the Userlogins
            //else if (ticket.AuthenticationScheme == "Identity.External")
            //{
            //    userId = (await _context.UserLogins.SingleAsync(x => x.ProviderKey == nameIdentifier)).UserId;
            //}

            var userId = ticket.Principal.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value.GetGuid();
            if (userId == null)
            {
                throw new ArgumentException("UserId in ticket.Principal is null.");
            }

            var authenticationTicket = new AppAuthenticationTicket()
            {
                UserId = (Guid)userId,
                LastActivity = DateTime.Now,
                Value = SerializeToBytes(ticket),
            };

            var expiresUtc = ticket.Properties.ExpiresUtc;
            if (expiresUtc.HasValue)
            {
                authenticationTicket.Expires = expiresUtc.Value.UtcDateTime.ToLocalTime();
            }

            await _context.AuthenticationTickets.AddAsync(authenticationTicket);
            await _context.SaveChangesAsync();
            return authenticationTicket.Id.ToString();
        }

        private byte[] SerializeToBytes(AuthenticationTicket source)
            => TicketSerializer.Default.Serialize(source);

        private AuthenticationTicket DeserializeFromBytes(byte[] source)
            => source == null ? null : TicketSerializer.Default.Deserialize(source);
    }
}
