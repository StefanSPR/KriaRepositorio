
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositorio.Api.Controllers;
using Repositorio.Dominio;
using Repositorio.Infra;
using Repositorio.Aplicacao.Dto.Create;
using Repositorio.Aplicacao.Dto.Return;
using System.Collections.Generic;
using System.Linq;
using Repositorio.Shared;
using Repositorio.Aplicacao.Dto.Update;
using Repositorio.Aplicacao.Interface;
using Repositorio.Aplicacao;

namespace Repositorio.Teste.Controller
{

    public class RepositorioControllerTests
    {
        private readonly Mock<ILogger<RepositorioController>> _mockLogger;
        private readonly IRepositorioApp _repositorioApp;
        private readonly IMapper _mapper;
        private readonly DbContextOptions<Contexto> _dbContextOptions;
        private readonly Contexto _contexto;
        private readonly RepositorioController _controller;

        public RepositorioControllerTests()
        {
            _mockLogger = new Mock<ILogger<RepositorioController>>();

            _dbContextOptions = new DbContextOptionsBuilder<Contexto>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CrtRepositorio, MdlRepositorio>()
                    .ForMember(dest => dest.DataAtualizacao, opt => opt.MapFrom(src => DateTime.Now));
                cfg.CreateMap<UpdRepositorio, MdlRepositorio>()
                    .ForMember(dest => dest.DataAtualizacao, opt => opt.MapFrom(src => DateTime.Now));
                cfg.CreateMap<MdlRepositorio, RtnRepositorio>()
                    .ForMember(dest => dest.DonoRepositorio, opt => opt.MapFrom(src => (src.Usuario ?? new()).Nome));
                cfg.CreateMap<MdlUsuario, RtnUsuario>();
            });
            _mapper = config.CreateMapper();

