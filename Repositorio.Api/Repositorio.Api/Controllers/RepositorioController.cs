using Microsoft.AspNetCore.Mvc;
using Repositorio.Aplicacao.Dto.Create;
using Repositorio.Aplicacao.Dto.Return;
using Repositorio.Aplicacao.Interface;

namespace Repositorio.Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class RepositorioController : ControllerBase
    {

        private readonly ILogger<RepositorioController> _logger;
        private readonly IRepositorioApp _repositorioApp;

        public RepositorioController(ILogger<RepositorioController> logger, IRepositorioApp repositorioApp)
        {
            _logger = logger;
            _repositorioApp = repositorioApp;
        }
        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation("Executando método GET para listar repositórios.");
            try
            {
                List<RtnRepositorio> ret = _repositorioApp.ListarTodos();
                return Ok(ret);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar repositórios.");
                return BadRequest(new { Message = "Erro interno do servidor." });
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            _logger.LogInformation("Executando método GET para buscar repositório com ID {Id}.", id);
            try
            {
                RtnRepositorio ret = _repositorioApp.ObterPorId(id);

                if (ret == null)
                {
                    _logger.LogWarning("Repositório com ID {Id} não encontrado.", id);
                    return NotFound();
                }

                return Ok(ret);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar repositório com ID {Id}.", id);
                return BadRequest(new { Message = "Erro interno do servidor." });
            }
        }

        [HttpPost]
        public IActionResult Post(CrtRepositorio dto)
        {
            _logger.LogInformation("Executando método POST para criar um novo repositório.");
            try
            {
                var ret = _repositorioApp.Salvar(dto);
                return Created("", ret);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar um novo repositório.");
                return BadRequest(new { Message = "Erro interno do servidor." });
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] CrtRepositorio dto)
        {
            _logger.LogInformation("Executando método PUT para atualizar repositório com ID {Id}.", id);
            try
            {
                RtnRepositorio? ret = _repositorioApp.Atuailizar(id, dto);
                if (ret == null)
                {
                    _logger.LogWarning("Repositório com ID {Id} não encontrado para atualização.", id);
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar repositório com ID {Id}.", id);
                return BadRequest(new { Message = "Erro interno do servidor." });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _logger.LogInformation("Executando método DELETE para remover repositório com ID {Id}.", id);
            try
            {

                var repositorio = _repositorioApp.Apagar(id);
                if (repositorio == null)
                {
                    _logger.LogWarning("Repositório com ID {Id} não encontrado para exclusão.", id);
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir repositório com ID {Id}.", id);
                return BadRequest(new { Message = "Erro interno do servidor." });
            }
        }

        [HttpGet("MeusRepositorios")]
        public IActionResult MeusRepositorios([FromQuery] string userName)
        {
            _logger.LogInformation("Executando método GET para listar repositórios do usuário {UserName}.", userName);
            try
            {
                List<RtnRepositorio> ret = _repositorioApp.ListarPorUsername(userName);


                return Ok(ret);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar repositórios do usuário {UserName}.", userName);
                return BadRequest(new { Message = "Erro interno do servidor." });
            }
        }

        [HttpGet("BuscaRepositorio")]
        public IActionResult BuscaRepositorio([FromQuery] string nome)
        {
            _logger.LogInformation("Executando método GET para buscar repositórios com o nome {Nome}.", nome);
            try
            {
                if (string.IsNullOrEmpty(nome))
                {
                    _logger.LogWarning("Nome não fornecido para busca.");
                    return BadRequest("Nome é obrigatório.");
                }
                List<RtnRepositorio> ret = _repositorioApp.ListarPorNome(nome);


                return Ok(ret);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar repositórios com o nome {Nome}.", nome);
                return BadRequest(new { Message = "Erro interno do servidor." });
            }
        }

        [HttpPut("Favoritar/{id}")]
        public IActionResult Favoritar(int id)
        {
            _logger.LogInformation("Executando método PUT para favoritar repositório com ID {Id}.", id);
            try
            {
                RtnRepositorio? ret = _repositorioApp.Favoritar(id);
                if (ret == null)
                {
                    _logger.LogWarning("Repositório com ID {Id} não encontrado para favoritar.", id);
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao favoritar repositório com ID {Id}.", id);
                return BadRequest(new { Message = "Erro interno do servidor." });
            }
        }

        [HttpGet("BuscaFavoritos")]
        public IActionResult BuscaFavoritos([FromQuery] string userName)
        {
            _logger.LogInformation("Executando método GET para listar repositórios favoritos do usuário {UserName}.", userName);
            try
            {
                List<RtnRepositorio> ret = _repositorioApp.ListarFavoritos(userName);

                return Ok(ret);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar repositórios favoritos do usuário {UserName}.", userName);
                return BadRequest(new { Message = "Erro interno do servidor." });
            }
        }
    }
}

