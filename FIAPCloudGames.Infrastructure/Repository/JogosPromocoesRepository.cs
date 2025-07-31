using FIAPCloudGames.Domain.Entity;
using FIAPCloudGames.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace FIAPCloudGames.Infrastructure.Repository
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
                .AsNoTracking()
                .FirstOrDefault(x =>
                    x.JogoId == jogoId &&
                    x.Promocao!.Ativo &&
                    x.Promocao.DataInicio <= dataAtual &&
                    x.Promocao.DataFim >= dataAtual &&
                    (PromocaoId == 0 || x.Promocao.Id == PromocaoId)
                );
        }

        public bool ExistePromocaoAtivaParaOJogo(int jogoId)
        {
            var dataAtual = DateTime.Now;

            return _dbSet
                .Include(x => x.Promocao)
                .AsNoTracking()
                .Any(x => x.JogoId == jogoId &&
                x.Promocao!.Ativo == true &&
                x.Promocao.DataInicio >= dataAtual &&
                x.Promocao.DataFim <= dataAtual);
        }

    }
}
