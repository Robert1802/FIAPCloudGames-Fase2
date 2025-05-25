using Microsoft.AspNetCore.Mvc;
using Core.Entity;
using Core.Input;
using Core.Repository;
using Core.Responses;
using Microsoft.AspNetCore.Authorization;
using FIAPCloudGamesApi.Helpers;

namespace FIAPCloudGamesApi.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class UsuarioJogoController : Controller
    {
        private readonly IUsuarioJogoRepository _usuarioJogoRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IJogoRepository _jogoRepository;
        private readonly IJogosPromocoesRepository _jogosPromocoesRepository;
        private readonly ILogger<UsuarioController> _logger;

        public UsuarioJogoController(IUsuarioJogoRepository usuarioJogoRepository,
                                     IUsuarioRepository usuarioRepository,
                                     IJogoRepository jogoRepository,
                                     IJogosPromocoesRepository jogosPromocoes,
                                     ILogger<UsuarioController> logger)
        {
            _usuarioJogoRepository = usuarioJogoRepository;
            _usuarioRepository = usuarioRepository;
            _jogoRepository = jogoRepository;
            _jogosPromocoesRepository = jogosPromocoes;
            _logger = logger;
        }

        [ProducesResponseType(typeof(ApiResponse<CompraJogoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<CompraJogoResponse>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<CompraJogoResponse>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<CompraJogoResponse>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<CompraJogoResponse>), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ApiResponse<CompraJogoResponse>), StatusCodes.Status500InternalServerError)]
        [HttpPost("comprar")]
        [Authorize]
        public IActionResult Comprar([FromBody] UsuarioJogoInput input)
        {
            try
            {
                _logger.LogInformation("Iniciando processo de compra. UsuarioId: {UsuarioId}, JogoId: {JogoId}, PromocaoId: {PromocaoId}",
                    input.UsuarioId, input.JogoId, input.PromocaoId);

                var usuario = UsuarioLogadoHelper.ObterUsuarioLogado(User);
                if (usuario == null || usuario.Id != input.UsuarioId)
                {
                    _logger.LogError(
                        "Usuário não autorizado para compra. UsuarioId informado: {UsuarioIdInformado}, UsuarioLogado: {UsuarioLogado}",
                        input.UsuarioId,
                        usuario?.Id.ToString() ?? "Nenhum"
                    );

                    return StatusCode(StatusCodes.Status401Unauthorized,
                        ApiResponse<CompraJogoResponse>.Falha(StatusCodes.Status401Unauthorized, "Usuário não autorizado para compra."));
                }

                var jogo = _jogoRepository.ObterPorId(input.JogoId);
                if (jogo == null)
                {
                    _logger.LogWarning("Tentativa de compra para JogoId {JogoId} que não existe.", input.JogoId);

                    return StatusCode(StatusCodes.Status404NotFound,
                        ApiResponse<CompraJogoResponse>.Falha(StatusCodes.Status404NotFound, "Jogo não encontrado."));
                }

                var jaPossui = _usuarioJogoRepository.ObterPorIdUsuarioIdJogo(input.UsuarioId, input.JogoId);
                if (jaPossui != null)
                {
                    _logger.LogWarning("Usuário {UsuarioId} tentou comprar o JogoId {JogoId} que já possui.", input.UsuarioId, input.JogoId);

                    return StatusCode(StatusCodes.Status409Conflict,
                        ApiResponse<CompraJogoResponse>.Falha(StatusCodes.Status409Conflict, "Usuário já possui este jogo."));
                }

                var promocao = _jogosPromocoesRepository.ObterPromocaoAtivaDoJogo(input.JogoId, input.PromocaoId);
                var descontoAplicado = promocao?.Desconto ?? 0;
                var precoFinal = (jogo.Preco * (1 - descontoAplicado / 100));

                _logger.LogInformation("Preço final calculado para JogoId {JogoId}: {PrecoFinal} (Desconto aplicado: {Desconto})",
                    input.JogoId, precoFinal, descontoAplicado);

                var saldoAtual = _usuarioRepository.ConferirSaldo(input.UsuarioId);
                _logger.LogInformation("Saldo atual do usuário {UsuarioId}: {SaldoAtual}", input.UsuarioId, saldoAtual);

                if (saldoAtual < precoFinal)
                {
                    _logger.LogWarning("Saldo insuficiente. UsuarioId: {UsuarioId}, Saldo: {SaldoAtual}, PrecoFinal: {PrecoFinal}",
                        input.UsuarioId, saldoAtual, precoFinal);

                    return StatusCode(StatusCodes.Status422UnprocessableEntity,
                        ApiResponse<CompraJogoResponse>.Falha(StatusCodes.Status422UnprocessableEntity, "Saldo insuficiente para comprar o jogo."));
                }

                _usuarioRepository.Subtrair(input.UsuarioId, precoFinal);
                _logger.LogInformation("Saldo subtraído com sucesso. UsuarioId: {UsuarioId}, Valor: {PrecoFinal}", input.UsuarioId, precoFinal);

                var usuarioJogo = new UsuarioJogo
                {
                    UsuarioId = input.UsuarioId,
                    JogoId = input.JogoId,
                    PrecoDaCompra = precoFinal,
                    PromocaoId = promocao?.PromocaoId
                };
                _usuarioJogoRepository.Cadastrar(usuarioJogo);

                _logger.LogInformation("Compra cadastrada com sucesso. UsuarioId: {UsuarioId}, JogoId: {JogoId}, PromocaoId: {PromocaoId}",
                    input.UsuarioId, input.JogoId, promocao?.PromocaoId.ToString() ?? "Nenhum");

                var response = new CompraJogoResponse(
                    input.UsuarioId,
                    input.JogoId,
                    jogo.Nome,
                    precoFinal,
                    promocao?.PromocaoId,
                    descontoAplicado,
                    "Compra realizada com sucesso."
                );

                _logger.LogInformation("Compra finalizada com sucesso para UsuarioId: {UsuarioId}, JogoId: {JogoId}", input.UsuarioId, input.JogoId);

                return Ok(ApiResponse<CompraJogoResponse>.Ok(response));
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "Erro inesperado no processo de compra. UsuarioId: {UsuarioId}, JogoId: {JogoId}", input.UsuarioId, input.JogoId);

                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<CompraJogoResponse>.Falha(StatusCodes.Status500InternalServerError, $"Erro interno: {e.Message}"));
            }
        }

        // Trazer jogos deste usuário
        [ProducesResponseType(typeof(ApiResponse<CompraJogoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<CompraJogoResponse>), StatusCodes.Status500InternalServerError)]
        [HttpGet("{usuarioId:int}")]
        public IActionResult Get([FromRoute] int usuarioId)
        {
            try
            {
                _logger.LogInformation("Iniciando processo de obtenção de jogos do usuário, para o usuário id {UsuarioId}", usuarioId);

                var response = _usuarioJogoRepository.JogosCompradosPorUsuario(usuarioId);

                _logger.LogInformation("Sucesso na obtenção de jogos do usuário, para o usuário id {UsuarioId}", usuarioId);

                return Ok(ApiResponse<List<UsuarioJogo>>.Ok(response));
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "Erro inesperado no processo de obter jogos do usuários. UsuarioId: {UsuarioId}", usuarioId);

                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<UsuarioJogo>.Falha(StatusCodes.Status500InternalServerError, $"Erro interno: {e.Message}"));
            }
        }
    }
}
