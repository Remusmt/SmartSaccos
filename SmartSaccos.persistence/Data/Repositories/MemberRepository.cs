using Microsoft.EntityFrameworkCore;
using SmartSaccos.ApplicationCore.Interfaces;
using SmartSaccos.Domains.Entities;
using SmartSaccos.persistence.Data.Context;
using System.Linq;
using System.Threading.Tasks;

namespace SmartSaccos.persistence.Data.Repositories
{
    public class MemberRepository<T> : Repository<T>, IMemberRepository<T>
        where T : Member
    {
        public MemberRepository(SmartSaccosContext context) : base(context)
        {
        }

        public bool DuplicateIdNumber(int id, string idNo, int companyId)
        {
            return smartSaccosContext.Set<T>()
                  .Any(e => e.IndentificationNo == idNo 
                  && e.CompanyId == companyId && e.Id != id);
        }

        public async Task<T> GetDetailedMember(int id)
        {
            return await smartSaccosContext.Set<T>()
                .Include("MemberAttachments")
                .Include("MemberAttachments.Attachment")
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<MemberAttachment> GetDetailedMemberAttachment(int id)
        {
            return await smartSaccosContext.MemberAttachments
                .Include("Attachment")
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public bool IdNumberExists(string idNo, int companyId)
        {
            return smartSaccosContext.Set<T>()
                .Any(e => e.IndentificationNo == idNo && e.CompanyId == companyId);
            
        }
    }
}
