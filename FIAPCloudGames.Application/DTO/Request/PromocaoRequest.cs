namespace FIAPCloudGames.Application.DTO.Request
{
    public record PromocaoRequest(
        string Nome,
        DateTime DataInicio,
        DateTime DataFim,
        bool Ativo
    );
}
