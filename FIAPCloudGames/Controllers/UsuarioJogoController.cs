using Microsoft.AspNetCore.Mvc;
using Core.Entity;
using Core.Input;
using Core.Repository;
using Infrastructure.Repository;
using Azure.Identity;

namespace FIAPCloudGamesApi.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class UsuarioJogoController : Controller
    {
        private readonly IUsuarioJogoRepository _usuarioJogoRepository;
        private readonly IJogoRepository _jogoRepository;

        public UsuarioJogoController(IUsuarioJogoRepository usuarioJogoRepository,
                                     IJogoRepository jogoRepository)
        {
            _usuarioJogoRepository = usuarioJogoRepository;
            _jogoRepository = jogoRepository;
        }

        // Atribuir Jogo a um usuario
        [HttpPost("comprar")]
        public IActionResult Post([FromBody] UsuarioJogoInput input)
        {
            try
            {
                // Verificar se o jogo ja foi comprado por este usuario
                var usuarioJogoComprado = _usuarioJogoRepository.ObterPorIdUsuario(input.IdUsuario, input.IdJogo);
                if (usuarioJogoComprado == null)
                {
                    // Verifica preço atual
                    var jogoAtual = _jogoRepository.ObterPorId(input.IdJogo);
                    var precoAtual = jogoAtual.Preco - (jogoAtual.Preco * jogoAtual.Desconto);

                    // TODO: Carteira
                    // Traz saldo do usuario
                    // Subtrai do saldo do usuario
                    // Usuario.Carteira - precoAtual

                    var usuarioJogoNovo = new UsuarioJogo()
                    {
                        IdUsuario = input.IdUsuario,
                        IdJogo = input.IdJogo,
                        PrecoDaCompra = precoAtual
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
