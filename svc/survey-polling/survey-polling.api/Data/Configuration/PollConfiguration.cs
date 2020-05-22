using survey_polling.api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace survey_polling.api.Data.Configuration
{
    public class PollConfiguration : IEntityTypeConfiguration<Poll>
    {
        public void Configure(EntityTypeBuilder<Poll> builder)
        {
            builder.ToTable("Poll");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.HostId)
                .IsRequired();

            builder.Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasOne(p => p.Host)
                .WithMany()
                .HasForeignKey(p => p.HostId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            Seed(builder);
        }

        private void Seed(EntityTypeBuilder<Poll> builder)
        {
            builder.HasData(
                new Poll
                {
                    Id = 1,
                    HostId = 1,
                    Title = "Test poll #1"
                },
                new Poll
                {
                    Id = 2,
                    HostId = 1,
                    Title = "Test poll #2"
                });
        }
    }
}
