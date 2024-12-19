using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Repositorio.Api.Controllers;
using Repositorio.Dominio;
using Repositorio.Infra;
using Repositorio.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.Teste.Dominio
{
    public class UsuarioTests
    {
        private readonly Mock<ILogger<RepositorioController>> _mockLogger;
        private readonly Mock<IMapper> _mockMapper;
        private readonly DbContextOptions<Contexto> _dbContextOptions;
        private readonly Contexto _contexto;

        public UsuarioTests()
        {
            _mockLogger = new Mock<ILogger<RepositorioController>>();
            _mockMapper = new Mock<IMapper>();

            // Configurando banco de dados em memória
            _dbContextOptions = new DbContextOptionsBuilder<Contexto>()
                .UseInMemoryDatabase(databaseName: "UsuarioTestDb")
                .Options;

            _contexto = new Contexto(_dbContextOptions);

            // Populando dados para testes
            Utilitario.PreencheDados(_contexto);
        }

        [Fact]
        public void GetUsuarios_DeveRetornarListaDeUsuarios()
        {
            // Arrange
            var usuarios = _contexto.Usuarios.ToList();

            // Assert
            Assert.Equal(3, usuarios.Count);
            Assert.Contains(usuarios, u => u.UserName == "joaosilva");
            Assert.Contains(usuarios, u => u.UserName == "mariapereira");
            Assert.Contains(usuarios, u => u.UserName == "renatoguerreiro");
        }

        [Fact]
        public void GetRepositoriosDeUsuario_DeveRetornarRepositoriosDoUsuario()
        {
            // Arrange
            Utilitario.PreencheDados(_contexto);
            var usuario = _contexto.Usuarios.Include(u => u.Repositorio).FirstOrDefault(u => u.UserName == "joaosilva");

            // Act
            var repositorios = usuario.Repositorio;

            // Assert
            Assert.NotNull(usuario);
            Assert.Equal(4, repositorios.Count);
            Assert.Contains(repositorios, r => r.Nome == "ECommerceAPI");
            Assert.Contains(repositorios, r => r.Nome == "FinanceTracker");
        }

        [Fact]
        public void AdicionarUsuario_DeveAdicionarUsuarioAoContexto()
        {
            // Arrange
            var novoUsuario = new MdlUsuario
            {
                Id = 4,
                Nome = "Novo Usuario",
                UserName = "novousuario"
            };

            // Act
            _contexto.Usuarios.Add(novoUsuario);
            _contexto.SaveChanges();

            // Assert
            var usuarioAdicionado = _contexto.Usuarios.FirstOrDefault(u => u.UserName == "novousuario");
            Assert.NotNull(usuarioAdicionado);
            Assert.Equal("Novo Usuario", usuarioAdicionado.Nome);
        }

        [Fact]
        public void RemoverUsuario_DeveRemoverUsuarioDoContexto()
        {
            // Arrange
            Utilitario.PreencheDados(_contexto);
            var usuario = _contexto.Usuarios.FirstOrDefault(u => u.UserName == "renatoguerreiro");

            // Act
            _contexto.Usuarios.Remove(usuario);
            _contexto.SaveChanges();

            // Assert
            var usuarioRemovido = _contexto.Usuarios.FirstOrDefault(u => u.UserName == "renatoguerreiro");
            Assert.Null(usuarioRemovido);
        }

        [Fact]
        public void AtualizarUsuario_DeveAtualizarNomeDoUsuario()
        {
            // Arrange
            var usuario = _contexto.Usuarios.FirstOrDefault(u => u.UserName == "joaosilva");

            // Act
            usuario.Nome = "João da Silva";
            _contexto.SaveChanges();

            // Assert
            var usuarioAtualizado = _contexto.Usuarios.FirstOrDefault(u => u.UserName == "joaosilva");
            Assert.NotNull(usuarioAtualizado);
            Assert.Equal("João da Silva", usuarioAtualizado.Nome);
        }

        [Fact]
        public void VerificarRelacionamento_RepositoriosDoUsuario()
        {
            // Arrange
            Utilitario.PreencheDados(_contexto);
            var usuario = _contexto.Usuarios.Include(u => u.Repositorio).FirstOrDefault(u => u.UserName == "mariapereira");

            // Assert
            Assert.NotNull(usuario);
            Assert.NotNull(usuario.Repositorio);
            Assert.Equal(4, usuario.Repositorio.Count);
            Assert.Contains(usuario.Repositorio, r => r.Nome == "IAChatbot");
        }
    }

}
