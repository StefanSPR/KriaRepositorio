using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repositorio.Dominio;

namespace Repositorio.Infra.Configurations
{
    public class RepositorioConfiguration : IEntityTypeConfiguration<MdlRepositorio>
    {
        public void Configure(EntityTypeBuilder<MdlRepositorio> builder)
        {

            builder
                .ToTable("Repositorio");

            builder.HasKey(r => r.Id);

            // Configuração das propriedades
            builder.Property(r => r.Nome)
                .HasMaxLength(255) 
                .IsRequired(); 

            builder.Property(r => r.Descricao)
                .HasMaxLength(500) 
                .IsRequired(); 

            builder.Property(r => r.Linguagem)
                .HasMaxLength(100) 
                .IsRequired(); 

            builder.Property(r => r.DataAtualizacao)
                .IsRequired();

            builder.Property(r => r.IdUsuario)
                .IsRequired();

            builder.Property(r => r.Favorito)
                .HasDefaultValue(false)
                .IsRequired();

            builder.HasOne(r => r.Usuario)
                .WithMany(u => u.Repositorio) 
                .HasForeignKey(r => r.IdUsuario)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
