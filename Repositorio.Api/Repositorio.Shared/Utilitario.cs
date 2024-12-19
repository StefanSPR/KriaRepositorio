using Repositorio.Dominio;
using Repositorio.Infra;

namespace Repositorio.Shared
{
    public class Utilitario
    {
        public static void PreencheDados(Contexto context)
        {
            // Verificar se já existem dados
            if (!context.Usuarios.Any())
            {
                context.Usuarios.AddRange(
                    new MdlUsuario
                    {
                        Id = 1,
                        Nome = "João Silva",
                        UserName = "joaosilva"
                    },
                    new MdlUsuario
                    {
                        Id = 2,
                        Nome = "Maria Pereira",
                        UserName = "mariapereira"
                    },
                    new MdlUsuario
                    {
                        Id = 3,
                        Nome = "Renato Guerreiro",
                        UserName = "renatoguerreiro"
                    }
                );
            }

            if (!context.Repositorios.Any())
            {
                context.Repositorios.AddRange(
            new MdlRepositorio
            {
                Id = 1,
                Nome = "ECommerceAPI",
                Descricao = "API para gerenciamento de lojas virtuais",
                Linguagem = "C#",
                DataAtualizacao = DateTime.Parse("2024-12-15T14:23:00"),
                IdUsuario = 1
            },
            new MdlRepositorio
            {
                Id = 2,
                Nome = "BlogEngine",
                Descricao = "Plataforma para blogs com suporte a markdown",
                Linguagem = "JavaScript",
                DataAtualizacao = DateTime.Parse("2024-12-12T10:00:00"),
                IdUsuario = 1
            },
            new MdlRepositorio
            {
                Id = 3,
                Nome = "WeatherApp",
                Descricao = "Aplicativo para previsão do tempo",
                Linguagem = "Python",
                DataAtualizacao = DateTime.Parse("2024-11-25T08:30:00"),
                IdUsuario = 1
            },
            new MdlRepositorio
            {
                Id = 4,
                Nome = "IAChatbot",
                Descricao = "Chatbot baseado em machine learning",
                Linguagem = "Python",
                DataAtualizacao = DateTime.Parse("2024-12-01T17:45:00"),
                IdUsuario = 2
            },
            new MdlRepositorio
            {
                Id = 5,
                Nome = "StockAnalyzer",
                Descricao = "Ferramenta para análise de ações",
                Linguagem = "R",
                DataAtualizacao = DateTime.Parse("2024-11-20T09:15:00"),
                IdUsuario = 2
            },
            new MdlRepositorio
            {
                Id = 6,
                Nome = "PhotoGallery",
                Descricao = "Sistema para gerenciamento de galerias de fotos",
                Linguagem = "PHP",
                DataAtualizacao = DateTime.Parse("2024-12-03T15:00:00"),
                IdUsuario = 2
            },
            new MdlRepositorio
            {
                Id = 7,
                Nome = "TaskManager",
                Descricao = "Aplicativo para organização de tarefas",
                Linguagem = "Java",
                DataAtualizacao = DateTime.Parse("2024-11-18T11:30:00"),
                IdUsuario = 3
            },
            new MdlRepositorio
            {
                Id = 8,
                Nome = "PortfolioSite",
                Descricao = "Site para exibir portfólios pessoais",
                Linguagem = "HTML/CSS/JavaScript",
                DataAtualizacao = DateTime.Parse("2024-12-10T20:00:00"),
                IdUsuario = 3
            },
            new MdlRepositorio
            {
                Id = 9,
                Nome = "GameEngine",
                Descricao = "Motor gráfico para jogos 2D",
                Linguagem = "C++",
                DataAtualizacao = DateTime.Parse("2024-12-08T18:00:00"),
                IdUsuario = 3
            },
            new MdlRepositorio
            {
                Id = 10,
                Nome = "FinanceTracker",
                Descricao = "Aplicação para gerenciamento financeiro",
                Linguagem = "C#",
                DataAtualizacao = DateTime.Parse("2024-11-29T13:15:00"),
                IdUsuario = 1
            },
            new MdlRepositorio
            {
                Id = 11,
                Nome = "RecipeApp",
                Descricao = "Plataforma para compartilhar receitas",
                Linguagem = "Ruby",
                DataAtualizacao = DateTime.Parse("2024-12-05T16:45:00"),
                IdUsuario = 2
            },
            new MdlRepositorio
            {
                Id = 12,
                Nome = "SocialNetwork",
                Descricao = "Rede social para profissionais",
                Linguagem = "TypeScript",
                DataAtualizacao = DateTime.Parse("2024-12-02T10:30:00"),
                Favorito = true,
                IdUsuario = 3
            }
                );
            }

            context.SaveChanges();
        }


        public static void PreencheUmRepositorio(Contexto context)
        {
            PreencheUmUsuario(context);
            if (!context.Repositorios.Any())
            {
                context.Repositorios.Add(new MdlRepositorio
                {
                    Id = 1,
                    Nome = "ECommerceAPI",
                    Descricao = "API para gerenciamento de lojas virtuais",
                    Linguagem = "C#",
                    DataAtualizacao = DateTime.Parse("2024-12-15T14:23:00"),
                    IdUsuario = 1
                });
                context.SaveChanges();
            }

        }
        public static void PreencheUmUsuario(Contexto context)
        {
            if (!context.Usuarios.Any())
            {
                context.Usuarios.Add(new MdlUsuario
                {
                    Id = 1,
                    Nome = "Renato Guerreiro",
                    UserName = "renatoguerreiro"
                });
            }
            context.SaveChanges();
        }
    }
}
