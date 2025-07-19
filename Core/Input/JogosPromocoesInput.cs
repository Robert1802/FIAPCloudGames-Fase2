namespace FIAPCloudGames.Core.Input
{
    public record JogosPromocoesInput(
        int JogoId,
        int PromocaoId,
        decimal Desconto
    );
}
