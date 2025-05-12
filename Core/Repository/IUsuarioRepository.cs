using Core.Entity;

namespace Core.Repository
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        Usuario ObterPorEmail(string email);
    }
}
