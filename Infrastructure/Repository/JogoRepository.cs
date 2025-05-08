using Core.Entity;
using Core.Repository;

namespace Infrastructure.Repository
{
    public class JogoRepository : EFREpository<Jogo>, IJogoRepository
    {
        public JogoRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
