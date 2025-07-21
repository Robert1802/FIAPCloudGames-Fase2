using Azure;
using FIAPCloudGames.Domain.Entity;
using FIAPCloudGames.Domain.Input;
using FIAPCloudGames.Domain.Repository;
using FIAPCloudGames.Domain.Responses;
using FIAPCloudGames.Domain.Utils;
using FIAPCloudGames.WebApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FIAPCloudGames.WebApi.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ILogger<UsuarioController> _logger;

        public UsuarioController(IUsuarioRepository usuarioRepository,
                              ILogger<UsuarioController> logger)
        {
            _usuarioRepository = usuarioRepository;
            _logger = logger;
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<UsuarioResponse>), StatusCodes.Status200OK)]
        public IActionResult Get([FromRoute] int id)
        {
            try
            {
                Usuario usuario = _usuarioRepository.ObterPorId(id);
                UsuarioResponse response = new
                (
                    usuario.Id,
                    usuario.Nome,
                    usuario.Email,
                    usuario.Senha,
                    usuario.NivelAcesso,
                    usuario.Saldo
                );

                return Ok(ApiResponse<UsuarioResponse>.Ok(response));
            }
            catch (Exception e)
            {
                string mensagem = $"Erro ao tentar trazer um usuario utilizando o Id: {id}."; 
                _logger.LogError(mensagem + " Detalhes: " + e.Message);
                return BadRequest(mensagem);
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

                if (_usuarioRepository.ObterPorEmail(input.Email) != null)
                    return BadRequest($"Já existe um usuário utilizando o E-mail \"{input.Email}\" em nossa base de dados");
                
                _usuarioRepository.Cadastrar(usuario);
                _logger.LogInformation($"Usuario \"{usuario.Nome}\" com email \"{usuario.Email}\" cadastrado com sucesso!");
                return Ok(ApiResponse<Usuario>.Ok(usuario));
                

            }
            catch (Exception e)
            {
                string mensagem = $"Erro ao tentar cadastrar um usuario utilizando o e-mail: {input.Email}";
                _logger.LogError(mensagem + " Detalhes: " + e.Message);
                return BadRequest(mensagem);
            }
        }

        [HttpPut]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<UsuarioResponse>), StatusCodes.Status200OK)]
        public IActionResult Put([FromBody] UsuarioUpdateInput input)
        {
            try
            {
                var validacao = ValidarAlteracaoUsuario(input.Id, out var usuario);
                if (validacao != null)
                    return validacao;

                usuario.Nome = input.Nome;
                usuario.Senha = PasswordHelper.HashSenha(input.Senha);

                _usuarioRepository.Alterar(usuario);

                UsuarioResponse response = new
                (
                    usuario.Id,
                    usuario.Nome,
                    usuario.Email,
                    usuario.Senha,
                    usuario.NivelAcesso,
                    usuario.Saldo
                );

                _logger.LogInformation($"Usuario \"{input.Nome}\" alterado com sucesso!");
                return Ok(ApiResponse<UsuarioResponse>.Ok(response));
            }
            catch (Exception e)
            {
                string mensagem = $"Erro inesperado ao tentar alterar o usuario: {input.Nome}.";
                _logger.LogError(mensagem + "Detalhes: " + e.Message);
                return BadRequest(ApiResponse<string>.Falha(500, mensagem));
            }
        }

        [HttpPut("administrador")]
        [Authorize(Roles = "Admin")]
        public IActionResult TransformarEmAdmin([FromBody] UsuarioAdminInput input)
        {
            try
            {
                var validacao = ValidarAlteracaoUsuario(input.IdUsuario, out var usuario);
                if (validacao != null)
                    return validacao;

                usuario.NivelAcesso = "Admin";

                _usuarioRepository.Alterar(usuario);

                _logger.LogInformation($"Usuario \"{usuario.Nome}\" agora é Admin!");
                return Ok(ApiResponse<Usuario>.Ok(usuario));
            }
            catch (Exception e)
            {
                string mensagem = $"Erro inesperado ao tentar alterar o usuario de Id: {input.IdUsuario}.";
                _logger.LogError(mensagem + "Detalhes: " + e.Message);
                return BadRequest(ApiResponse<string>.Falha(500, mensagem));
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize]
        public IActionResult Delete([FromRoute] int id)
        {
            try
            {
                var validacao = ValidarAlteracaoUsuario(id, out var usuario);
                if (validacao != null)
                    return validacao;

                _usuarioRepository.Deletar(id);
                string mensagem = $"Usuario \"{usuario.Nome}\" deletado com sucesso!";
                _logger.LogInformation(mensagem);
                return Ok(ApiResponse<string>.Ok(mensagem));
            }
            catch (Exception e)
            {
                string mensagem = $"Erro inesperado ao tentar deletar o usuario de Id: {id}.";
                _logger.LogError(mensagem + "Detalhes: " + e.Message);
                return BadRequest(ApiResponse<string>.Falha(500, mensagem));
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
                string mensagem = $"Foi depositado R${input.Deposito} para o usuario de Id: {input.Id}. O saldo atual é de é de R${saldo}";
                _logger.LogInformation(mensagem);
                return Ok(ApiResponse<string>.Ok(mensagem));
            }
            catch (Exception e)
            {
                string mensagem = $"Erro inesperado ao tentar depositar {input.Deposito} para o usuario de Id: {input.Id}.";
                _logger.LogError(mensagem + "Detalhes: " + e.Message);
                return BadRequest(ApiResponse<string>.Falha(500, mensagem));
            }
        }

        private static bool NaoEhAdminEQuerEditarOutroUsuario(int idUsuario, Usuario usuarioLogado, bool ehAdmin)
        {
            return !ehAdmin && usuarioLogado.Id != idUsuario;
        }

        private IActionResult? ValidarAlteracaoUsuario(int idUsuario, out Usuario usuario)
        {
            var usuarioLogado = UsuarioLogadoHelper.ObterUsuarioLogado(User);

            if (usuarioLogado == null)
            {
                usuario = null!;
                return Unauthorized(ApiResponse<string>.Falha(401, "Usuário não autenticado."));
            }

            usuario = _usuarioRepository.ObterPorId(idUsuario);
            if (usuario == null)
            {
                return BadRequest(ApiResponse<string>.Falha(400, "Usuário não cadastrado."));
            }

            var ehAdmin = usuarioLogado.NivelAcesso == "Admin";

            if (NaoEhAdminEQuerEditarOutroUsuario(idUsuario, usuarioLogado, ehAdmin))
            {
                return StatusCode(403, ApiResponse<string>.Falha(403, "Você não tem permissão para alterar este usuário."));
            }

            return null;
        }
    }
}
