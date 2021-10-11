using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SaleCom.Application.Contracts.Accounts
{
    public class RegisterAccount
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        public string TenantName { get; set; }
        [Required]
        public string Phone { get; set; }
    }
}
