using Core.Entity;
using Core.Repository;

namespace Infrastructure.Repository
{
    public class UsuarioJogoRepository : EFREpository<UsuarioJogo>, IUsuarioJogoRepository
    {
        public UsuarioJogoRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
