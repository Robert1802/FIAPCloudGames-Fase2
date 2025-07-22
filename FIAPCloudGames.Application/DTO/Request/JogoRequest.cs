namespace FIAPCloudGames.Application.DTO.Request
{
    public record JogoRequest(
        string? Nome,
        string? Empresa,
        string? Descricao,
        decimal Preco
    );
}
