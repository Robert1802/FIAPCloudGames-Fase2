using Core.Entity;

namespace Core.Repository
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        Usuario? ObterPorEmail(string email);
        Usuario? ObterPorEmailESenha(string email, string senha);
        Usuario? Logar(string email, string senhaTexto);
        decimal ConferirSaldo(int id);
        decimal Depositar(int id, decimal valor);
        decimal Subtrair(int id, decimal valor);        
    }
}
