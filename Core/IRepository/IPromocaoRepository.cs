using FIAPCloudGames.Core.Entity;

namespace FIAPCloudGames.Core.Repository
{
    public interface IPromocaoRepository : IRepository<Promocao>
    {
        bool ExistePromocaoComNome(string nome);
    }
}
