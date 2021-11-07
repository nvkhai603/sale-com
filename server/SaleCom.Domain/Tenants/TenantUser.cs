using Nvk.Ddd.Domain;
using SaleCom.Domain.Identity;
using System;
using System.Text;

namespace SaleCom.Domain.Tenants
{
    /// <summary>
    /// Định nghĩa lớp người dùng trong công ty.
    /// </summary>
    public class TenantUser : AggregateRoot, ISoftDelete
    {
        /// <summary>
        /// Id người dùng.
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// Id công ty.
        /// </summary>
        public Guid TenantId { get; set; }
        public virtual Tenant Tenant { get; set; }
        public virtual AppUser User { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? DeletedId { get; set; }
        public DateTime? DeletionTime { get; set; }
    }
}
