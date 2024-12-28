using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Repositorio.Aplicacao.Interface;
using Repositorio.Infra;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;

namespace Repositorio.Aplicacao.SuperClass
{
    public abstract class BaseApp<TContext, TCrt, TReturn, TUpd, TSearch> : IBaseApp<TContext, TCrt,TReturn, TUpd, TSearch> where TContext : class
    {
        protected readonly Contexto _contexto;
        protected readonly IMapper _mapper;
        protected BaseApp(Contexto contexto, IMapper mapper)
        {
            _contexto = contexto;
            _mapper = mapper;
        }
        #region Public
        #region Virtual
        public virtual void Apagar(int id)
        {
            var model = ObterPorIdBase(id);
            if (model == null) throw new ObjectNotFoundException($"Id não encontrado: {id}");
            _contexto.Set<TContext>().Remove(model);
            _contexto.SaveChanges();
        }
        public virtual TReturn Inserir(TCrt create)
        {
            var model = _mapper.Map<TContext>(create);
            if (!Validar(model)) throw new ArgumentException("Objeto inválido");
            _contexto.Set<TContext>().Add(model);
            _contexto.SaveChanges();
            return _mapper.Map<TReturn>(model);
        }
        public virtual TReturn? ObterPorId(int id)
        {
            return _mapper.Map<TReturn>(ObterPorIdBase(id));
        }
        public virtual TReturn Atualizar(int id, TUpd upd)
        {
            var model = ObterPorIdBase(id);
            if (model == null) throw new ObjectNotFoundException($"Id não encotrado: {id}");
            if (!Validar(model)) throw new ArgumentException("Objeto inválido");
            _mapper.Map(upd, model);
            _contexto.SaveChanges();
            return _mapper.Map<TReturn>(model);
        }
        #endregion
        #region Abstract
        public abstract IList<TReturn> Selecionar(TSearch search);
        #endregion
        #endregion
        #region Protected
        protected virtual IList<TReturn> Buscar(Func<TContext, bool> func, params Expression<Func<TContext, object>>[] includes)
        {
            var dbQuery = _contexto.Set<TContext>().AsQueryable();
            DbSet<TContext> dbSet = _contexto.Set<TContext>();
            if(includes is not null && includes.Any())
            {
                foreach (var include in includes.ToList())
                {
                    dbQuery = dbSet.Include(include);
                }
            }
            return _mapper.Map<IList<TReturn>>(dbQuery.Where(func).ToList());
        }
        protected virtual TContext? ObterPorIdBase(int id)
        {
            return _contexto.Set<TContext>().Find(id);
        }
        protected abstract bool Validar(TContext? mopdel);
        #endregion
    }
}
