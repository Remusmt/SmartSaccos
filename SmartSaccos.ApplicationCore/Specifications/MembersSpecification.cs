using SmartSaccos.Domains.Entities;

namespace SmartSaccos.ApplicationCore.Specifications
{
    public class MembersSpecification: BaseSpecification<Member>
    {
        public MembersSpecification(int companyId, bool detailed = false) :
          base(e => e.IsDeleted == false && e.CompanyId == companyId)
        {
            if (detailed)
            {
                AddInclude(e => e.MemberAttachments);
                AddInclude("MemberAttachments.Attachment");
                AddInclude("MemberApprovals");
                AddInclude("HomeAddress");
                AddInclude("PermanentAddress");
            }
        }
    }
}
