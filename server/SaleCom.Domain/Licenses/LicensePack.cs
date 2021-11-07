using Nvk.Ddd.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaleCom.Domain.Licenses
{
    /// <summary>
    /// Gói cước ứng dụng.
    /// </summary>
    public class LicensePack: AggregateRoot<int>
    {
        public string DisplayName { get; set; }
        public string Code { get; set; }
    }
}
