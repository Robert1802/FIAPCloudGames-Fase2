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
    [Route("/[controller]")]
    public class JogoController : ControllerBase
    {
        private readonly IJogoRepository _jogoRepository;
        private readonly ILogger<JogoController> _logger;

        public JogoController(IJogoRepository jogoRepository,
                              ILogger<JogoController> logger)
        {
            _jogoRepository = jogoRepository;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] string? filtroNome)
        {
            try
            {
                var jogosDto = new List<JogoDto>();
                List<Jogo> jogos = [];
                if (filtroNome == null)
                    jogos.AddRange(_jogoRepository.ObterTodos());
                else
                    jogos.AddRange(_jogoRepository.ObterTodosFiltro(filtroNome));

                foreach (var jogo in jogos)
                {
                    jogosDto.Add(new JogoDto()
                    {
                        Id = jogo.Id,
                        Nome = jogo.Nome,
                        Empresa = jogo.Empresa,
                        Descricao = jogo.Descricao,
                        Preco = jogo.Preco,                        
                        DataCriacao = jogo.DataCriacao
                    });
                }

                return Ok(jogosDto);
            }
            catch (Exception e)
            {
                string mensagem = "Erro ao tentar trazer todos os Jogos.";
                _logger.LogError(mensagem + " Detalhes: " + e.Message);
                return BadRequest(ApiResponse<string>.Falha(500, mensagem));
            }
        }

        [HttpGet("{id:int}")]
        public IActionResult Get([FromRoute] int id)
        {
            try
            {
                return Ok(_jogoRepository.ObterPorId(id));
            }
            catch (Exception e)
            {
                string mensagem = $"Erro ao tentar trazer um Jogo utilizando o Id: {id}";
                _logger.LogError(mensagem + " Detalhes: " + e.Message);
                return BadRequest(ApiResponse<string>.Falha(500, mensagem));
            }
        }

        [HttpPost("Cadastrar")]
        [Authorize(Roles = "Admin")]
        public IActionResult Post([FromBody] JogoInput input)
        {
            try
            {  
                var jogo = new Jogo()
                {
                    Nome = input.Nome?.Trim() ?? string.Empty,
                    Empresa = input.Empresa,
                    Descricao = input.Descricao?.Trim() ?? string.Empty,
                    Preco = input.Preco,
                    Desconto = 0,
                    UsuarioId = UsuarioLogadoHelper.ObterUsuarioLogado(User)!.Id
                };

                VerificaSeJogoExiste(jogo.Nome);

                _jogoRepository.Cadastrar(jogo);

                _logger.LogInformation($"Jogo \"{jogo.Nome}\" cadastrado com sucesso!");
                return Ok(jogo);
            }
            catch (Exception e) when ((e.Message ?? string.Empty).Contains("já existe em nossos servidores"))
            {
                string mensagem = $"O jogo \"{input.Nome}\" já existe em nossos servidores.";
                _logger.LogError(mensagem + " Detalhes: " + e.Message);
                return BadRequest(mensagem);
            }
            catch (Exception e)
            {
                string mensagem = $"Um erro ocorreu ao tentar cadastrar o jogo: \"{input.Nome}\".";
                _logger.LogError(mensagem + " Detalhes: " + e.Message);
                return BadRequest(ApiResponse<string>.Falha(500, mensagem));
            }
        }

        private void VerificaSeJogoExiste(string nome)
        {
            var jogo = _jogoRepository.VerificarSeJogoExiste(nome);

            if (jogo is not null)
                throw new Exception("O nome do jogo informado já existe em nossos servidores");
        }

        // Fazer check para ver se Usuario é Administrador
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public IActionResult Put([FromBody] JogoUpdateInput input)
        {
            try
            {
                Jogo jogo = _jogoRepository.ObterPorId(input.Id);
                jogo.Nome = input.Nome.Trim();
                jogo.Empresa = input.Empresa;
                jogo.Descricao = input.Descricao.Trim();
                jogo.Preco = input.Preco;
                jogo.UsuarioId = UsuarioLogadoHelper.ObterUsuarioLogado(User)!.Id;

                _jogoRepository.Alterar(jogo);

                string mensagem = $"Jogo \"{jogo.Nome}\" atualizado com sucesso!";
                _logger.LogInformation(mensagem);
                return Ok(mensagem);

            }
            catch (Exception e)
            {
                string mensagem = $"Um erro ocorreu ao tentar atualizar o jogo: \"{input.Nome}\".";
                _logger.LogError(mensagem + " Detalhes: " + e.Message);
                return BadRequest(ApiResponse<string>.Falha(500, mensagem));
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete([FromRoute] int id)
        {
            try
            {
                _jogoRepository.Deletar(id);
                string mensagem = $"Jogo de Id \"{id}\" deletado com sucesso!";
                _logger.LogInformation(mensagem);
                return Ok(ApiResponse<string>.Ok(mensagem));

            }
            catch (Exception e)
            {
                string mensagem = $"Um erro ocorreu ao tentar deletar o jogo de Id: \"{id}\".";
                _logger.LogError(mensagem + " Detalhes: " + e.Message);
                return BadRequest(ApiResponse<string>.Falha(500, mensagem));
            }
        }        
    }
}
