using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SaleCom.Application.Contracts.Products
{
    public interface IProductService: IAppService
    {
        Task CreateProductsAsync(CreateProductReq input);
        Task UpdateProductsAsync();
        Task DeleteProductsAsync();
    }
}
