using Core.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Repository.Configurations
{
    public class UsuarioJogoConfiguration : IEntityTypeConfiguration<UsuarioJogo>
    {
        public void Configure(EntityTypeBuilder<UsuarioJogo> builder)
        {
            builder.ToTable("UsuarioJogo");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnType("INT").UseIdentityColumn();
            builder.Property(p => p.IdUsuario).HasColumnType("INT").IsRequired();
            builder.Property(p => p.IdJogo).HasColumnType("INT").IsRequired();
            builder.Property(p => p.PrecoDaCompra).HasColumnType("DECIMAL(10,0)").IsRequired();
            builder.Property(p => p.DataCriacao).HasColumnName("DataCriacao").HasColumnType("DATETIME").IsRequired();
        }
    }
}
