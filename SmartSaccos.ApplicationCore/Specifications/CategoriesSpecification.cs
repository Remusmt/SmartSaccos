using SmartSaccos.Domains.Entities;

namespace SmartSaccos.ApplicationCore.Specifications
{
    public class CategoriesSpecification<T> : BaseSpecification<T>
        where T : Category
    {
        public CategoriesSpecification(int companyId) :
            base(e => e.IsDeleted == false && e.CompanyId == companyId)
        {
        }
    }
}
