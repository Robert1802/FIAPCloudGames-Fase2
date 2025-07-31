using FIAPCloudGames.Domain.Entity;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace FIAPCloudGames.Application.Services
{
    public class UsuarioService : ControllerBase
    {
        public Usuario ObterUsuarioLogado()
        {
            return new Usuario
            {
                Id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0"),
                NivelAcesso = User.FindFirst(ClaimTypes.Role)?.Value ?? "Usuario"
            };
        }

        public void EhAdmin(Usuario usuario)
        {
            Usuario usuarioLogado = ObterUsuarioLogado();

            if (usuario == null)
                BadRequest("Usuário não cadastrado");

            bool ehAdmin = usuarioLogado.NivelAcesso == "Admin";
            bool mesmoUsuario = usuarioLogado.Id != usuario!.Id;

            // Se não for admin e estiver tentando editar/deletar outro usuário
            if (!ehAdmin && mesmoUsuario)
                Forbid("Você não tem permissão para alterar este usuário.");
        }

    }
}
