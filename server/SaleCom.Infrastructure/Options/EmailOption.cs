using System;
using System.Collections.Generic;
using System.Text;

namespace SaleCom.Infrastructure.Options
{
    public class EmailOption
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
