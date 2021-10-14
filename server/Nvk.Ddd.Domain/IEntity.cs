using System;
using System.Collections.Generic;
using System.Text;

namespace Nvk.Ddd.Domain
{
    /// <summary>
    /// Defines an entity. It's primary key may not be "Id" or it may have a composite primary key.
    /// Use <see cref="IEntity{TKey}"/> where possible for better integration to repositories and other structures in the framework.
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Return array keys for this entity.
        /// </summary>
        object[] GetKeys();
        /// <summary>
        /// Time created.
        /// </summary>
        DateTime? CreationTime { get; set; }
        /// <summary>
        /// Id user create.
        /// </summary>
        Guid? CreatorId { get; set; }
        /// <summary>
        /// Creator id.
        /// </summary>
        DateTime? LastModificationTime { get; set; }
        /// <summary>
        /// Last modifier id.
        /// </summary>
        Guid? LastModifierId { get; set; }
    }

    /// <summary>
    /// Defines an entity with a single primary key with "Id" property.
    /// </summary>
    /// <typeparam name="TKey">Type of the primary key of the entity</typeparam>
    public interface IEntity<TKey> : IEntity
    {
        /// <summary>
        /// Primary key.
        /// </summary>
        TKey Id { get; set; }
    }
}
