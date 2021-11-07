using Nvk.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaleCom.Application.Orders
{
    public class OrderService : AppService
    {
        public OrderService(ILazyServiceProvider lazyServiceProvider) : base(lazyServiceProvider)
        {
        }
    }
}
