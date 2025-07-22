namespace FIAPCloudGames.Application.DTO.Request
{
    public record JogoUpdateRequest(
        int Id,
        string? Nome,
        string? Empresa,
        string? Descricao,
        decimal Preco
    );
}
