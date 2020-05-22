using survey_polling.api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace survey_polling.api.Data.Configuration
{
    public class OptionConfiguration : IEntityTypeConfiguration<Option>
    {
        public void Configure(EntityTypeBuilder<Option> builder)
        {
            builder.ToTable("Option");

            builder.HasKey(o => o.Id);

            builder.Property(o => o.Content)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasOne(o => o.Poll)
                .WithMany()
                .HasForeignKey(o => o.PollId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            Seed(builder);
        }

        private void Seed(EntityTypeBuilder<Option> builder)
        {
            builder.HasData(
                new Option
                {
                    Id = 1,
                    PollId = 1,
                    QuestionId = 1,
                    Content = "Nothing",
                },
                new Option
                {
                    Id = 2,
                    PollId = 1,
                    QuestionId = 1,
                    Content = "Greater than or equal to",
                },
                new Option
                {
                    Id = 3,
                    PollId = 1,
                    QuestionId = 1,
                    Content = "Lambda",
                },
                new Option
                {
                    Id = 4,
                    PollId = 1,
                    QuestionId = 2,
                    Content = "1",
                },
                new Option
                {
                    Id = 5,
                    PollId = 1,
                    QuestionId = 2,
                    Content = "2",
                });
        }
    }
}
