using System.ComponentModel.DataAnnotations;

namespace SmartSaccos.Domains.Entities
{
    public class Currency : BaseEntity
    {
        public int CountryId { get; set; }
        [StringLength(50)]
        public string ISOCode { get; set; }
        [StringLength(150)]
        public string Name { get; set; }

        public Country Country { get; set; }
    }
}
