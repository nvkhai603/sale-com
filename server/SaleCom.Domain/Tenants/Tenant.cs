using Nvk.Ddd.Domain;
using SaleCom.Domain.Licenses;
using System;
using System.Text;

namespace SaleCom.Domain.Tenants
{
    /// <summary>
    /// Định nghĩa lớp công ty.
    /// </summary>
    public class Tenant: AggregateRoot<Guid>
    {
        public Tenant(string name)
        {
            Name = name;
        }
        /// <summary>
        /// Tên công ty.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Hình ảnh đại diện.
        /// </summary>
        public string Image { get; set; }
        public virtual DomainTenant DomainTenant { get; set; }
    }
}
