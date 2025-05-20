namespace Core.Entity
{
    public class Promocao : EntityBase
    {
        public required int Id { get; set; }
        public required string Nome { get; set; }
        public required DateTime DataInicio { get; set; }
        public required DateTime DataFim { get; set; }
        public virtual required List<Jogo> Jogos { get; set; }

    }
}
