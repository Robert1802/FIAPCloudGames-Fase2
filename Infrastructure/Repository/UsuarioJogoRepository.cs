using Core.Entity;
using Core.Repository;

namespace Infrastructure.Repository
{
    public class UsuarioJogoRepository : EFREpository<UsuarioJogo>, IUsuarioJogoRepository
    {
        public UsuarioJogoRepository(ApplicationDbContext context) : base(context)
        {
        }
        public UsuarioJogo? ObterPorIdUsuarioIdJogo(int idUsuario, int idJogo)
        {
            return _dbSet.FirstOrDefault(entity => entity.UsuarioId == idUsuario && entity.JogoId == idJogo);
        }

        public List<UsuarioJogo> JogosCompradosPorUsuario(int idUsuario)
        {
            return _dbSet.Where(entity => entity.UsuarioId == idUsuario).ToList();
        }

    }
}
