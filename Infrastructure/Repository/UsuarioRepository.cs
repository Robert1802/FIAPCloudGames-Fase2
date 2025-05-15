using Core.Entity;
using Core.Repository;

namespace Infrastructure.Repository
{
    public class UsuarioRepository : EFREpository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Usuario ObterPorEmail(string email)
        {
            return _dbSet.FirstOrDefault(entity => entity.Email == email);
        }

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
    }
}
