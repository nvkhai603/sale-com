using Microsoft.AspNetCore.Mvc;
using Nvk.EntityFrameworkCore.UnitOfWork;
using SaleCom.Domain.Orders;
using SaleCom.EntityFramework;
using System.Linq;
using System.Threading.Tasks;

namespace SaleCom.Api.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IUnitOfWork<SaleComDbContext> _uow;   
        public OrdersController(IUnitOfWork<SaleComDbContext> uow)
        {
            _uow = uow;
        }

        [HttpGet]
        public IQueryable<Order> GetAll()
        {
            var orderRepo = _uow.GetRepository<Order>();
            return orderRepo.GetAll();
        }

        [HttpPost]
        public async Task<IActionResult> Insert(Order order) { 
            var orderRepo = _uow.GetRepository<Order>();
            await orderRepo.InsertAsync(order);
            await _uow.SaveChangesAsync();
            return Ok(order);
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update(Order order)
        {
            var orderRepo = _uow.GetRepository<Order>();
            var oldOrder = await orderRepo.FindAsync(order.Id);
            oldOrder.Code = order.Code;
            oldOrder.ConcurrencyStamp = order.ConcurrencyStamp;
            orderRepo.Update(oldOrder);
            await _uow.SaveChangesAsync();
            return Ok(order);
        }
    }
}
