namespace Core.Input
{
    public class UsuarioUpdateInput
    {
        public int Id { get; set; }
        public required string Nome { get; set; }
        public required string Senha { get; set; }
    }
}
