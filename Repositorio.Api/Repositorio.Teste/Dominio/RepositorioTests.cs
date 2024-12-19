using Microsoft.EntityFrameworkCore;
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
    public class RepositorioTests
    {
        private readonly DbContextOptions<Contexto> _dbContextOptions;
        private readonly Contexto _contexto;

        public RepositorioTests()
        {
            // Configurando banco de dados em memória
            _dbContextOptions = new DbContextOptionsBuilder<Contexto>()
                .UseInMemoryDatabase(databaseName: "RepositorioTestDb")
                .Options;

            _contexto = new Contexto(_dbContextOptions);

            // Populando dados para testes
            Utilitario.PreencheDados(_contexto);
        }

        [Fact]
        public void AdicionarRepositorio_DeveAdicionarRepositorioAoContexto()
        {
            // Arrange
            var novoRepositorio = new MdlRepositorio
            {
                Id = 13,
                Nome = "NewProject",
                Descricao = "Novo projeto para testes",
                Linguagem = "Go",
                DataAtualizacao = DateTime.UtcNow,
                IdUsuario = 1
            };

            // Act
            _contexto.Repositorios.Add(novoRepositorio);
            _contexto.SaveChanges();

            // Assert
            var repositorioAdicionado = _contexto.Repositorios.FirstOrDefault(r => r.Id == 13);
            Assert.NotNull(repositorioAdicionado);
            Assert.Equal("NewProject", repositorioAdicionado.Nome);
        }

        [Fact]
        public void AtualizarRepositorio_DeveAtualizarDescricaoDoRepositorio()
        {
            // Arrange
            var repositorio = _contexto.Repositorios.FirstOrDefault(r => r.Id == 1);

            // Act
            repositorio.Descricao = "Descrição atualizada para testes";
            _contexto.SaveChanges();

            // Assert
            var repositorioAtualizado = _contexto.Repositorios.FirstOrDefault(r => r.Id == 1);
            Assert.NotNull(repositorioAtualizado);
            Assert.Equal("Descrição atualizada para testes", repositorioAtualizado.Descricao);
        }

        [Fact]
        public void DeletarRepositorio_DeveRemoverRepositorioDoContexto()
        {
            // Arrange
            var repositorio = _contexto.Repositorios.FirstOrDefault(r => r.Id == 2);

            // Act
            _contexto.Repositorios.Remove(repositorio);
            _contexto.SaveChanges();

            // Assert
            var repositorioRemovido = _contexto.Repositorios.FirstOrDefault(r => r.Id == 2);
            Assert.Null(repositorioRemovido);
        }

        [Fact]
        public void ConsultarRepositorio_PorLinguagem()
        {
            // Arrange
            string linguagem = "Python";

            // Act
            var repositorios = _contexto.Repositorios.Where(r => r.Linguagem == linguagem).ToList();

            // Assert
            Assert.NotEmpty(repositorios);
            Assert.Equal(2, repositorios.Count);
            Assert.Contains(repositorios, r => r.Nome == "WeatherApp");
            Assert.Contains(repositorios, r => r.Nome == "IAChatbot");
        }

        [Fact]
        public void ConsultarFavoritos_DeveRetornarRepositoriosFavoritos()
        {
            // Act
            var favoritos = _contexto.Repositorios.Where(r => r.Favorito).ToList();

            // Assert
            Assert.Single(favoritos);
            Assert.Contains(favoritos, r => r.Nome == "SocialNetwork");
        }

        [Fact]
        public void VerificarRelacionamento_RepositorioUsuario()
        {
            // Arrange
            var repositorio = _contexto.Repositorios.Include(r => r.Usuario).FirstOrDefault(r => r.Id == 1);

            // Assert
            Assert.NotNull(repositorio);
            Assert.NotNull(repositorio.Usuario);
            Assert.Equal("João Silva", repositorio.Usuario.Nome);
        }
    }
}
