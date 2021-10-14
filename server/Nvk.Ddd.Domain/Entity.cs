using System;
using System.Collections.Generic;
using System.Text;

namespace Nvk.Ddd.Domain
{
    public class Entity : IEntity
    {
        /// <summary>
        /// Time created.
        /// </summary>
        public DateTime? CreationTime { get; set; }
        /// <summary>
        /// Id user create.
        /// </summary>
        public Guid? CreatorId { get; set; }
        /// <summary>
        /// Creator id.
        /// </summary>
        public DateTime? LastModificationTime { get; set; }
        /// <summary>
        /// Last modifier id.
        /// </summary>
        public Guid? LastModifierId { get; set; }

        /// <summary>
        /// Return array keys for this entity.
        /// </summary>
        public object[] GetKeys()
        {
            throw new NotImplementedException();
        }
    }

    public class Entity<TKey> : Entity, IEntity<TKey>
    {
        /// <summary>
        /// Primary key.
        /// </summary>
        public TKey Id { get; set; }
    }
}
