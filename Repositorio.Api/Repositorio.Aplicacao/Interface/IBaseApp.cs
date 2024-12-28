namespace Repositorio.Aplicacao.Interface;

public interface IBaseApp<TContext, TCrt, TReturn, TUpd, TSearch> where TContext : class
{
    TReturn? ObterPorId(int id);
    IList<TReturn> Selecionar(TSearch search);
    void Apagar(int id);
    TReturn Atualizar(int id, TUpd upd);
    TReturn Inserir(TCrt create);
}
