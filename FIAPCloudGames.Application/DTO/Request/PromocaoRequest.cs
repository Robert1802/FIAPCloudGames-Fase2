namespace FIAPCloudGames.Application.DTO.Request
{
    public class PromocaoRequest
    {
        public string Nome { get; set; } = null!;
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public bool Ativo { get; set; }
    }
}
