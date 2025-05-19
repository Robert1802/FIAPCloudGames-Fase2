using System.Security.Claims;
using Core.Entity;

namespace FIAPCloudGamesApi.Helpers;

public static class UsuarioLogadoHelper
{
    public static Usuario? ObterUsuarioLogado(ClaimsPrincipal user)
    {
        if (UsuarioNaoEstaAutenticado(user))
            return null;

        var idClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var nivelClaim = user.FindFirst(ClaimTypes.Role)?.Value;

        if (!int.TryParse(idClaim, out var id))
            return null;

        return new Usuario
        {
            Id = id,
            NivelAcesso = nivelClaim ?? "Usuario"
        };
    }

    private static bool UsuarioNaoEstaAutenticado(ClaimsPrincipal user)
    {
        return user == null || user.Identity == null || !user.Identity.IsAuthenticated;
    }
}
