using Core.Entity;

namespace Core.Repository
{
    public interface IPromocaoRepository : IRepository<Promocao>
    {
        Promocao VerificaSePromocaoExiste(string nome);
    }
}
