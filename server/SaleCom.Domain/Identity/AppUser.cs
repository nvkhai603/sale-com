using Microsoft.AspNetCore.Identity;
using Nvk.Ddd.Domain;
using SaleCom.Domain.Tenants;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaleCom.Domain.Identity
{
    public class AppUser : IdentityUser<Guid>, IAggregateRoot, IHasConcurrencyStamp, ISoftDelete
    {
        public AppUser() : base()
        {

        }

        public AppUser(string userName) : base(userName)
        {

        }
        public DateTime? CreationTime { get; set; }
        public Guid? CreatorId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public Guid? LastModifierId { get; set; }
        public bool IsDeleted { get ; set ; }
        public Guid? DeletedId { get ; set ; }
        public DateTime? DeletionTime { get ; set ; }
        public object[] GetKeys()
        {
            throw new NotImplementedException();
        }
    }
}
