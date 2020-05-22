using survey_polling.api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace survey_polling.api.Data.Configuration
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.ToTable("Question");

            builder.HasKey(q => q.Id);

            builder.Property(q => q.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasOne(q => q.Poll)
                .WithMany(p => p.Questions)
                .HasForeignKey(q => q.PollId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            Seed(builder);
        }

        private void Seed(EntityTypeBuilder<Question> builder)
        {
            builder.HasData(
                new Question
                {
                    Id = 1,
                    PollId = 1,
                    Title = "What is this symbol: =>"
                },
                new Question
                {
                    Id = 2,
                    PollId = 1,
                    Title = "What is 4/2?"
                },
                new Question
                {
                    Id = 3,
                    PollId = 2,
                    Title = "What is 1+1?"
                });
        }
    }
}
