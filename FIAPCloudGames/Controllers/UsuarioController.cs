using Core.Entity;
using Core.Input;
using Core.Repository;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Mvc;

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
                    Senha = input.Senha,
                    NivelAcesso = "Usuario"
                };
                if(_usuarioRepository.ObterPorEmail(input.Email) == null)
                {
                    _usuarioRepository.Cadastrar(usuario);
                    return Ok(usuario);
                }
                else
                {
                    return BadRequest("Email já cadastrado");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody] UsuarioUpdateInput input)
        {
            try
            {
                var usuario = _usuarioRepository.ObterPorId(input.Id);
                if (usuario == null)
                    return BadRequest("Usuário não cadastrado");
                usuario.Nome = input.Nome;
                usuario.Senha = input.Senha;
                _usuarioRepository.Alterar(usuario);
                return Ok(usuario);

            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
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

    }
}
