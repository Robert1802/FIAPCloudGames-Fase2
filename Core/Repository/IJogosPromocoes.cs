using Core.Entity;

namespace Core.Repository
{
    public interface IJogosPromocoes
    {
        JogosPromocoes? ObterPromocaoAtivaDoJogo(int jogoId, int PromocaoId);
    }
}
