using Microsoft.AspNetCore.Mvc;
using Core.Entity;
using Core.Input;
using Core.Repository;
using Infrastructure.Repository;
using Azure.Identity;
using Core.Responses;

namespace FIAPCloudGamesApi.Controllers
{
    [ApiController]    
    [Route("/[controller]")]
    public class UsuarioJogoController : Controller
    {
        private readonly IUsuarioJogoRepository _usuarioJogoRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IJogoRepository _jogoRepository;
        private readonly IJogosPromocoes _jogosPromocoesRepository;
        

        public UsuarioJogoController(IUsuarioJogoRepository usuarioJogoRepository,
                                     IUsuarioRepository usuarioRepository,
                                     IJogoRepository jogoRepository,
                                     IJogosPromocoes jogosPromocoes)
        {
            _usuarioJogoRepository = usuarioJogoRepository;
            _usuarioRepository = usuarioRepository;
            _jogoRepository = jogoRepository;
            _jogosPromocoesRepository = jogosPromocoes;
        }

        [HttpPost("comprar")]
        public IActionResult Comprar([FromBody] UsuarioJogoInput input)
        {
            try
            {
                var usuario = Helpers.UsuarioLogadoHelper.ObterUsuarioLogado(User);
                if (usuario == null || usuario.Id != input.UsuarioId)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized,
                        ApiResponse<CompraJogoResponse>.Falha(StatusCodes.Status401Unauthorized, "Usuário não autorizado para compra."));
                }

                var jogo = _jogoRepository.ObterPorId(input.JogoId);
                if (jogo == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound,
                        ApiResponse<CompraJogoResponse>.Falha(StatusCodes.Status404NotFound, "Jogo não encontrado."));
                }

                var jaPossui = _usuarioJogoRepository.ObterPorIdUsuarioIdJogo(input.UsuarioId, input.JogoId);
                if (jaPossui != null)
                {
                    return StatusCode(StatusCodes.Status409Conflict,
                        ApiResponse<CompraJogoResponse>.Falha(StatusCodes.Status409Conflict, "Usuário já possui este jogo."));
                }

                var promocao = _jogosPromocoesRepository.ObterPromocaoAtivaDoJogo(input.JogoId, input.PromocaoId);
                var descontoAplicado = promocao?.Desconto ?? 0;
                var precoFinal = jogo.Preco * (1 - descontoAplicado);

                var saldoAtual = _usuarioRepository.ConferirSaldo(input.UsuarioId);
                if (saldoAtual < precoFinal)
                {
                    return StatusCode(StatusCodes.Status422UnprocessableEntity,
                        ApiResponse<CompraJogoResponse>.Falha(StatusCodes.Status422UnprocessableEntity, "Saldo insuficiente para comprar o jogo."));
                }

                _usuarioRepository.Subtrair(input.UsuarioId, precoFinal);

                var usuarioJogo = new UsuarioJogo
                {
                    UsuarioId = input.UsuarioId,
                    JogoId = input.JogoId,
                    PrecoDaCompra = precoFinal,
                    PromocaoId = promocao?.PromocaoId
                };
                _usuarioJogoRepository.Cadastrar(usuarioJogo);

                var response = new CompraJogoResponse(
                    input.UsuarioId,
                    input.JogoId,
                    jogo.Nome,
                    precoFinal,
                    promocao?.PromocaoId,
                    descontoAplicado,
                    "Compra realizada com sucesso."
                );

                return Ok(ApiResponse<CompraJogoResponse>.Ok(response));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<CompraJogoResponse>.Falha(StatusCodes.Status500InternalServerError, $"Erro interno: {e.Message}"));
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
