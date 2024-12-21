using Repositorio.Aplicacao.Dto.Create;
using Repositorio.Aplicacao.Dto.Return;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.Aplicacao.Interface
{
    public interface IRepositorioApp
    {
        RtnRepositorio? Apagar(int id);
        RtnRepositorio? Atuailizar(int id, CrtRepositorio dto);
        RtnRepositorio? Favoritar(int id);
        List<RtnRepositorio> ListarFavoritos(string userName);
        List<RtnRepositorio> ListarPorNome(string nome);
        List<RtnRepositorio> ListarPorUsername(string userName);
        List<RtnRepositorio> ListarTodos();
        RtnRepositorio ObterPorId(int id);
        RtnRepositorio Salvar(CrtRepositorio dto);
    }
}
