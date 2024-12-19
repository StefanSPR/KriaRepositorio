using Microsoft.EntityFrameworkCore;
using Repositorio.Dominio;
using Repositorio.Infra.Configurations;

namespace Repositorio.Infra
{
    public class Contexto : DbContext
    {
        public DbSet<MdlUsuario> Usuarios { get; set; }
        public DbSet<MdlRepositorio> Repositorios { get; set; }
        public Contexto(DbContextOptions<Contexto> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new RepositorioConfiguration());
            modelBuilder.ApplyConfiguration(new UsuarioConfiguration());
        }
    }
}
