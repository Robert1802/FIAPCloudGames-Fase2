namespace FIAPCloudGames.Application.DTO.Request
{
    public class JogoRequest
    {
        public string? Nome { get; set; }
        public string? Empresa { get; set; }
        public string? Descricao { get; set; }
        public decimal Preco { get; set; }
    }
}
