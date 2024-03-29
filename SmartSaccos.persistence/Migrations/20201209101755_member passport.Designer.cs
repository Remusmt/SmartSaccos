﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SmartSaccos.persistence.Data.Context;

namespace SmartSaccos.persistence.Migrations
{
    [DbContext(typeof(SmartSaccosContext))]
    [Migration("20201209101755_member passport")]
    partial class memberpassport
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("UserClaims");
                });

            modelBuilder.Entity("SmartSaccos.Domains.Entities.ApplicationUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FullName")
                        .HasColumnType("varchar(150)")
                        .HasMaxLength(150);

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("text");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("text");

                    b.Property<int>("UserType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("ApplicationUsers");
                });

            modelBuilder.Entity("SmartSaccos.Domains.Entities.Attachment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<string>("ContentDisposition")
                        .HasColumnType("text");

                    b.Property<string>("ContentType")
                        .HasColumnType("text");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<string>("CreatedByName")
                        .HasColumnType("varchar(150)")
                        .HasMaxLength(150);

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("timestamp");

                    b.Property<string>("Extension")
                        .HasColumnType("text");

                    b.Property<string>("FileName")
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<long>("Length")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("RootPath")
                        .HasColumnType("text");

                    b.Property<string>("SystemFileName")
                        .HasColumnType("text");

                    b.Property<int>("UpdateCode")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Attachments");
                });

            modelBuilder.Entity("SmartSaccos.Domains.Entities.AuditLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ActionType")
                        .HasColumnType("int");

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<string>("CreatedByName")
                        .HasColumnType("varchar(150)")
                        .HasMaxLength(150);

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("timestamp");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("RecordId")
                        .HasColumnType("int");

                    b.Property<int>("RecordType")
                        .HasColumnType("int");

                    b.Property<string>("SerializedRecord")
                        .HasColumnType("text");

                    b.Property<int>("UpdateCode")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("AuditLogs");
                });

            modelBuilder.Entity("SmartSaccos.Domains.Entities.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Code")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50);

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<string>("CreatedByName")
                        .HasColumnType("varchar(150)")
                        .HasMaxLength(150);

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("timestamp");

                    b.Property<string>("Description")
                        .HasColumnType("varchar(150)")
                        .HasMaxLength(150);

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("UpdateCode")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Category");
                });

            modelBuilder.Entity("SmartSaccos.Domains.Entities.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("CompanyName")
                        .HasColumnType("varchar(150)")
                        .HasMaxLength(150);

                    b.Property<int>("CountryId")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("timestamp");

                    b.Property<int>("CurrencyId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("PoBox")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("PostalCode")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("RegistrationNumber")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Town")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50);

                    b.Property<int>("UpdateCode")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.HasIndex("CurrencyId");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("SmartSaccos.Domains.Entities.CompanyDefaults", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("AllowPostingToParentAccount")
                        .HasColumnType("bit");

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<string>("CreatedByName")
                        .HasColumnType("varchar(150)")
                        .HasMaxLength(150);

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("timestamp");

                    b.Property<int>("CurrentFinancialYear")
                        .HasColumnType("int");

                    b.Property<int>("DefaultCurrency")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("MemberNumber")
                        .HasColumnType("int");

                    b.Property<int>("UpdateCode")
                        .HasColumnType("int");

                    b.Property<bool>("UseAccountNumbers")
                        .HasColumnType("bit");

                    b.Property<bool>("UseFinancialYear")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("CompanyDefaults");
                });

            modelBuilder.Entity("SmartSaccos.Domains.Entities.Country", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("timestamp");

                    b.Property<int?>("CurrencyId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50);

                    b.Property<int>("UpdateCode")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CurrencyId");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("SmartSaccos.Domains.Entities.Currency", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CountryId")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("timestamp");

                    b.Property<string>("ISOCode")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50);

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(150)")
                        .HasMaxLength(150);

                    b.Property<int>("UpdateCode")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.ToTable("Currencies");
                });

            modelBuilder.Entity("SmartSaccos.Domains.Entities.LedgerAccount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("AccountName")
                        .HasColumnType("varchar(150)")
                        .HasMaxLength(150);

                    b.Property<string>("AccountNumber")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50);

                    b.Property<int>("AccountType")
                        .HasColumnType("int");

                    b.Property<bool>("AddToDashboard")
                        .HasColumnType("bit");

                    b.Property<decimal>("Balance")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<string>("BankAccountNo")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50);

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<string>("CreatedByName")
                        .HasColumnType("varchar(150)")
                        .HasMaxLength(150);

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("timestamp");

                    b.Property<decimal>("CurrencyBalance")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<int>("CurrencyId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<int>("DetailAccountType")
                        .HasColumnType("int");

                    b.Property<bool>("HasOverDraft")
                        .HasColumnType("bit");

                    b.Property<byte>("Height")
                        .HasColumnType("tinyint");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<decimal>("OverDraftLimit")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<int?>("ParentAccountId")
                        .HasColumnType("int");

                    b.Property<bool>("ShowInPettyCash")
                        .HasColumnType("bit");

                    b.Property<int?>("TaxRateId")
                        .HasColumnType("int");

                    b.Property<int>("UpdateCode")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CurrencyId");

                    b.HasIndex("ParentAccountId");

                    b.ToTable("LedgerAccounts");
                });

            modelBuilder.Entity("SmartSaccos.Domains.Entities.Member", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ApplicationUserId")
                        .HasColumnType("int");

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<string>("CreatedByName")
                        .HasColumnType("varchar(150)")
                        .HasMaxLength(150);

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("timestamp");

                    b.Property<DateTime>("DateJoined")
                        .HasColumnType("datetime");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("datetime");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<int>("IdBackAttachmentId")
                        .HasColumnType("int");

                    b.Property<int>("IdFrontAttachmentId")
                        .HasColumnType("int");

                    b.Property<string>("IndentificationNo")
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("MaritalStatus")
                        .HasColumnType("int");

                    b.Property<string>("MemberNumber")
                        .HasColumnType("text");

                    b.Property<int>("MemberStatus")
                        .HasColumnType("int");

                    b.Property<string>("OtherNames")
                        .HasColumnType("text");

                    b.Property<int>("PassportCopyId")
                        .HasColumnType("int");

                    b.Property<int>("PassportPhotoId")
                        .HasColumnType("int");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<decimal>("Shared")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<string>("Surname")
                        .HasColumnType("text");

                    b.Property<int>("UpdateCode")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationUserId");

                    b.ToTable("Members");
                });

            modelBuilder.Entity("SmartSaccos.Domains.Entities.MemberApproval", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ApplicationUserId")
                        .HasColumnType("int");

                    b.Property<int>("ApprovalAction")
                        .HasColumnType("int");

                    b.Property<string>("Comments")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("timestamp");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("MemberId")
                        .HasColumnType("int");

                    b.Property<string>("MessageToMember")
                        .HasColumnType("text");

                    b.Property<int>("UpdateCode")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MemberId");

                    b.ToTable("MemberApprovals");
                });

            modelBuilder.Entity("SmartSaccos.Domains.Entities.MemberAttachment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AttachmentId")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("timestamp");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("MemberId")
                        .HasColumnType("int");

                    b.Property<int>("UpdateCode")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AttachmentId");

                    b.HasIndex("MemberId");

                    b.ToTable("MemberAttachments");
                });

            modelBuilder.Entity("SmartSaccos.Domains.Entities.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("NormalizedName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("SmartSaccos.Domains.Entities.RoleClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("RoleClaims");
                });

            modelBuilder.Entity("SmartSaccos.Domains.Entities.UserLogin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("UserLogins");
                });

            modelBuilder.Entity("SmartSaccos.Domains.Entities.UserRole", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("RoleId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("SmartSaccos.Domains.Entities.UserToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("UserTokens");
                });

            modelBuilder.Entity("SmartSaccos.Domains.Entities.CostCenter", b =>
                {
                    b.HasBaseType("SmartSaccos.Domains.Entities.Category");

                    b.HasDiscriminator().HasValue("CostCenter");
                });

            modelBuilder.Entity("SmartSaccos.Domains.Entities.Department", b =>
                {
                    b.HasBaseType("SmartSaccos.Domains.Entities.Category");

                    b.HasDiscriminator().HasValue("Department");
                });

            modelBuilder.Entity("SmartSaccos.Domains.Entities.Company", b =>
                {
                    b.HasOne("SmartSaccos.Domains.Entities.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("SmartSaccos.Domains.Entities.Currency", "Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("SmartSaccos.Domains.Entities.Country", b =>
                {
                    b.HasOne("SmartSaccos.Domains.Entities.Currency", "Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyId");
                });

            modelBuilder.Entity("SmartSaccos.Domains.Entities.Currency", b =>
                {
                    b.HasOne("SmartSaccos.Domains.Entities.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("SmartSaccos.Domains.Entities.LedgerAccount", b =>
                {
                    b.HasOne("SmartSaccos.Domains.Entities.Currency", "Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartSaccos.Domains.Entities.LedgerAccount", "ParentAccount")
                        .WithMany("ChildAccounts")
                        .HasForeignKey("ParentAccountId");
                });

            modelBuilder.Entity("SmartSaccos.Domains.Entities.Member", b =>
                {
                    b.HasOne("SmartSaccos.Domains.Entities.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("ApplicationUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("SmartSaccos.Domains.Entities.MemberApproval", b =>
                {
                    b.HasOne("SmartSaccos.Domains.Entities.Member", "Member")
                        .WithMany("MemberApprovals")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SmartSaccos.Domains.Entities.MemberAttachment", b =>
                {
                    b.HasOne("SmartSaccos.Domains.Entities.Attachment", "Attachment")
                        .WithMany()
                        .HasForeignKey("AttachmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartSaccos.Domains.Entities.Member", "Member")
                        .WithMany("MemberAttachments")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
