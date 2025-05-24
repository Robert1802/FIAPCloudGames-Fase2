using Core.Entity;
using Core.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public sealed class PromocaoRepository : EFREpository<Promocao>, IPromocaoRepository
    {
        public PromocaoRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Promocao? VerificarSePromocaoExiste(string nome) => _dbSet.FirstOrDefault(entity => entity.Nome == nome);
        
    }
}
