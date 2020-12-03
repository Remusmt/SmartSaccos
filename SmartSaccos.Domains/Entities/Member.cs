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
        }
        public string MemberNumber { get; set; }
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

        public ApplicationUser ApplicationUser { get; set; }
        public ICollection<MemberAttachment> MemberAttachments { get; set; }

    }
}
