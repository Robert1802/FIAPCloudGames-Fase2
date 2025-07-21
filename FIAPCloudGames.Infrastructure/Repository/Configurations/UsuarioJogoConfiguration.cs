using FIAPCloudGames.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FIAPCloudGames.Infrastructure.Repository.Configurations
{
    public class UsuarioJogoConfiguration : IEntityTypeConfiguration<UsuarioJogo>
    {
        public void Configure(EntityTypeBuilder<UsuarioJogo> builder)
        {
            builder.ToTable("UsuarioJogo");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id).HasColumnType("INT").UseIdentityColumn();
            builder.Property(p => p.UsuarioId).HasColumnType("INT").IsRequired();
            builder.Property(p => p.JogoId).HasColumnType("INT").IsRequired();
            builder.Property(p => p.PrecoDaCompra).HasColumnType("DECIMAL(10,2)").IsRequired();
            builder.Property(p => p.DataCriacao).HasColumnType("DATETIME").IsRequired();

            builder.HasOne(p => p.Usuario)
                   .WithMany(u => u.UsuarioJogos)
                   .HasForeignKey(p => p.UsuarioId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.Jogo)
                   .WithMany(j => j.UsuarioJogos)
                   .HasForeignKey(p => p.JogoId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.Promocao)
                   .WithMany(pr => pr.UsuarioJogos)
                   .HasForeignKey(p => p.PromocaoId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
