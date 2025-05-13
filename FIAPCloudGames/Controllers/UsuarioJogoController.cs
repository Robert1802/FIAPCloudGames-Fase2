using Microsoft.AspNetCore.Mvc;
using Core.Entity;
using Core.Input;
using Core.Repository;
using Infrastructure.Repository;

namespace FIAPCloudGamesApi.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class UsuarioJogoController : Controller
    {
        private readonly IUsuarioJogoRepository _usuarioJogoRepository;

        public UsuarioJogoController(IUsuarioJogoRepository usuarioJogoRepository)
        {
            _usuarioJogoRepository = usuarioJogoRepository;
        }

        // Atribuir Jogo a um usuario
        [HttpPost("comprar")]
        public IActionResult Post([FromBody] UsuarioJogoInput input)
        {
            try
            {

                var usuarioJogoComprado = _usuarioJogoRepository.ObterPorIdUsuario(input.IdUsuario, input.IdJogo);
                if (usuarioJogoComprado == null)
                {
                    var usuarioJogoNovo = new UsuarioJogo()
                    {
                        IdUsuario = input.IdUsuario,
                        IdJogo = input.IdJogo
                    };
                    _usuarioJogoRepository.Cadastrar(usuarioJogoNovo);
                    return Ok(usuarioJogoNovo);
                }
                else
                {
                    return BadRequest("Usuário ja possui este Jogo");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        // Trazer jogos deste usuário
        [HttpGet("{idUsuario:int}")]
        public IActionResult Get([FromRoute] int idUsuario) // ou Get(int id)
        {
            try
            {
                return Ok(_usuarioJogoRepository.JogosCompradosPorUsuario(idUsuario));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
