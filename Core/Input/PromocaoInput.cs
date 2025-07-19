namespace FIAPCloudGames.Core.Input
{
    public class PromocaoInput
    {
        public string Nome { get; set; } = null!;
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public bool Ativo { get; set; }
    }
}
