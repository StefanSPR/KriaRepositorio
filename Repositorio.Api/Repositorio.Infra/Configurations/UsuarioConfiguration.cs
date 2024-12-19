using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repositorio.Dominio;

namespace Repositorio.Infra.Configurations
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<MdlUsuario>
    {
        public void Configure(EntityTypeBuilder<MdlUsuario> builder)
        {
            builder.ToTable("Usuario");

            // Configurando a chave primária
            builder.HasKey(u => u.Id);

            // Configuração das propriedades
            builder.Property(u => u.Nome)
                .HasMaxLength(250) 
                .IsRequired(); 

            builder.Property(u => u.UserName)
                .HasMaxLength(100) 
                .IsRequired();

            builder.HasMany(u => u.Repositorio)
                .WithOne(r => r.Usuario)
                .HasForeignKey(r => r.IdUsuario);
        }
    }
}
