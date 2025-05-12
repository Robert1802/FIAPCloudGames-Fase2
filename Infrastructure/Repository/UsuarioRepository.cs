using Core.Entity;
using Core.Repository;

namespace Infrastructure.Repository
{
    public class UsuarioRepository : EFREpository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Usuario ObterPorEmail(string email)
        {
            return _dbSet.FirstOrDefault(entity => entity.Email == email);
        }
    }
}
