using Nvk.Ddd.Domain;
using SaleCom.Domain.Shared.Varations;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaleCom.Application.Contracts.Varations
{
    public class VarationRes : AggregateRoot<Guid>, IVarationCore
    {
        public string Images { get; set; }
        public bool IsLock { get; set; }
        public string BarCode { get; set; }
        public decimal AverageImportPrice { get; set; }
        public decimal LastImportPrice { get; set; }
        public decimal RetailPrice { get; set; }
        public decimal Weight { get; set; }
        public int TotalQuantity { get; set; }
        public int RemainQuantity { get; set; }
        public decimal TotalPurchasePrice { get; set; }
    }
}
