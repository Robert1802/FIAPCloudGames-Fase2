using Core.Entity;

namespace Core.Repository
{
    public interface IUsuarioJogoRepository : IRepository<UsuarioJogo>
    {
        UsuarioJogo ObterPorIdUsuario(int idUsuario, int idJogo);

        List<UsuarioJogo> JogosCompradosPorUsuario(int idUsuario);
    }
}
