namespace Core.Entity
{
    public class Jogo : EntityBase
    {
        public string? Nome { get; set; }
        public string? Empresa {  get; set; }
        public string? Descricao { get; set; }
        public decimal Preco {  get; set; }
        public decimal Desconto { get; set; }
        public int? PromocaoId { get; set; }
    }
}
