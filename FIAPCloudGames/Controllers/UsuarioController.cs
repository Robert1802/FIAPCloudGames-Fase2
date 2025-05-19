using Core.Entity;
using Core.Input;
using Core.Repository;
using Core.Utils;
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
        public IActionResult Get([FromRoute] int id) // ou Get(int id)
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
                var usuario = new Usuario()
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
                else
                {
                    return BadRequest("E-mail já cadastrado");
                }
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
                var usuarioLogado = ObterUsuarioLogado();
                var usuario = _usuarioRepository.ObterPorId(input.Id);

                if (usuario == null)
                    return BadRequest("Usuário não cadastrado");

                var ehAdmin = usuarioLogado.NivelAcesso == "Admin";

                // Se não for admin e estiver tentando editar outro usuário
                if (!ehAdmin && usuarioLogado.Id != input.Id)
                    return Forbid("Você não tem permissão para alterar este usuário.");

                usuario.Nome = input.Nome;
                usuario.Senha = PasswordHelper.HashSenha(input.Senha);

                _usuarioRepository.Alterar(usuario);

                return Ok(usuario);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        private Usuario ObterUsuarioLogado()
        {
            return new Usuario
            {
                Id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0"),
                NivelAcesso = User.FindFirst(ClaimTypes.Role)?.Value ?? "Usuario"
            };
        }



        [HttpDelete("{id:int}")]
        public IActionResult Delete([FromRoute] int id)
        {
            try
            {
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
                // verificar se usuario existe
                if (_usuarioRepository.ObterPorId(input.Id) == null)
                    return BadRequest("Usuario Inexistente.");

                // Verificar se valor é válido
                if(input.Deposito < 0)
                    return BadRequest($"Valor de R$ {input.Deposito} invalido");

                // Depositar valor no saldo
                var saldo = _usuarioRepository.Depositar(input.Id, input.Deposito);
                return Ok($"Foi depositado {input.Deposito}. O novo saldo é de {saldo}");
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

    }
}
