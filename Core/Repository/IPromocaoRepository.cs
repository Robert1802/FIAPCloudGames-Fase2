using Core.Entity;

namespace Core.Repository
{
    public interface IPromocaoRepository : IRepository<Promocao>
    {
        Promocao? VerificarSePromocaoExiste(string nome);
    }
}
