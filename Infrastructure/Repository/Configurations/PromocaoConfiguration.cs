using Core.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Repository.Configurations
{
    public class PromocaoConfiguration : IEntityTypeConfiguration<Promocao>
    {
        public void Configure(EntityTypeBuilder<Promocao> builder)
        {
            builder.ToTable("Promocoes");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnType("INT").UseIdentityColumn();
            builder.Property(p => p.Nome).HasColumnType("VARCHAR(200)").IsRequired();
            builder.Property(p => p.DataInicio).HasColumnName("DataInicio").HasColumnType("DATETIME").IsRequired();
            builder.Property(p => p.DataFim).HasColumnName("DataFim").HasColumnType("DATETIME").IsRequired();
            builder.HasMany(p => p.Jogos).WithOne().HasForeignKey("PromocaoId");
        }
    }
}
