using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaleCom.Application.Contracts.Products;
using System;
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
        public async Task<IActionResult> GetProducts()
        {
            var body = await _productService.GetProductsAsync();
            return Ok(body);
        }

        [HttpGet("{id}/varations")]
        public async Task<IActionResult> GetProductDetail(Guid id)
        {
            var body = await _productService.GetProductDetailAsync(id);
            if (body == null)
            {
                return NotFound();
            }
            return Ok(body);
        }

        [HttpPost("{id}/varations")]
        public async Task<IActionResult> UpdateProductDetail(Guid id, [FromBody] UpdateProductDetailReq input)
        {
            var isFound = await _productService.UpdateProductDetailAsync(id, input);
            if (!isFound)
            {
                return NotFound();
            }
            return NoContent();
        }

        /// <summary>
        /// Tạo mới sản phẩm
        /// </summary>
        /// <param name="input">Đầu vào</param>
        /// <returns status="201">Tạo thành công</returns>
        [HttpPost]
        public async Task<IActionResult> CreateProducts(CreateProductReq input)
        {
            var id = await _productService.CreateProductsAsync(input);
            return CreatedAtAction(nameof(CreateProducts), new { productId = id });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var hasValue = await _productService.DeleteProductAsync(id);
            if (!hasValue) {
                return NotFound();
            }
            return NoContent();
        }
    }
}
