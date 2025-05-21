using Core.Entity;
using Core.Repository;

namespace Infrastructure.Repository
{
    public class PromocaoRepository : EFREpository<Promocao>, IPromocaoRepository
    {
        public PromocaoRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Promocao VerificaSePromocaoExiste(string nome) => _dbSet.FirstOrDefault(entity => entity.Nome == nome);

    }
}
