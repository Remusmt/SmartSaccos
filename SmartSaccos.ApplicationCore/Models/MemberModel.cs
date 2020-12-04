using SmartSaccos.Domains.Enums;

namespace SmartSaccos.ApplicationCore.Models
{
    public class MemberModel
    {
        public int Id { get; set; }
        public string MemberNumber { get; set; }
        public string Surname { get; set; }
        public string OtherNames { get; set; }
        public Gender Gender { get; set; }
        public MaritalStatus MaritalStatus { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string IndentificationNo { get; set; }
    }
}
