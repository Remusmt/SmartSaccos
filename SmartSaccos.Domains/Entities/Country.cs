using System.ComponentModel.DataAnnotations;

namespace SmartSaccos.Domains.Entities
{
    public class Country : BaseEntity
    {
        [StringLength(50)]
        public string Name { get; set; }
        public int? CurrencyId { get; set; }
        public Currency Currency { get; set; }
    }
}
