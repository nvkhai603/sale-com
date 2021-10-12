using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaleCom.Domain.Shared.Identity
{
    public class AppUser: IdentityUser<Guid>
    {
        public AppUser(): base()
        {

        }

        public AppUser(string userName): base(userName)
        {

        }

        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public int Status { get; set; }
    }
}
