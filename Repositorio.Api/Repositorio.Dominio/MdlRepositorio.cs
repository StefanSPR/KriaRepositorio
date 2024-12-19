namespace Repositorio.Dominio
{
    public class MdlRepositorio
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Descricao { get; set; }
        public string? Linguagem { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public bool Favorito { get; set; }
        public int IdUsuario { get; set; }
        public MdlUsuario? Usuario { get; set; }

    }
}
