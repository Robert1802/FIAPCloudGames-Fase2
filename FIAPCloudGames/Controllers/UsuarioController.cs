using Core.Entity;
using Core.Input;
using Core.Repository;
using Core.Responses;
using Core.Utils;
using FIAPCloudGamesApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FIAPCloudGamesApi.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioController(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        [HttpGet("{id:int}")]
        public IActionResult Get([FromRoute] int id)
        {
            try
            {
                return Ok(_usuarioRepository.ObterPorId(id));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] UsuarioInput input)
        {
            try
            {
                var usuario = new Usuario
                {
                    Nome = input.Nome,
                    Email = input.Email,
                    Senha = PasswordHelper.HashSenha(input.Senha),
                    NivelAcesso = "Usuario",
                    Saldo = 0
                };

                if (_usuarioRepository.ObterPorEmail(input.Email) == null)
                {
                    _usuarioRepository.Cadastrar(usuario);
                    return Ok(usuario);
                }

                return BadRequest("E-mail já cadastrado");
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPut]
        [Authorize]
        public IActionResult Put([FromBody] UsuarioUpdateInput input)
        {
            try
            {
                var validacao = ValidarAlteracaoUsuario(input, out var usuario);
                if (validacao != null)
                    return validacao;

                usuario.Nome = input.Nome;
                usuario.Senha = PasswordHelper.HashSenha(input.Senha);

                _usuarioRepository.Alterar(usuario);

                return Ok(ApiResponse<Usuario>.Ok(usuario));
            }
            catch (Exception e)
            {
                return BadRequest(ApiResponse<string>.Falha(500, $"Erro inesperado: {e.Message}"));
            }
        }

        private IActionResult? ValidarAlteracaoUsuario(UsuarioUpdateInput input, out Usuario usuario)
        {
            var usuarioLogado = UsuarioLogadoHelper.ObterUsuarioLogado(User);

            if (usuarioLogado == null)
            {
                usuario = null!;
                return Unauthorized(ApiResponse<string>.Falha(401, "Usuário não autenticado."));
            }

            usuario = _usuarioRepository.ObterPorId(input.Id);
            if (usuario == null)
            {
                return BadRequest(ApiResponse<string>.Falha(400, "Usuário não cadastrado."));
            }

            var ehAdmin = usuarioLogado.NivelAcesso == "Admin";

            if (NaoEhAdminEQuerEditarOutroUsuario(input, usuarioLogado, ehAdmin))
            {
                return StatusCode(403, ApiResponse<string>.Falha(403, "Você não tem permissão para alterar este usuário."));
            }

            return null;
        }

        [HttpDelete("{id:int}")]
        [Authorize]
        public IActionResult Delete([FromRoute] int id)
        {
            try
            {
                var usuario = _usuarioRepository.ObterPorId(id);
                if (usuario == null)
                    return BadRequest("Usuário não encontrado.");

                _usuarioRepository.Deletar(id);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost("depositar")]
        public IActionResult Depositar([FromBody] UsuarioDeposito input)
        {
            try
            {
                if (_usuarioRepository.ObterPorId(input.Id) == null)
                    return BadRequest("Usuário inexistente.");

                if (input.Deposito < 0)
                    return BadRequest($"Valor de R$ {input.Deposito} inválido.");

                var saldo = _usuarioRepository.Depositar(input.Id, input.Deposito);
                return Ok($"Foi depositado R$ {input.Deposito}. O novo saldo é de R$ {saldo}");
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        private static bool NaoEhAdminEQuerEditarOutroUsuario(UsuarioUpdateInput input, Usuario usuarioLogado, bool ehAdmin)
        {
            return !ehAdmin && usuarioLogado.Id != input.Id;
        }
    }
}
