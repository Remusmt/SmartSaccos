using SmartSaccos.Domains.Enums;

namespace SmartSaccos.Domains.Entities
{
    public class MemberApproval: BaseEntity
    {
        public int ApplicationUserId { get; set; }
        public int MemberId { get; set; }
        public ApprovalAction ApprovalAction { get; set; }
        public string MessageToMember { get; set; }
        public string Comments { get; set; }

        public Member Member { get; set; }
    }
}
