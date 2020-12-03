using SmartSaccos.ApplicationCore.Interfaces;
using SmartSaccos.Domains.Entities;
using SmartSaccos.persistence.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public bool IdNumberExists(string idNo, int companyId)
        {
            return smartSaccosContext.Set<T>()
                .Any(e => e.IndentificationNo == idNo && e.CompanyId == companyId);
            
        }
    }
}
