namespace FIAPCloudGames.Application.DTO.Request
{
    public record JogosPromocoesRequest(
        int JogoId,
        int PromocaoId,
        decimal Desconto
    );
}
