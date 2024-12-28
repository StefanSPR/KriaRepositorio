using Repositorio.Aplicacao.Dto.Create;
using Repositorio.Aplicacao.Dto.Return;
using Repositorio.Aplicacao.Dto.Update;
using Repositorio.Aplicacao.SuperClass;
using Repositorio.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.Aplicacao.Interface
{
    public interface IRepositorioApp : IBaseApp<MdlRepositorio, CrtRepositorio, RtnRepositorio, UpdRepositorio, MdlRepositorio>
    {
        //RtnRepositorio? Apagar(int id);
        //RtnRepositorio? Atuailizar(int id, CrtRepositorio dto);
        //RtnRepositorio ObterPorId(int id);
        //RtnRepositorio Salvar(CrtRepositorio dto);
        RtnRepositorio? Favoritar(int id);
        List<RtnRepositorio> ListarFavoritos(string userName);
        List<RtnRepositorio> ListarPorNome(string nome);
        List<RtnRepositorio> ListarPorUsername(string userName);
        List<RtnRepositorio> ListarTodos();
    }
}
