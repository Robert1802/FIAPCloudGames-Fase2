using FIAPCloudGames.Domain.Entity;

namespace FIAPCloudGames.Domain.Repository
{
    public interface IJogosPromocoesRepository : IRepository<JogosPromocoes>
    {
        JogosPromocoes? ObterPromocaoAtivaDoJogo(int jogoId, int PromocaoId);

        bool ExistePromocaoAtivaParaOJogo(int jogoId);
    }
}
