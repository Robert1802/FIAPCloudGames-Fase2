namespace Core.Input
{
    public class JogoDto
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Empresa { get; set; }
        public string? Descricao { get; set; }
        public decimal Preco { get; set; }
        public decimal Desconto { get; set; }
        public DateTime DataCriacao { get; set; }
        public int? PromocaoId { get; set; }
    }
}
