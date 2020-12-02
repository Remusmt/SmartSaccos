using SmartSaccos.Domains.Entities;

namespace SmartSaccos.ApplicationCore.Interfaces
{
    public interface ICategoryRepository<T> : IRepository<T>
            where T : Category
    {
        bool CodeExists(string code, int companyId);
        bool DescriptionExists(string description, int companyId);
        bool DuplicateCode(int id, string code, int companyId);
        bool DuplicateDescription(int id, string description, int companyId);
        string GetCode(int companyId);
    }
}
