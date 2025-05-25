using Core.Entity;

namespace Core.Repository
{
    public interface IPromocaoRepository : IRepository<Promocao>
    {
        bool ExistePromocaoComNome(string nome);
    }
}
