using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Repositorio.Aplicacao.Dto.Create;
using Repositorio.Aplicacao.Dto.Return;
using Repositorio.Aplicacao.Interface;
using Repositorio.Dominio;
using Repositorio.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.Aplicacao
{
    public class RepositorioApp : IRepositorioApp
    {
        private readonly Contexto _contexto;
        private IMapper _mapper;

        public RepositorioApp(Contexto contexto, IMapper mapper)
        {
            _contexto = contexto;
            _mapper = mapper;
        }

        public RtnRepositorio? Apagar(int id)
        {
            var rep = _contexto.Repositorios.FirstOrDefault(a => a.Id == id);
            if (rep == null) return null;

            _contexto.Repositorios.Remove(rep);
            _contexto.SaveChanges();

            return _mapper.Map<RtnRepositorio>(rep);
        }

        public RtnRepositorio? Atuailizar(int id, CrtRepositorio dto)
        {
            var repositorio = _contexto.Repositorios.FirstOrDefault(x => x.Id == id);
            if (repositorio == null) return null;

            _mapper.Map(dto, repositorio);
            _contexto.SaveChanges();
            return _mapper.Map<RtnRepositorio>(repositorio);
        }

        public RtnRepositorio? Favoritar(int id)
        {
            var repositorio = _contexto.Repositorios.Include(x => x.Usuario).FirstOrDefault(x => x.Id == id);
            if (repositorio == null) return null ;
            repositorio.Favorito = true;
            _contexto.SaveChanges();
            return new();
        }

        public List<RtnRepositorio> ListarFavoritos(string userName)
        {
            var repositorios = _contexto.Repositorios
                .Include(x => x.Usuario)
                .Where(x => x.Usuario!.UserName.ToUpper() == userName.ToUpper() && x.Favorito)
                .ToList();
            return _mapper.Map<List<RtnRepositorio>>(repositorios);
        }

        public List<RtnRepositorio> ListarPorNome(string nome)
        {

            var lst = _contexto.Repositorios
                .Include(x => x.Usuario)
                .Where(x => x.Usuario!.Nome.ToUpper().Contains(nome.ToUpper()) || x.Nome!.ToUpper().Contains(nome.ToUpper()))
                .ToList();
            return _mapper.Map<List<RtnRepositorio>>(lst);
        }

        public List<RtnRepositorio> ListarPorUsername(string userName)
        {
            var lst = _contexto.Repositorios
                     .Include(x => x.Usuario)
                     .Where(x => x.Usuario!.UserName.ToUpper() == userName.ToUpper())
                     .ToList();
            return _mapper.Map<List<RtnRepositorio>>(lst);
        }

        public List<RtnRepositorio> ListarTodos()
        {
            var repositorios = _contexto.Repositorios
                .Include(x => x.Usuario)
                .OrderBy(x => x.Id)
                .ToList();
            return _mapper.Map<List<RtnRepositorio>>(repositorios);
        }

        public RtnRepositorio ObterPorId(int id)
        {
            var repositorio = _contexto.Repositorios
                    .Include(x => x.Usuario)
                    .FirstOrDefault(x => x.Id == id);
            return _mapper.Map<RtnRepositorio>(repositorio);
        }

        public RtnRepositorio Salvar(CrtRepositorio dto)
        {
            if (!Validar(dto)) throw new Exception("Objeto Inválido.");
            var repositorio = _mapper.Map<MdlRepositorio>(dto);
            _contexto.Repositorios.Add(repositorio);
            _contexto.SaveChanges();
            return _mapper.Map<RtnRepositorio>(repositorio);
        }

        private bool Validar(CrtRepositorio dto)
        {
            return true;
        }
    }
}
