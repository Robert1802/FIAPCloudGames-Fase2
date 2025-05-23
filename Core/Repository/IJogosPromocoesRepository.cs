using Core.Entity;

namespace Core.Repository
{
    public interface IJogosPromocoesRepository : IRepository<JogosPromocoes>
    {
        JogosPromocoes? ObterPromocaoAtivaDoJogo(int jogoId, int PromocaoId);
    }
}
