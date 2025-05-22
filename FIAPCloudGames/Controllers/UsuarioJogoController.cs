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
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IJogoRepository _jogoRepository;

        public UsuarioJogoController(IUsuarioJogoRepository usuarioJogoRepository,
                                     IUsuarioRepository usuarioRepository,
                                     IJogoRepository jogoRepository)
        {
            _usuarioJogoRepository = usuarioJogoRepository;
            _usuarioRepository = usuarioRepository;
            _jogoRepository = jogoRepository;
        }

        // Atribuir Jogo a um usuario
        [HttpPost("comprar")]
        public IActionResult Post([FromBody] UsuarioJogoInput input)
        {
            try
            {
                // Verificar se o jogo ja foi comprado por este usuario
                var usuarioJogoComprado = _usuarioJogoRepository.ObterPorIdUsuarioIdJogo(input.IdUsuario, input.IdJogo);
                if (usuarioJogoComprado == null)
                {
                    // Verifica preço atual
                    var jogoAtual = _jogoRepository.ObterPorId(input.IdJogo);
                    var precoAtual = jogoAtual.Preco - (jogoAtual.Preco * jogoAtual.Desconto);

                    // Traz saldo do usuario
                    var saldoAtual = _usuarioRepository.ConferirSaldo(input.IdUsuario);

                    // Verificar se saldo vai ficar negativo
                    if(saldoAtual < precoAtual)
                        return BadRequest("Saldo insuficiente para comprar o jogo.");

                    // Subtrai do saldo do usuario
                    _usuarioRepository.Subtrair(input.IdUsuario, precoAtual);

                    var usuarioJogoNovo = new UsuarioJogo()
                    {
                        UsuarioId = input.IdUsuario,
                        JogoId = input.IdJogo,
                        PrecoDaCompra = precoAtual,
                        DataCriacao = DateTime.Now,
                        Jogo = jogoAtual,
                        Promocao = null,
                        PromocaoId = 1,
                        Usuario = Helpers.UsuarioLogadoHelper.ObterUsuarioLogado(User)

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
