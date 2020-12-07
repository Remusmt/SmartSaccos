using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSaccos.Domains.Entities;

namespace SmartSaccos.persistence.Data.Config
{
    public class MembersConfiguration : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.HasOne(e => e.ApplicationUser)
                .WithMany()
                .HasForeignKey(e => e.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.MemberAttachments)
                .WithOne(e => e.Member)
                .HasForeignKey(e => e.MemberId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.MemberApprovals)
                .WithOne(e => e.Member)
                .HasForeignKey(e => e.MemberId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class MemberAttachmentConfiguration : IEntityTypeConfiguration<MemberAttachment>
    {
        public void Configure(EntityTypeBuilder<MemberAttachment> builder)
        {
            builder.HasOne(e => e.Attachment)
                .WithMany()
                .HasForeignKey(e => e.AttachmentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
