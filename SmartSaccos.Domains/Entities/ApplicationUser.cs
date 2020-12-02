using Microsoft.AspNetCore.Identity;
using SmartSaccos.Domains.Enums;
using System.ComponentModel.DataAnnotations;

namespace SmartSaccos.Domains.Entities
{
    public class ApplicationUser : IdentityUser<int>
    {
        [MaxLength(150)]
        public string FullName { get; set; }
        public int CompanyId { get; set; }
        public UserType UserType { get; set; }
    }

    public partial class UserLogin : IdentityUserLogin<int>
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
    }
    public partial class Role : IdentityRole<int>
    {
        public int CompanyId { get; set; }
        public Role() : base()
        {
        }
        public Role(string roleName)
        {
            Name = roleName;
        }
    }
    public partial class RoleClaim : IdentityRoleClaim<int>
    {
        public int CompanyId { get; set; }
    }
    public partial class UserToken : IdentityUserToken<int>
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
    }

    public partial class UserRole : IdentityUserRole<int>
    {
        public int CompanyId { get; set; }
    }

    public partial class UserClaim : IdentityUserClaim<int>
    {
        public int CompanyId { get; set; }
    }
}
