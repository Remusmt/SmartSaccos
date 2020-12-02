using System;

namespace SmartSaccos.Domains.Entities
{
    public class MemberAttachment: BaseEntity
    {
        public MemberAttachment()
        {
            CreatedOn = DateTimeOffset.UtcNow;
        }
        public int MemberId { get; set; }
        public int AttachmentId { get; set; }

        public Member Member { get; set; }
        public Attachment Attachment { get; set; }
    }
}
