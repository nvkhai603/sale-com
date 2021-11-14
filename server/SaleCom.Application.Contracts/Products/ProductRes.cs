using Nvk.Ddd.Domain;
using SaleCom.Application.Contracts.Varations;
using SaleCom.Domain.Shared.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaleCom.Application.Contracts.Products
{
    public class ProductRes : AggregateRoot<Guid>, IProductCore 
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public string Links { get; set; }
        public int LimitQuantityToWarn { get; set; }
        public bool IsSellNegative { get; set; }
        public string Tags { get; set; }
        public bool IsWarningByVariation { get; set; }
    }

    public class ProductDetailRes: ProductRes
    {
        public virtual ICollection<VarationRes> Varations { get; set; }
    }
}
