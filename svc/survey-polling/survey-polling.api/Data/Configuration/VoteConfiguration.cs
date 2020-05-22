using survey_polling.api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace survey_polling.api.Data.Configuration
{
    public class VoteConfiguration : IEntityTypeConfiguration<Vote>
    {
        public void Configure(EntityTypeBuilder<Vote> builder)
        {
            builder.ToTable("Vote");

            builder.HasKey(v => v.Id);

            builder.Property(v => v.PollId)
                .IsRequired();

            builder.HasOne(v => v.Poll)
                .WithMany()
                .HasForeignKey(p => p.PollId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.HasOne(v => v.Option)
                .WithMany()
                .HasForeignKey(v => v.OptionId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.HasOne(v => v.Question)
                .WithMany(q => q.Votes)
                .HasForeignKey(v => v.QuestionId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            //Seed(builder);
        }

        private void Seed(EntityTypeBuilder<Vote> builder)
        {
            builder.HasData
                (
                    new Vote
                    {
                        Id = 1,
                        OptionId = 1,
                        PollId = 1,
                        QuestionId = 1,
                    }
                );
        }
    }
}
