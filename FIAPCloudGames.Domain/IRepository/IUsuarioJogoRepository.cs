using FIAPCloudGames.Domain.Entity;

namespace FIAPCloudGames.Domain.Repository
{
    public interface IUsuarioJogoRepository : IRepository<UsuarioJogo>
    {
        UsuarioJogo? ObterPorIdUsuarioIdJogo(int idUsuario, int idJogo);

        List<UsuarioJogo> JogosCompradosPorUsuario(int idUsuario);
    }
}
