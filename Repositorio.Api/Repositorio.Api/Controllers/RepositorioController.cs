
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositorio.Aplicacao.Dto.Create;
using Repositorio.Aplicacao.Dto.Return;
using Repositorio.Dominio;
using Repositorio.Infra;

namespace Repositorio.Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class RepositorioController : ControllerBase
    {

        private readonly ILogger<RepositorioController> _logger;
        private readonly Contexto _contexto;
        private IMapper _mapper;

        public RepositorioController(ILogger<RepositorioController> logger, Contexto contexto, IMapper mapper)
        {
            _logger = logger;
            _contexto = contexto;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation("Executando método GET para listar repositórios.");
            try
            {
                var repositorios = _contexto.Repositorios
                    .Include(x => x.Usuario)
                    .OrderBy(x => x.Id)
                    .ToList();
                return Ok(_mapper.Map<List<RtnRepositorio>>(repositorios));
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
                var repositorio = _contexto.Repositorios
                    .Include(x => x.Usuario)
                    .FirstOrDefault(x => x.Id == id);

                if (repositorio == null)
                {
                    _logger.LogWarning("Repositório com ID {Id} não encontrado.", id);
                    return NotFound();
                }

                return Ok(_mapper.Map<RtnRepositorio>(repositorio));
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
                var repositorio = _mapper.Map<MdlRepositorio>(dto);
                _contexto.Repositorios.Add(repositorio);
                _contexto.SaveChanges();
                return Created("", _mapper.Map<RtnRepositorio>(repositorio));
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
                var repositorio = _contexto.Repositorios.FirstOrDefault(x => x.Id == id);
                if (repositorio == null)
                {
                    _logger.LogWarning("Repositório com ID {Id} não encontrado para atualização.", id);
                    return NotFound();
                }

                _mapper.Map(dto, repositorio);
                _contexto.SaveChanges();
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
                var repositorio = _contexto.Repositorios.FirstOrDefault(a => a.Id == id);
                if (repositorio == null)
                {
                    _logger.LogWarning("Repositório com ID {Id} não encontrado para exclusão.", id);
                    return NotFound();
                }

                _contexto.Repositorios.Remove(repositorio);
                _contexto.SaveChanges();
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
                var repositorios = _contexto.Repositorios
                    .Include(x => x.Usuario)
                    .Where(x => x.Usuario!.UserName.ToUpper() == userName.ToUpper())
                    .ToList();

                return Ok(_mapper.Map<List<RtnRepositorio>>(repositorios));
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

                var repositorios = _contexto.Repositorios
                    .Include(x => x.Usuario)
                    .Where(x => x.Usuario!.Nome.ToUpper().Contains(nome.ToUpper()) || x.Nome!.ToUpper().Contains(nome.ToUpper()))
                    .ToList();

                return Ok(_mapper.Map<List<RtnRepositorio>>(repositorios));
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
                var repositorio = _contexto.Repositorios.Include(x => x.Usuario).FirstOrDefault(x => x.Id == id);
                if (repositorio == null)
                {
                    _logger.LogWarning("Repositório com ID {Id} não encontrado para favoritar.", id);
                    return NotFound();
                }

                repositorio.Favorito = true;
                _contexto.SaveChanges();
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
                var repositorios = _contexto.Repositorios
                    .Include(x => x.Usuario)
                    .Where(x => x.Usuario!.UserName.ToUpper() == userName.ToUpper() && x.Favorito)
                    .ToList();

                return Ok(_mapper.Map<List<RtnRepositorio>>(repositorios));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar repositórios favoritos do usuário {UserName}.", userName);
                return BadRequest(new { Message = "Erro interno do servidor." });
            }
        }
    }
}

