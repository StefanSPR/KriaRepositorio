namespace Repositorio.Dominio
{
    public class MdlUsuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string UserName { get; set; }
        public IList<MdlRepositorio> Repositorio { get; set; }
    }
}
