using FIAPCloudGames.Domain.Entity;

namespace FIAPCloudGames.Domain.Repository
{
    public interface IPromocaoRepository : IRepository<Promocao>
    {
        bool ExistePromocaoComNome(string nome);
    }
}
