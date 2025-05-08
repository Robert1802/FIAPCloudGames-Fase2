using Core.Entity;
using Core.Repository;

namespace Infrastructure.Repository
{
    public class UsuarioRepository : EFREpository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
