using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FIAPCloudGames.Domain.Responses
{
    public record UsuarioResponse
    (
        int UsuarioId,
        string Nome,
        string Email,
        string Senha,
        string NivelAcesso,
        decimal Saldo
    );
}
