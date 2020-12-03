using SmartSaccos.Domains.Entities;

namespace SmartSaccos.ApplicationCore.Interfaces
{
    public interface IMemberRepository<T> : IRepository<T>
        where T : Member
    {
        bool IdNumberExists(string idNo, int companyId);
        bool DuplicateIdNumber(int id, string idNo, int companyId);
    }
}
