using Core.Entity;
using Core.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public sealed class JogosPromocoesRepository : EFREpository<JogosPromocoes>, IJogosPromocoesRepository
    {
        public JogosPromocoesRepository(ApplicationDbContext context) : base(context)
        {
        }

        public JogosPromocoes? ObterPromocaoAtivaDoJogo(int jogoId, int PromocaoId)
        {
            var dataAtual = DateTime.Now;

            return _dbSet
                .Include(x => x.Promocao)
                .FirstOrDefault(x =>
                    x.JogoId == jogoId &&
                    x.Promocao.Ativo &&
                    x.Promocao.DataInicio <= dataAtual &&
                    x.Promocao.DataFim >= dataAtual &&
                    (PromocaoId == 0 || x.Promocao.Id == PromocaoId)
                );
        }
    }
}
