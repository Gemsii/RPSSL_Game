using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RPSSL.API.Infrastructure.Persistence.Entities;

namespace RPSSL.API.Infrastructure.Persistence.Configuration
{
    public class GameConfiguration : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            builder.HasKey(g => g.Id);

            builder.Property(g => g.Id)
                .ValueGeneratedOnAdd();

            builder.Property(g => g.PlayerChoice)
                .IsRequired();

            builder.Property(g => g.ComputerChoice)
                .IsRequired();

            builder.Property(g => g.Result)
                .IsRequired();

            builder.Property(g => g.CreatedAt)
                .IsRequired();

            builder.HasOne(g => g.Player)
                .WithMany(p => p.Games)
                .HasForeignKey(g => g.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
