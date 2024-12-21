using System.Numerics;

namespace Repositorio.Aplicacao.Dto.Return
{
    public class RtnRepositorio
    {
        // retornando Id para que seja possível a busca individual
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Descricao { get; set; }
        public string? Linguagem { get; set; }
        public string? DonoRepositorio { get; set; }
        public bool Favorito { get; set; }
    }
}
