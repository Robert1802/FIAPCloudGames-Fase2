using FIAPCloudGames.Domain.Entity;

namespace FIAPCloudGames.Domain.Repository
{
    public interface IJogoRepository : IRepository<Jogo>
    {    
        Jogo? VerificarSeJogoExiste(int id);

        Jogo? VerificarSeJogoExiste(string nome);

        List<Jogo> ObterTodosFiltro(string filtroNome);
    }
}
