using Core.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Repository.Configurations
{
    public class PromocaoConfiguration : IEntityTypeConfiguration<Promocao>
    {
        public void Configure(EntityTypeBuilder<Promocao> builder)
        {
            builder.ToTable("Promocao");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id).HasColumnType("INT").UseIdentityColumn();
            builder.Property(p => p.Nome).HasColumnType("VARCHAR(50)").IsRequired();
            builder.Property(p => p.DataInicio).HasColumnType("DATETIME").IsRequired();
            builder.Property(p => p.DataFim).HasColumnType("DATETIME").IsRequired();
            builder.Property(p => p.Ativo).HasColumnType("BIT").IsRequired();
            builder.Property(p => p.DataCriacao).HasColumnType("DATETIME").IsRequired();

            builder.HasOne(p => p.Usuario)
                   .WithMany(u => u.Promocoes)
                   .HasForeignKey(p => p.UsuarioId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
