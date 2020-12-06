using SmartSaccos.ApplicationCore.Interfaces;
using SmartSaccos.ApplicationCore.Models;
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
        private readonly IRepository<MemberAttachment> memberAttRepository;
        private readonly IRepository<Attachment> attachmentRepository;
        public MemberService(
            Logger loger,
            IRepository<CompanyDefaults> companyDefaultsRepo,
            IMemberRepository<Member> memberRepo,
            IRepository<MemberAttachment> memberAttRepo,
            IRepository<Attachment> attachmentRepo
            )
        {
            logger = loger;
            memberRepository = memberRepo;
            companyDefaultsRepository = companyDefaultsRepo;
            memberAttRepository = memberAttRepo;
            attachmentRepository = attachmentRepo;
        }

        public async Task<Member> GetMemberAsync(int id)
        {
            return await memberRepository.GetByIdAsync(id);
        }

        public async Task<Member> GetDetailedMemberAsync(int id)
        {
            return await memberRepository.GetDetailedMember(id);
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

        public async Task<MemberAttachment> GetMemberAttachmentAsync(int id)
        {
            return await memberRepository.GetDetailedMemberAttachment(id);
        }

        public async Task<Attachment> GetAttachmentByIdAsync(int id)
        {
            return await attachmentRepository.GetByIdAsync(id);
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

        public async Task<Member> KycDetails(
           MemberModel model,
           DateTimeOffset dateTimeOffset,
           bool systemGenerated = false)
        {

            if (string.IsNullOrWhiteSpace(model.OtherNames))
                throw new Exception("First name cannot be blank");

            if (string.IsNullOrWhiteSpace(model.IndentificationNo))
                throw new Exception("ID/Passport number cannot be blank");

            if (string.IsNullOrWhiteSpace(model.PhoneNumber))
                throw new Exception("Phone number cannot be blank");

            Member member = await GetMemberAsync(model.Id);
            if (member == null)
                throw new Exception("Member not found");

            if (memberRepository.DuplicateIdNumber(member.Id, model.IndentificationNo, member.CompanyId))
                throw new Exception($"Updating member ID/Passport number with {member.IndentificationNo} would create a duplicate record");

            member.Gender = model.Gender;
            member.IndentificationNo = model.IndentificationNo;
            member.MaritalStatus = model.MaritalStatus;
            member.OtherNames = model.OtherNames;
            member.PhoneNumber = model.PhoneNumber;
            member.Surname = model.Surname;
            member.MemberStatus = MemberStatus.KycPersonal;

            memberRepository.Update(member);
            if (!systemGenerated)
            {
                await logger.Log(new AuditLog
                {
                    ActionType = ActionType.Update,
                    CompanyId = member.CompanyId,
                    CreatedBy = member.ApplicationUserId,
                    CreatedByName = $"{member.OtherNames} {member.Surname}",
                    CreatedOn = dateTimeOffset,
                    RecordId = member.Id,
                    RecordType = RecordType.LedgerAccount,
                    SerializedRecord = logger.SeliarizeObject(member)
                });
            }

            return member;
        }

        public async Task<Member> SaveAttachment(
            Member member,
            Attachment attachment,
            AttachmentType attachmentType)
        {
            MemberAttachment memberAttachment = new MemberAttachment
            {
                MemberId = member.Id,
                Attachment = attachment,
            };
            memberAttRepository.Add(memberAttachment);
            await memberAttRepository.SaveChangesAsync();
            switch (attachmentType)
            {
                case AttachmentType.IdFront:
                    member.IdFrontAttachmentId = memberAttachment.Id;
                    member.MemberStatus = MemberStatus.KycDocs;
                    memberRepository.Update(member);
                    await memberRepository.SaveChangesAsync();
                    break;
                case AttachmentType.IdBack:
                    member.IdBackAttachmentId = memberAttachment.Id;
                    member.MemberStatus = MemberStatus.KycDocs;
                    memberRepository.Update(member);
                    await memberRepository.SaveChangesAsync();
                    break;
                case AttachmentType.Avator:
                    member.PassportPhotoId = memberAttachment.Id;
                    member.MemberStatus = MemberStatus.kycPassport;
                    memberRepository.Update(member);
                    await memberRepository.SaveChangesAsync();
                    break;
            }
            return member;
        }

        public async Task<bool> DeleteAttachmentAsync(MemberAttachment memberAttachment)
        {
            Attachment attachment = await attachmentRepository
                .GetByIdAsync(memberAttachment.AttachmentId);
            attachmentRepository.Delete(attachment);
            memberAttRepository.Delete(memberAttachment);
            await memberAttRepository.SaveChangesAsync();
            return true;
        }
    }
}
