using System;
using System.Collections.Generic;
using System.Text;

namespace SaleCom.Domain.Shared.Orders
{
    public class Order
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
    }
}
