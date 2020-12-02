using System;
using System.ComponentModel.DataAnnotations;

namespace SmartSaccos.Domains.Entities
{
    public abstract class BaseEntity
    {
        public BaseEntity()
        {
            CreatedOn = DateTimeOffset.UtcNow;
        }
        /// <summary>
        /// This is a DBMS generated primary key
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Offers a way to implement soft delete
        /// </summary>
        public bool IsDeleted { get; set; }
        /// <summary>
        /// Holds version number of the entity.
        /// It is used to lock an entity for use by one app/user at a time
        /// As soon as the entity is modified the UpdateCode is incremented.
        /// Apps with a lower updatecode are not allowed to edit the entity
        /// </summary>
        public int UpdateCode { get; set; }
        /// <summary>
        /// Will be picked when creating the item for the first time
        /// </summary>
        public DateTimeOffset CreatedOn { get; set; }
    }
}
