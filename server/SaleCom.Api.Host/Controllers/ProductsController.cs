using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaleCom.Application.Contracts.Products;
using System.Threading.Tasks;

namespace SaleCom.Api.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            return Ok();
        }

        /// <summary>
        /// Tạo mới sản phẩm
        /// </summary>
        /// <param name="input">Đầu vào</param>
        /// <returns status="201">Tạo thành công</returns>
        [HttpPost]
        public async Task<IActionResult> CreateProducts(CreateProductReq input)
        {
            await _productService.CreateProductsAsync(input);
            return Ok();
        }

    }
}
