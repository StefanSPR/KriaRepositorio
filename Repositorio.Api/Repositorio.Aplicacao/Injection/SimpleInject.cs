using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repositorio.Aplicacao.Interface;
using Repositorio.Infra;
using Repositorio.Shared;

namespace Repositorio.Aplicacao.Injection
{
    public class SimpleInject
    {
        public static void InsereDadosMocados(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<Contexto>();
                Utilitario.PreencheDados(dbContext);
            }
        }
        public static void InitializeInjections(IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddDbContext<Contexto>(options => options.UseInMemoryDatabase("InMemoryDb"));

            InjecaoApp(services);
        }

        private static void InjecaoApp(IServiceCollection services)
        {
            services.AddScoped<IRepositorioApp, RepositorioApp>();
        }
    }
}
