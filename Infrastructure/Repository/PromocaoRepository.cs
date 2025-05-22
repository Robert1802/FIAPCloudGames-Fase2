using Core.Entity;
using Core.Repository;

namespace Infrastructure.Repository
{
    public sealed class PromocaoRepository : EFREpository<Promocao>, IPromocaoRepository
    {
        public PromocaoRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
