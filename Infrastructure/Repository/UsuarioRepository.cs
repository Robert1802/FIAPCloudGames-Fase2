using Core.Entity;
using Core.Repository;
using Core.Utils;

namespace Infrastructure.Repository;

public class UsuarioRepository(ApplicationDbContext context) : EFREpository<Usuario>(context), IUsuarioRepository
{
    public Usuario? ObterPorEmail(string email) =>
        _dbSet.FirstOrDefault(entity => entity.Email == email);

    public Usuario? ObterPorEmailESenha(string email, string senha) =>
        _dbSet.FirstOrDefault(entity => entity.Email == email &&
            entity.Senha == senha);

    public decimal ConferirSaldo(int id)
    {
        Usuario usuario = _dbSet.FirstOrDefault(entity => entity.Id == id);
        return (decimal)usuario.Saldo;
    }

    public decimal Depositar(int id, decimal valor)
    {
        Usuario usuario = ObterPorId(id);
        usuario.Saldo = usuario.Saldo + valor;
        _dbSet.Update(usuario);
        _context.SaveChanges();
        return (decimal)usuario.Saldo;
    }

    public decimal Subtrair(int id, decimal valor)
    {
        Usuario usuario = ObterPorId(id);
        usuario.Saldo = usuario.Saldo - valor;
        _dbSet.Update(usuario);
        _context.SaveChanges();
        return (decimal)usuario.Saldo;
    }

    public Usuario? Logar(string email, string senhaTexto)
    {
        var usuario = _context.Set<Usuario>().FirstOrDefault(u => u.Email == email);

        if (usuario == null)
            return null;

        var senhaValida = PasswordHelper.VerificarSenha(senhaTexto, usuario.Senha!);

        return senhaValida ? usuario : null;
    }
}