using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SmartSaccos.ApplicationCore.DomainServices;
using SmartSaccos.ApplicationCore.Interfaces;
using SmartSaccos.ApplicationCore.Services;
using SmartSaccos.Domains.Entities;
using SmartSaccos.MemberPortal.Helpers;
using SmartSaccos.persistence.Data.Context;
using SmartSaccos.persistence.Data.Repositories;
using System.Text;

namespace SmartSaccos.MemberPortal
{
    public class Startup
    {
        private readonly string SPAOrigins = "AllowedOrigins";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var appSettingsSection = Configuration.GetSection("AppSettings");
            var appSettings = appSettingsSection.Get<AppSettings>();
            services.Configure<AppSettings>(appSettingsSection);

            services.AddCors(options =>
            {
                options.AddPolicy(name: SPAOrigins,
                                  builder =>
                                  {
                                      builder.WithOrigins(appSettings.AllowedOrigins)
                                      .AllowAnyHeader()
                                      .AllowAnyMethod();
                                  });
            });

            services.AddDbContext<SmartSaccosContext>(options =>
                        options.UseMySQL(
                            Configuration.GetConnectionString("MysqlConnection")));


            // configure jwt authentication
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            //Use objects that derive from identity TKEY
            services.AddDefaultIdentity<ApplicationUser>(
                options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 6;
                    options.Password.RequiredUniqueChars = 1;
                })
                .AddEntityFrameworkStores<SmartSaccosContext>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "witsoft.co.ke",
                        ValidAudience = "witsoft.co.ke",
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };
                });
            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            services.AddScoped<IRepository<AuditLog>, Repository<AuditLog>>();
            services.AddScoped<IRepository<Company>, Repository<Company>>();
            services.AddScoped<IRepository<Country>, Repository<Country>>();
            services.AddScoped<IRepository<Currency>, Repository<Currency>>();
            services.AddScoped<IRepository<CompanyDefaults>, Repository<CompanyDefaults>>();
            services.AddScoped<IMemberRepository<Member>, MemberRepository<Member>>();
            services.AddScoped<IRepository<Attachment>, Repository<Attachment>>();
            services.AddScoped<IRepository<MemberAttachment>, Repository<MemberAttachment>>();

            services.AddScoped(typeof(Logger));
            services.AddScoped(typeof(CompanyService));
            services.AddScoped(typeof(MemberService));
            services.AddScoped(typeof(MessageService));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseHttpsRedirection();

            app.UseFileServer();

            app.UseRouting();

            app.UseCors(SPAOrigins);

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
