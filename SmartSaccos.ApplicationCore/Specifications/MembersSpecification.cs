using SmartSaccos.Domains.Entities;

namespace SmartSaccos.ApplicationCore.Specifications
{
    public class MembersSpecification: BaseSpecification<Member>
    {
        public MembersSpecification(int companyId) :
           base(e => e.IsDeleted == false && e.CompanyId == companyId)
        {
        }
    }
}
