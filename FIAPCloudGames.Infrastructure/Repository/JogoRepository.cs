using FIAPCloudGames.Domain.Entity;
using FIAPCloudGames.Domain.Repository;

namespace FIAPCloudGames.Infrastructure.Repository
{
    public sealed class JogoRepository : EFREpository<Jogo>, IJogoRepository
    {
        public JogoRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Jogo? VerificarSeJogoExiste(int id) => 
            _dbSet.FirstOrDefault(entity => entity.Id == id);

        public Jogo? VerificarSeJogoExiste(string nome) =>
            _dbSet.FirstOrDefault(entity => entity.Nome == nome);

        public List<Jogo> ObterTodosFiltro(string filtroNome) =>
            _dbSet.Where(x => x.Nome.Contains(filtroNome) ).ToList();
    }
}
