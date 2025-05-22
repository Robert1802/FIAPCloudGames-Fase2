using Core.Entity;

namespace Core.Repository
{
    public interface IJogoRepository : IRepository<Jogo>
    {    
        Jogo? VerificarSeJogoExiste(int id);

        Jogo? VerificarSeJogoExiste(string nome);

        List<Jogo> ObterTodosFiltro(string filtroNome);
    }
}
