using Nvk.Data;
using Nvk.EntityFrameworkCore.UnitOfWork;
using SaleCom.Application.Contracts.Products;
using SaleCom.Domain.Products;
using SaleCom.EntityFramework;
using System;
using System.Threading.Tasks;

namespace SaleCom.Application.Products
{
    public class ProductService : AppService, IProductService
    {
        private IUnitOfWork<SaleComDbContext> _uow;
        public ProductService(ILazyServiceProvider lazyServiceProvider, IUnitOfWork<SaleComDbContext> uow) : base(lazyServiceProvider)
        {
            _uow = uow;
        }

        public async Task CreateProductsAsync(CreateProductReq input)
        {
            var productRepo = _uow.GetRepository<Product>();
            var product = _mapper.Map<CreateProductReq, Product>(input);
            await productRepo.InsertAsync(product);
            await _uow.SaveChangesAsync();
            return;
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