            _contexto = new Contexto(_dbContextOptions);
            _repositorioApp = new RepositorioApp(_contexto, _mapper);
            _controller = new RepositorioController(_mockLogger.Object, _repositorioApp);
        }
        #region GET

        [Fact]
        public void Get_DeveRetornarNotFoundParaIdInvalido()
        {
            // Arrange
            // Act
            var result = _controller.Get(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
        [Fact]
        public void MeusRepositorios_DeveRetornarRepositoriosDoUsuario()
        {
            // Arrange
            Utilitario.PreencheDados(_contexto);
            var userName = "joaosilva";

            // Act
            var result = _controller.MeusRepositorios(userName) as OkObjectResult;
            var repositorios = result.Value as List<RtnRepositorio>;

            // Assert
            Assert.NotNull(repositorios);
            Assert.True(repositorios.Count > 0);
        }

        [Fact]
        public void MeusRepositorios_DeveRetornarBadRequestEmCasoDeErro()
        {
            // Arrange
            _contexto.Dispose(); // Forçar erro no banco

            // Act
            var result = _controller.MeusRepositorios("joaosilva");

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void BuscaRepositorio_DeveRetornarRepositoriosPeloNome()
        {
            // Arrange
            Utilitario.PreencheDados(_contexto);
            var nome = "ECommerce";

            // Act
            var result = _controller.BuscaRepositorio(nome) as OkObjectResult;
            var repositorios = result.Value as List<RtnRepositorio>;

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(repositorios);
            Assert.Contains(repositorios, r => r.Nome == "ECommerceAPI");
        }

        [Fact]
        public void BuscaRepositorio_DeveRetornarBadRequestSeNomeForNuloOuVazio()
        {
            // Act
            var result = _controller.BuscaRepositorio(null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public void Favoritar_DeveFavoritarRepositorio()
        {
            // Arrange
            var id = 1;

            // Act
            var result = _controller.Favoritar(id);

            // Assert
            Assert.IsType<NoContentResult>(result);
            var repositorio = _contexto.Repositorios.FirstOrDefault(r => r.Id == id);
            Assert.True(repositorio.Favorito);
        }

        [Fact]
        public void Favoritar_DeveRetornarNotFoundSeRepositorioNaoExistir()
        {
            // Arrange
            var id = 999;

            // Act
            var result = _controller.Favoritar(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void BuscaFavoritos_DeveRetornarRepositoriosFavoritosDoUsuario()
        {
            // Arrange
            Utilitario.PreencheDados(_contexto);
            var userName = "renatoguerreiro";

            // Act
            var result = _controller.BuscaFavoritos(userName) as OkObjectResult;
            var repositorios = result.Value as List<RtnRepositorio>;

            // Assert
            Assert.NotNull(repositorios);
            Assert.Single(repositorios);
            Assert.Equal("SocialNetwork", repositorios[0].Nome);
        }

        [Fact]
        public void BuscaFavoritos_DeveRetornarBadRequestEmCasoDeErro()
        {
            // Arrange
            _contexto.Dispose(); // Forçar erro no banco

            // Act
            var result = _controller.BuscaFavoritos("renatoguerreiro");

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        #endregion
        #region POST
        [Fact]
        public void Post_DeveCriarNovoRepositorio()
        {
            // Arrange
            Utilitario.PreencheUmUsuario(_contexto);
            var dto = new CrtRepositorio
            {
                Nome = "BlogEngine",
                Descricao = "Plataforma para blogs com suporte a markdown",
                Linguagem = "JavaScript",
                IdUsuario = 1
            };

            // Act
            var result = _controller.Post(dto);

            // Assert
            var createdResult = Assert.IsType<CreatedResult>(result);
            Assert.True(createdResult.StatusCode == 201);
        }

        [Theory]
        [InlineData("ProjetoTeste1", "Descrição do Projeto 1", "C#", 1)]
        [InlineData("ProjetoTeste2", "Descrição do Projeto 2", "Python", 2)]
        public void Post_DeveCriarNovosRepositorios(string nome, string descricao, string linguagem, int idUsuario)
        {
            // Arrange
            var dto = new CrtRepositorio
            {
                Nome = nome,
                Descricao = descricao,
                Linguagem = linguagem,
                IdUsuario = idUsuario
            };

            // Act
            var result = _controller.Post(dto) as CreatedResult;

            // Assert
            Assert.NotNull(result);

            var repositorioCriado = result.Value as RtnRepositorio;
            Assert.NotNull(repositorioCriado);
            Assert.Equal(nome, repositorioCriado.Nome);
            Assert.Equal(descricao, repositorioCriado.Descricao);
            Assert.Equal(linguagem, repositorioCriado.Linguagem);
        }
        #endregion
        #region PUT
        [Fact]
        public void Put_DeveAtualizarRepositorioExistente()
        {
            // Arrange
            var id = 1;
            var dto = new UpdRepositorio
            {
                Nome = "Projeto Atualizado",
                Descricao = "Descrição Atualizada",
                IdUsuario = 1,
                Linguagem = ".Net"
            };

            // Act
            var result = _controller.Put(id, dto);

            // Assert
            Assert.IsType<NoContentResult>(result);
            var repositorioAtualizado = _contexto.Repositorios.FirstOrDefault(r => r.Id == id);
            Assert.Equal("Projeto Atualizado", repositorioAtualizado.Nome);
            Assert.Equal("Descrição Atualizada", repositorioAtualizado.Descricao);
        }

        [Fact]
        public void Put_DeveRetornarNotFoundSeRepositorioNaoExistir()
        {
            // Arrange
            var id = 999; // ID inexistente
            var dto = new UpdRepositorio
            {
                Nome = "Projeto Inexistente"
            };

            // Act
            var result = _controller.Put(id, dto);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
        #endregion
        #region DELETE
        [Fact]
        public void Delete_DeveRemoverRepositorio()
        {
            // Arrange
            Utilitario.PreencheUmRepositorio(_contexto);

            // Act
            var result = _controller.Delete(3);

            // Assert
            Assert.IsType<NoContentResult>(result);
            //Assert.Empty(_contexto.Repositorios.ToList());
        }
        #endregion




    }

}