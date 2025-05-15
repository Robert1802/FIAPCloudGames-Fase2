using Core.Entity;

namespace Core.Repository
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        Usuario ObterPorEmail(string email);
        decimal ConferirSaldo(int id);
        decimal Depositar(int id, decimal valor);
        decimal Subtrair(int id, decimal valor);
    }
}
