using SmartSaccos.ApplicationCore.Interfaces;
using SmartSaccos.ApplicationCore.Models;
using SmartSaccos.ApplicationCore.Services;
using SmartSaccos.ApplicationCore.Specifications;
using SmartSaccos.Domains.Entities;
using SmartSaccos.Domains.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartSaccos.ApplicationCore.DomainServices
{
    public class MemberService
    {
        private readonly Logger logger;
        private readonly IMemberRepository<Member> memberRepository;
        private readonly IRepository<Attachment> attachmentRepository;
        private readonly IRepository<MemberAddress> addressRepository;
        private readonly IRepository<MemberAttachment> memberAttRepository;
        private readonly IRepository<MemberApproval> memberApprovalRepository;
        private readonly IRepository<CompanyDefaults> companyDefaultsrepository;
        public MemberService(
            Logger loger,
            IMemberRepository<Member> memberRepo,
            IRepository<Attachment> attachmentRepo,
            IRepository<MemberAddress> addressRepo,
            IRepository<MemberAttachment> memberAttRepo,
            IRepository<MemberApproval> memberApprovalRepo,
            IRepository<CompanyDefaults> companyDefaultsrepo
            )
        {
            logger = loger;
            memberRepository = memberRepo;
            addressRepository = addressRepo;
            memberAttRepository = memberAttRepo;
            attachmentRepository = attachmentRepo;
            memberApprovalRepository = memberApprovalRepo;
            companyDefaultsrepository = companyDefaultsrepo;
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

        public async Task<List<Member>> GetDetailedMembersAsync(int companyId)
        {
            return await memberRepository
                .ListAsync(new MembersSpecification(companyId, true));
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
            member.PassportCopyId = model.PassportCopyId;
            member.SignatureId = model.SignatureId;           
            member.NearestTown = model.NearestTown;
            member.NextOfKin = model.NextOfKin;
            member.NokRelation = model.NokRelation;
            member.NokContacts = model.NokContacts;
            member.NokIsMinor = model.NokIsMinor;
            member.Occupation = model.Occupation;
            member.OccupationType = model.OccupationType;
            member.LearntAboutUs = model.LearntAboutUs;
            member.Title = model.Title;

            bool save = false;
            MemberAddress homeAddress;
            if (member.HomeAddressId.HasValue)
            {
                homeAddress = await addressRepository
                    .GetByIdAsync(member.HomeAddressId.Value);

                homeAddress.Country = model.PermanentAddress?.Country;
                homeAddress.County = model.PermanentAddress?.County;
                homeAddress.District = model.PermanentAddress?.District;
                homeAddress.Location = model.PermanentAddress?.Location;
                homeAddress.Village = model.PermanentAddress?.Village;

                addressRepository.Update(homeAddress);
            }
            else
            {
                homeAddress = new MemberAddress
                {
                    Country = model.HomeAddress?.Country,
                    County = model.HomeAddress?.County,
                    District = model.HomeAddress?.District,
                    Location = model.HomeAddress?.Location,
                    Village = model.HomeAddress?.Village
                };
                addressRepository.Add(homeAddress);
                save = true;
            }

            MemberAddress permanentAddress;
            if (member.PermanentAddressId.HasValue)
            {
                permanentAddress = await addressRepository
                    .GetByIdAsync(member.PermanentAddressId.Value);
                permanentAddress.Country = model.PermanentAddress?.Country;
                permanentAddress.County = model.PermanentAddress?.County;
                permanentAddress.District = model.PermanentAddress?.District;
                permanentAddress.Location = model.PermanentAddress?.Location;
                permanentAddress.Village = model.PermanentAddress?.Village;
                
                addressRepository.Update(permanentAddress);
            }
            else
            {
                permanentAddress = new MemberAddress
                {
                    Country = model.PermanentAddress?.Country,
                    County = model.PermanentAddress?.County,
                    District = model.PermanentAddress?.District,
                    Location = model.PermanentAddress?.Location,
                    Village = model.PermanentAddress?.Village
                };
                addressRepository.Add(permanentAddress);
                save = true;
            }

            if (save) await addressRepository.SaveChangesAsync();
           
            member.HomeAddressId = homeAddress.Id;
            member.PermanentAddressId = permanentAddress.Id;

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
                case AttachmentType.PassportCopy:
                    member.PassportCopyId = memberAttachment.Id;
                    member.MemberStatus = MemberStatus.KycDocs;
                    memberRepository.Update(member);
                    await memberRepository.SaveChangesAsync();
                    break;
                case AttachmentType.Signature:
                    member.SignatureId = memberAttachment.Id;
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

        public async Task<Member> ApproveMember(MemberApproval memberApproval)
        {
            Member member = await GetMemberByUserIdAsync(memberApproval.MemberId);
            if (member == null)
                throw new Exception("Member not found");

            CompanyDefaults companyDefaults = await companyDefaultsrepository
                .FirstOrDefaultAsync(e => e.CompanyId == member.CompanyId);

            member.MemberStatus = MemberStatus.PaidMembership;
            member.MemberNumber = GetMemberNumber(member, companyDefaults.MemberNumber);
            memberRepository.Update(member);
            companyDefaults.MemberNumber += 1;
            companyDefaultsrepository.Update(companyDefaults);
            memberApprovalRepository.Add(memberApproval);

            await memberApprovalRepository.SaveChangesAsync();
            return await GetDetailedMemberAsync(memberApproval.MemberId);
        }

        public async Task<Member> PutMemberOnHold(MemberApproval memberApproval)
        {
            Member member = await GetMemberByUserIdAsync(memberApproval.MemberId);
            if (member == null)
                throw new Exception("Member not found");

            member.MemberStatus = MemberStatus.OnHold;
            memberRepository.Update(member);
            memberApprovalRepository.Add(memberApproval);

            await memberApprovalRepository.SaveChangesAsync();
            return await GetDetailedMemberAsync(memberApproval.MemberId);
        }

        public async Task<Member> RejectMember(MemberApproval memberApproval)
        {
            Member member = await GetMemberByUserIdAsync(memberApproval.MemberId);
            if (member == null)
                throw new Exception("Member not found");

            member.MemberStatus = MemberStatus.Rejected;
            memberRepository.Update(member);
            memberApprovalRepository.Add(memberApproval);

            await memberApprovalRepository.SaveChangesAsync();
            return await GetDetailedMemberAsync(memberApproval.MemberId);
        }

        private string GetMemberNumber(Member member, int currentNumber)
        {
            StringBuilder sb = new StringBuilder(DateTime.Now.Month.ToString().PadLeft(2, '0'));
            sb.Append((DateTime.Now.Year - 2000).ToString());
            sb.Append(member.IndentificationNo.Substring(0, 1));
            sb.Append(member.IndentificationNo.Substring(member.IndentificationNo.Length - 1, 1));
            sb.Append((currentNumber + 1).ToString().PadLeft(4, '0'));
            return sb.ToString();
        }
    }
}
