using Nvk.Ddd.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaleCom.Domain.Shared.Orders
{
    public class Order: AggregateRoot<Guid>
    {
        public string Code { get; set; }
    }
}
