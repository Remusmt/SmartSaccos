namespace SmartSaccos.Domains.Entities
{
    public class MemberAddress: BaseEntity
    {
        public string Village { get; set; }
        public string Location { get; set; }
        public string District { get; set; }
        public string County { get; set; }
        public string Country { get; set; }
    }
}
