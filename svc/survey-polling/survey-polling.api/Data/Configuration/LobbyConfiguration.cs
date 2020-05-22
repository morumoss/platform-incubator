using survey_polling.api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace survey_polling.api.Data.Configuration
{
    public class LobbyConfiguration : IEntityTypeConfiguration<Lobby>
    {
        public void Configure(EntityTypeBuilder<Lobby> builder)
        {
            builder.ToTable("Lobby");

            builder.HasKey(l => l.Id);

            builder.Property(l => l.PollId)
                .IsRequired();

            builder.Property(l => l.Pin)
               .IsRequired();

            builder.HasOne(l => l.Poll)
                .WithMany(p => p.Lobbies)
                .HasForeignKey(l => l.PollId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(l => l.ActiveQuestion)
                .WithOne();

            Seed(builder);
        }

        private void Seed(EntityTypeBuilder<Lobby> builder)
        {
            //builder.HasData(
            //    new Lobby
            //    {
            //        Id = 1,
            //        PollId = 1,
            //        Pin = "241573"
            //    });
        }
    }
}
