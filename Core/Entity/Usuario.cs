namespace Core.Entity
{
    public class Usuario : EntityBase
    {
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public string? Senha { get; set; }
        public string? NivelAcesso { get; set; }
        public decimal? Saldo { get; set; }
    }
}
