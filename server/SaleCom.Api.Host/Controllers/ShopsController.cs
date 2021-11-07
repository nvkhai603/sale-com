using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SaleCom.Api.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ShopsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllShopsByTenant()
        {
            return Ok();
        }

        [HttpPut("{shopId}")]
        public IActionResult UpdateShop(int shopId)   
        {
            return Ok();
        }
    }
}
