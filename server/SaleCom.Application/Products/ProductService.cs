using Nvk.Data;
using Nvk.EntityFrameworkCore.UnitOfWork;
using Nvk.EntityFrameworkCore.UnitOfWork.Collections;
using SaleCom.Application.Contracts.Products;
using SaleCom.Domain.Products;
using SaleCom.EntityFramework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace SaleCom.Application.Products
{
    public class ProductService : AppService, IProductService
    {
        private IUnitOfWork<SaleComDbContext> _uow;
        public ProductService(ILazyServiceProvider lazyServiceProvider, IUnitOfWork<SaleComDbContext> uow) : base(lazyServiceProvider)
        {
            _uow = uow;
        }

        public async Task<Guid> CreateProductsAsync([JetBrains.Annotations.NotNull] CreateProductReq input)
        {
            var productRepo = _uow.GetRepository<Product>();
            var product = _mapper.Map<CreateProductReq, Product>(input);
            await productRepo.InsertAsync(product);
            await _uow.SaveChangesAsync();
            return product.Id;
        }

        public async Task<bool> DeleteProductAsync([JetBrains.Annotations.NotNull] Guid id)
        {
            var productRepo = _uow.GetRepository<Product>();
            var product = await productRepo.FindAsync(id);
            if (product == null)
            {
                return false;
            }
            productRepo.Delete(product);
            await _uow.SaveChangesAsync();
            return true;
        }

        public Task DeleteProductsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ProductDetailRes> GetProductDetailAsync([JetBrains.Annotations.NotNull] Guid id)
        {
            var productRepo = _uow.GetRepository<Product>();
            var productDetail = await productRepo.GetFirstOrDefaultAsync(predicate: (x => x.Id.Equals(id)), include: src => src.Include(prd => prd.Varations));
            if (productDetail == null) {
                return null;
            }
            var productDetailDto = _mapper.Map<Product, ProductDetailRes>(productDetail);
            return productDetailDto;
        }

        public async Task<IPagedList<ProductRes>> GetProductsAsync()
        {
            var productRepo = _uow.GetRepository<Product>();
            var pagedProduct = await productRepo.GetPagedListAsync();
            return PagedList.From(pagedProduct, converter: x => _mapper.Map<IEnumerable<Product>, IEnumerable<ProductRes>>(x));
        }

        public async Task<bool> UpdateProductDetailAsync([JetBrains.Annotations.NotNull] Guid id, [JetBrains.Annotations.NotNull] UpdateProductDetailReq input)
        {
            var productRepo = _uow.GetRepository<Product>();
            // Lấy về ProductDetail và Tracking
            var productDetail = await productRepo.GetFirstOrDefaultAsync(predicate: (x => x.Id.Equals(id)), include: src => src.Include(prd => prd.Varations), disableTracking: false);
            if (productDetail == null)
            {
                return false;
            }
            productDetail = _mapper.Map(input, productDetail);
            //TODO: Cập nhật biến thể
            await _uow.SaveChangesAsync();
            return true;
        }

        public Task UpdateProductsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
