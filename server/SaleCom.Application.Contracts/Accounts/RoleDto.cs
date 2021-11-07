using System;
using System.Collections.Generic;
using System.Text;

namespace SaleCom.Application.Contracts.Accounts
{
    public class RoleDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ConcurrencyStamp { get; set; }
    }
}
