using System.ComponentModel.DataAnnotations;

namespace SmartSaccos.Domains.Entities
{
    public class Category : AppBaseEntity
    {
        [StringLength(50)]
        public string Code { get; set; }
        [StringLength(150)]
        public string Description { get; set; }
    }
}
