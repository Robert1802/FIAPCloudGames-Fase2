using Core.Entity;
using Core.Input;
using Core.Repository;
using Core.Responses;
using FIAPCloudGamesApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FIAPCloudGamesApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PromocaoController : ControllerBase
    {
        private readonly IPromocaoRepository _promocaoRepository;
        private readonly ILogger<PromocaoController> _logger;

        public PromocaoController(IPromocaoRepository promocaoRepository, ILogger<PromocaoController> logger)
        {
            _promocaoRepository = promocaoRepository;
            _logger = logger;
        }

        [HttpPost("Cadastrar")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        public IActionResult Cadastrar([FromBody] PromocaoInput input)
        {
            try
            {
                var usuarioLogado = UsuarioLogadoHelper.ObterUsuarioLogado(User);
                if (usuarioLogado == null)
                {
                    _logger.LogWarning("Tentativa de criar promoção sem usuário autenticado.");
                    return Unauthorized(ApiResponse<string>.Falha(StatusCodes.Status401Unauthorized, "Usuário não autorizado"));
                }

                var promocao = new Promocao
                {
                    Nome = input.Nome,
                    DataInicio = input.DataInicio,
                    DataFim = input.DataFim,
                    Ativo = input.Ativo,
                    UsuarioId = usuarioLogado.Id

                };

                VerificaSePromocaoExiste(promocao.Nome);
                ValidaPeriodoPromocao(promocao.DataInicio, promocao.DataFim);

                _promocaoRepository.Cadastrar(promocao);
                _logger.LogInformation("Promoção criada com sucesso. PromoçãoId: {PromocaoId}, CriadaPor: {UsuarioId}",
                    promocao.Id, usuarioLogado.Id);

                return StatusCode(StatusCodes.Status201Created, ApiResponse<int>.Ok(promocao.Id));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Erro ao criar promoção");
                return StatusCode(500, ApiResponse<string>.Falha(StatusCodes.Status500InternalServerError, $"Erro interno: {e.Message}"));
            }
        }
        private void VerificaSePromocaoExiste(string nome)
        {
            var promocao = _promocaoRepository.VerificarSePromocaoExiste(nome);
            if (promocao is not null) throw new Exception("O nome da promoção informada já existe em nossos servidores");
        }
        private void ValidaPeriodoPromocao(DateTime DtInicio, DateTime DtFim)
        {
            if (!(DtInicio <= DtFim && DtFim >= DtInicio))
                throw new Exception("Datas da promoção inválidas");
        }



        [HttpGet("Obter/{id}")]
        [ProducesResponseType(typeof(ApiResponse<Promocao>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public IActionResult Obter(int id)
        {
            var promocao = _promocaoRepository.ObterPorId(id);
            if (promocao == null)
            {
                _logger.LogWarning("Promoção não encontrada. PromoçãoId: {PromocaoId}", id);
                return NotFound(ApiResponse<string>.Falha(404, "Promoção não encontrada"));
            }

            _logger.LogInformation("Promoção consultada. PromoçãoId: {PromocaoId}", id);
            return Ok(ApiResponse<Promocao>.Ok(promocao));
        }

        [HttpGet("ObterTodos")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<Promocao>>), StatusCodes.Status200OK)]
        public IActionResult ObterTodos()
        {
            var promocoes = _promocaoRepository.ObterTodos();
            _logger.LogInformation("Listagem de promoções realizada. Total: {Total}", promocoes.Count());
            return Ok(ApiResponse<IEnumerable<Promocao>>.Ok(promocoes));
        }

        [HttpPut("Alterar/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse<Promocao>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public IActionResult Alterar(int id, [FromBody] PromocaoInput input)
        {
            var promocao = _promocaoRepository.ObterPorId(id);
            if (promocao == null)
            {
                _logger.LogWarning("Tentativa de atualizar promoção inexistente. PromoçãoId: {PromocaoId}", id);
                return NotFound(ApiResponse<string>.Falha(StatusCodes.Status404NotFound, "Promoção não encontrada"));
            }

            promocao.Nome = input.Nome;
            promocao.DataInicio = input.DataInicio;
            promocao.DataFim = input.DataFim;
            promocao.Ativo = input.Ativo;
            promocao.UsuarioId = UsuarioLogadoHelper.ObterUsuarioLogado(User)!.Id;
            _promocaoRepository.Alterar(promocao);

            _logger.LogInformation("Promoção atualizada com sucesso. PromoçãoId: {PromocaoId}", id);

            return Ok(ApiResponse<Promocao>.Ok(promocao));
        }

        [HttpDelete("Deletar/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public IActionResult Deletar(int id)
        {
            var promocao = _promocaoRepository.ObterPorId(id);
            if (promocao == null)
            {
                _logger.LogWarning("Tentativa de deletar promoção inexistente. PromoçãoId: {PromocaoId}", id);
                return NotFound(ApiResponse<string>.Falha(StatusCodes.Status404NotFound, "Promoção não encontrada"));
            }

            _promocaoRepository.Deletar(id);

            _logger.LogInformation("Promoção deletada com sucesso pelo usuário {usuario}. PromoçãoId: {PromocaoId}", 
                UsuarioLogadoHelper.ObterUsuarioLogado(User)!.Nome , 
                id);

            return NoContent();
        }
    }
}
