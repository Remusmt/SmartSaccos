using System.ComponentModel.DataAnnotations;

namespace SmartSaccos.Domains.Entities
{
    public abstract class AppBaseEntity: BaseEntity
    {
        /// <summary>
        /// All entries should have a reference to the clients id from the
        /// subscription module.
        /// This enables partioning of users data
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// Holds Id of the user that created this record
        /// </summary>
        public int CreatedBy { get; set; }
        /// <summary>
        /// Holds the name of the logged in user when the record is created
        /// Should be supplied by the user manager in the controllers
        /// </summary>
        [StringLength(150)]
        public string CreatedByName { get; set; }
    }
}
