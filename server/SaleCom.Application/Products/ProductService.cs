using Nvk.Data;
using SaleCom.Application.Contracts.Products;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SaleCom.Application.Products
{
    public class ProductService : AppService, IProductService
    {
        public ProductService(ILazyServiceProvider lazyServiceProvider) : base(lazyServiceProvider)
        {
        }

        public Task CreateProductsAsync(CreateProductReq input)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProductsAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateProductsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
