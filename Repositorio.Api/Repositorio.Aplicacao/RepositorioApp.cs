using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Repositorio.Aplicacao.Dto.Create;
using Repositorio.Aplicacao.Dto.Return;
using Repositorio.Aplicacao.Dto.Update;
using Repositorio.Aplicacao.Interface;
using Repositorio.Aplicacao.SuperClass;
using Repositorio.Dominio;
using Repositorio.Infra;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.Aplicacao
{
    public class RepositorioApp : BaseApp<MdlRepositorio, CrtRepositorio, RtnRepositorio, UpdRepositorio, MdlRepositorio>, IRepositorioApp
    {
        public RepositorioApp(Contexto contexto, IMapper mapper) : base(contexto, mapper)
        {
        }

        public RtnRepositorio? Favoritar(int id)
        {
            var repositorio = ObterPorIdBase(id);
            if (repositorio == null) return null;
            repositorio.Favorito = !repositorio.Favorito;
            _contexto.SaveChanges();
            return new();
        }

        public List<RtnRepositorio> ListarFavoritos(string userName)
        {
            var ret = Buscar(x => x.Usuario!.UserName.ToUpper() == userName.ToUpper() && x.Favorito, x => x.Usuario);
            return _mapper.Map<List<RtnRepositorio>>(ret);
        }

        public List<RtnRepositorio> ListarPorNome(string nome)
        {
            var ret = Buscar(x => x.Usuario!.Nome.ToUpper().Contains(nome.ToUpper()) || x.Nome!.ToUpper().Contains(nome.ToUpper()), x => x.Usuario);
            return _mapper.Map<List<RtnRepositorio>>(ret);
        }

        public List<RtnRepositorio> ListarPorUsername(string userName)
        {
            var ret = Buscar(x => x.Usuario!.UserName.ToUpper() == userName.ToUpper(), x => x.Usuario);
            return _mapper.Map<List<RtnRepositorio>>(ret);
        }

        public List<RtnRepositorio> ListarTodos()
        {
            var ret = Buscar(x => true, x => x.Usuario);
            return _mapper.Map<List<RtnRepositorio>>(ret);
        }

        public override IList<RtnRepositorio> Selecionar(MdlRepositorio search)
        {
            return Buscar(x => (x.Nome == search.Nome || search.Nome is not null)
                               && (x.Descricao == search.Descricao || search.Descricao is not null), x => x.Usuario);
        }

        protected override bool Validar(MdlRepositorio? model)
        {
            if (model is null) return false;
            return true;
        }
    }
}
