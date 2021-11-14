using Nvk.EntityFrameworkCore.UnitOfWork.Collections;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SaleCom.Application.Contracts.Products
{
    public interface IProductService : IAppService
    {
        Task<Guid> CreateProductsAsync([JetBrains.Annotations.NotNull] CreateProductReq input);
        Task UpdateProductsAsync();
        Task DeleteProductsAsync();
        Task<bool> DeleteProductAsync([JetBrains.Annotations.NotNull] Guid id);
        Task<IPagedList<ProductRes>> GetProductsAsync();
        Task<ProductDetailRes> GetProductDetailAsync([JetBrains.Annotations.NotNull] Guid id);
        Task<bool> UpdateProductDetailAsync([JetBrains.Annotations.NotNull] Guid id, [JetBrains.Annotations.NotNull] UpdateProductDetailReq input);
    }
}
