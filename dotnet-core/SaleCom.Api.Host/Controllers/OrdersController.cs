using Microsoft.AspNetCore.Mvc;
using Nvk.EntityFrameworkCore.UnitOfWork;
using SaleCom.Domain.Shared.Orders;
using System.Linq;

namespace SaleCom.Api.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IUnitOfWork _uow;   
        public OrdersController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet]
        public IQueryable<Order> GetAll()
        {
            var orderRepo = _uow.GetRepository<Order>();
            return orderRepo.GetAll();
        }
    }
}
