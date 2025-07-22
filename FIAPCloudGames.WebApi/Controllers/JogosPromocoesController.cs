﻿using FIAPCloudGames.WebApi.Helpers;
using FIAPCloudGames.Domain.Entity;
using FIAPCloudGames.Domain.Repository;
using FIAPCloudGames.Domain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FIAPCloudGames.Application.DTO.Request;

namespace FIAPCloudGames.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JogosPromocoesController : ControllerBase
    {
        private readonly IJogoRepository _jogoRepository;
        private readonly IPromocaoRepository _promocaoRepository;
        private readonly IJogosPromocoesRepository _jogosPromocoesRepository;
        private readonly ILogger<JogosPromocoesController> _logger;

        public JogosPromocoesController(
            IJogoRepository jogoRepository, 
            IPromocaoRepository promocaoRepository,
            IJogosPromocoesRepository jogosPromocoesRepository,
            ILogger<JogosPromocoesController> logger)
        {
            _jogoRepository = jogoRepository;
            _promocaoRepository = promocaoRepository;
            _jogosPromocoesRepository = jogosPromocoesRepository;
            _logger = logger;
        }

        /// <summary>
        /// Cria uma promoção para um jogo
        /// </summary>
        [HttpPost("Cadastrar")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        public IActionResult Cadastrar([FromBody] JogosPromocoesRequest input)
        {
            try
            {
                var usuario = UsuarioLogadoHelper.ObterUsuarioLogado(User);
                if (usuario is null)
                {
                    _logger.LogWarning("Usuário não autorizado.");
                    return Unauthorized(ApiResponse<string>.Falha(StatusCodes.Status401Unauthorized, "Usuário não autorizado."));
                }

                var jogo = _jogoRepository.ObterPorId(input.JogoId);
                if (jogo is null)
                {
                    _logger.LogWarning("Jogo não encontrado. JogoId: {JogoId}", input.JogoId);
                    return NotFound(ApiResponse<string>.Falha(StatusCodes.Status404NotFound, "Jogo não encontrado."));
                }

                var promocao = _promocaoRepository.ObterPorId(input.PromocaoId);
                if (promocao is null)
                {
                    _logger.LogWarning("Promoção não encontrada. PromocaoId: {PromocaoId}", input.PromocaoId);
                    return NotFound(ApiResponse<string>.Falha(StatusCodes.Status404NotFound, "Promoção não encontrada."));
                }

                var existePromocaoAtiva = _jogosPromocoesRepository.ExistePromocaoAtivaParaOJogo(input.JogoId);
                if (existePromocaoAtiva)
                {
                    _logger.LogWarning("Já existe uma promoção ativa para o jogo. JogoId: {JogoId}", input.JogoId);
                    return Conflict(ApiResponse<string>
                        .Falha(StatusCodes.Status409Conflict, "Já existe uma promoção ativa para este jogo."));
                }

                var entidade = new JogosPromocoes
                {
                    JogoId = input.JogoId,
                    PromocaoId = input.PromocaoId,
                    Desconto = input.Desconto,
                    UsuarioId = usuario.Id,
                };

                _jogosPromocoesRepository.Cadastrar(entidade);

                _logger.LogInformation("Promoção adicionada ao jogo. JogoId: {JogoId}, PromocaoId: {PromocaoId}", input.JogoId, input.PromocaoId);

                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Erro ao criar promoção para jogo.");
                return StatusCode(StatusCodes.Status500InternalServerError, ApiResponse<string>
                        .Falha(StatusCodes.Status500InternalServerError, $"Erro interno: {e.Message}"));
            }
        }

        /// <summary>
        /// Buscar promoção por ID
        /// </summary>
        [HttpGet("Obter/{id}")]
        [ProducesResponseType(typeof(ApiResponse<JogosPromocoes>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public IActionResult Obter(int id)
        {
            var entidade = _jogosPromocoesRepository.ObterPorId(id);
            if (entidade == null)
            {
                _logger.LogWarning("Promoção do jogo não encontrada. Id: {Id}", id);
                return NotFound(ApiResponse<string>.Falha(StatusCodes.Status404NotFound, "Promoção do jogo não encontrada."));
            }

            _logger.LogInformation("Promoção do jogo encontrada. Id: {Id}", id);
            return Ok(ApiResponse<JogosPromocoes>.Ok(entidade));
        }

        /// <summary>
        /// Listar todas as promoções dos jogos
        /// </summary>
        [HttpGet("ObterTodos")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<JogosPromocoes>>), StatusCodes.Status200OK)]
        public IActionResult ObterTodos()
        {
            var entidades = _jogosPromocoesRepository.ObterTodos();

            _logger.LogInformation("Listagem de promoções dos jogos realizada. Total: {Total}", entidades.Count());
            return Ok(ApiResponse<IEnumerable<JogosPromocoes>>.Ok(entidades));
        }

        /// <summary>
        /// Deletar uma promoção de jogo
        /// </summary>
        [HttpDelete("Deletar/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public IActionResult Deletar(int id)
        {
            var entidade = _jogosPromocoesRepository.ObterPorId(id);
            if (entidade == null)
            {
                _logger.LogWarning("Promoção do jogo não encontrada para deletar. Id: {Id}", id);
                return NotFound(ApiResponse<string>.Falha(StatusCodes.Status404NotFound, "Promoção do jogo não encontrada."));
            }

            _jogosPromocoesRepository.Deletar(id);

            _logger.LogInformation("Promoção do jogo deletada. Id: {Id}", id);
            return Ok(ApiResponse<string>.Ok("Promoção excluída com sucesso."));
        }
    }
}
