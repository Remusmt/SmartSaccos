using SmartSaccos.ApplicationCore.Interfaces;
using SmartSaccos.ApplicationCore.Services;
using SmartSaccos.ApplicationCore.Specifications;
using SmartSaccos.Domains.Entities;
using SmartSaccos.Domains.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartSaccos.ApplicationCore.DomainServices
{
    public class MemberService
    {
        private readonly Logger logger;
        private readonly IRepository<CompanyDefaults> companyDefaultsRepository;
        private readonly IMemberRepository<Member> memberRepository;
        public MemberService(
            Logger loger,
            IRepository<CompanyDefaults> companyDefaultsRepo,
            IMemberRepository<Member> memberRepo
            )
        {
            logger = loger;
            memberRepository = memberRepo;
            companyDefaultsRepository = companyDefaultsRepo;
        }

        public async Task<Member> GetMemberAsync(int id)
        {
            return await memberRepository.GetByIdAsync(id);
        }

        public async Task<Member> GetMemberByUserIdAsync(int userId)
        {
            return await memberRepository
                .FirstOrDefaultAsync(e => e.ApplicationUserId == userId);
        }

        public async Task<List<Member>> GetMembersAsync(int companyId)
        {
            return await memberRepository
                .ListAsync(new MembersSpecification(companyId));
        }

        public async Task<Member> Add(Member member)
        {
            if (member.CompanyId == 0)
                throw new Exception("An error occured while saving member");

            if (string.IsNullOrWhiteSpace(member.OtherNames))
                throw new Exception("Name cannot be blank");

            memberRepository.Add(member);
            await memberRepository.SaveChangesAsync();
            await logger.Log(new AuditLog
            {
                ActionType = ActionType.Create,
                CompanyId = member.CompanyId,
                CreatedBy = member.CreatedBy,
                CreatedByName = member.CreatedByName,
                CreatedOn = member.CreatedOn,
                RecordId = member.Id,
                RecordType = RecordType.Member,
                SerializedRecord = logger.SeliarizeObject(member)
            });
            return member;
        }

        public async Task<Member> Update(
           Member member,
           int userId,
           string userFullName,
           DateTimeOffset dateTimeOffset,
           bool systemGenerated = false)
        {
            if (string.IsNullOrWhiteSpace(member.OtherNames))
            {
                throw new Exception("Account name cannot be blank");
            }
            if (memberRepository.DuplicateIdNumber(member.Id, member.IndentificationNo, member.CompanyId))
            {
                throw new Exception($"Updating member id with {member.IndentificationNo} would create a duplicate record");
            }

            memberRepository.Update(member);
            if (!systemGenerated)
            {
                await logger.Log(new AuditLog
                {
                    ActionType = ActionType.Update,
                    CompanyId = member.CompanyId,
                    CreatedBy = userId,
                    CreatedByName = userFullName,
                    CreatedOn = dateTimeOffset,
                    RecordId = member.Id,
                    RecordType = RecordType.LedgerAccount,
                    SerializedRecord = logger.SeliarizeObject(member)
                });
            }

            return member;
        }
    }
}
