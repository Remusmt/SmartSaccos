using SmartSaccos.Domains.Enums;
using System;
using System.Collections.Generic;

namespace SmartSaccos.Domains.Entities
{
    public class Member: AppBaseEntity
    {
        public Member()
        {
            MemberAttachments = new HashSet<MemberAttachment>();
            MemberApprovals = new HashSet<MemberApproval>();
        }
        public string MemberNumber { get; set; }
        public string Title { get; set; }
        public string Surname { get; set; }
        public string OtherNames { get; set; }
        public Gender Gender { get; set; }
        public MaritalStatus MaritalStatus { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime DateJoined { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public decimal Shared { get; set; }
        public MemberStatus MemberStatus { get; set; }
        public int ApplicationUserId { get; set; }
        public string IndentificationNo { get; set; }
        public int PassportPhotoId { get; set; }
        public int IdFrontAttachmentId { get; set; }
        public int IdBackAttachmentId { get; set; }
        public int PassportCopyId { get; set; }
        public int SignatureId { get; set; }
        public int? HomeAddressId { get; set; }
        public int? PermanentAddressId { get; set; }
        public string NearestTown { get; set; }
        public string NextOfKin { get; set; }
        public string NokRelation { get; set; }
        public string NokContacts { get; set; }
        public bool NokIsMinor { get; set; }
        public string Occupation { get; set; }
        public OccupationType OccupationType { get; set; }
        public LearntAboutUs LearntAboutUs { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
        public MemberAddress HomeAddress { get; set; }
        public MemberAddress PermanentAddress { get; set; }

        public ICollection<MemberAttachment> MemberAttachments { get; set; }
        public ICollection<MemberApproval> MemberApprovals { get; set; }
    }
}
