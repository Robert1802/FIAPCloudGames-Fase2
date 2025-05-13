namespace Core.Entity
{
    public class UsuarioJogo : EntityBase
    {
        public int IdUsuario { get; set; }
        public int IdJogo { get; set; }
        public decimal PrecoDaCompra { get; set; }
    }
}
