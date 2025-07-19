using FIAPCloudGames.Core.Entity;

namespace FIAPCloudGames.Core.Repository
{
    public interface IJogosPromocoesRepository : IRepository<JogosPromocoes>
    {
        JogosPromocoes? ObterPromocaoAtivaDoJogo(int jogoId, int PromocaoId);

        bool ExistePromocaoAtivaParaOJogo(int jogoId);
    }
}
