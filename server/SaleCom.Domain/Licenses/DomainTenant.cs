using Nvk.Ddd.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaleCom.Domain.Licenses
{
    /// <summary>
    /// Miền, Khái niệm lớn nhất, là cha của các công ty, chỉ có 1 người sở h
    /// </summary>
    public class DomainTenant: AggregateRoot
    {
        public DomainTenant(Guid ownerId)
        {
            OwnerId = ownerId;
        }
        public Guid OwnerId { get; set; }
    }
}
