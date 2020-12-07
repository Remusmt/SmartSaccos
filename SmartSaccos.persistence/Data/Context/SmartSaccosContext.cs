using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SmartSaccos.Domains.Entities;
using System.Reflection;

namespace SmartSaccos.persistence.Data.Context
{
    public class SmartSaccosContextFactory : IDesignTimeDbContextFactory<SmartSaccosContext>
    {
        public SmartSaccosContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SmartSaccosContext>();
            optionsBuilder.UseMySQL("server=localhost;database=SmartSaccosDb;user=root;password=RMEK6078");
            return new SmartSaccosContext(optionsBuilder.Options);
        }
    }
    public class SmartSaccosContext : DbContext
    {
        public SmartSaccosContext(DbContextOptions<SmartSaccosContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            builder.Entity<UserRole>().HasKey(e => e.RoleId);
        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RoleClaim> RoleClaims { get; set; }
        public DbSet<IdentityUserClaim<int>> UserClaims { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<CompanyDefaults> CompanyDefaults { get; set; }

        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CostCenter> CostCenters { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<LedgerAccount> LedgerAccounts { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<MemberAttachment> MemberAttachments { get; set; }
        public DbSet<MemberApproval> MemberApprovals { get; set; }

    }
}
