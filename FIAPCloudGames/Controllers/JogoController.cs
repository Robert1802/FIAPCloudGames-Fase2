using Core.Entity;
using Core.Input;
using Core.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FIAPCloudGamesApi.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class JogoController : ControllerBase
    {
        private readonly IJogoRepository _jogoRepository;

        public JogoController(IJogoRepository jogoRepository)
        {
            _jogoRepository = jogoRepository;
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
                        Desconto = jogo.Desconto,
                        DataCriacao = jogo.DataCriacao
                    });
                }

                return Ok(jogosDto);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id:int}")]
        public IActionResult Get([FromRoute] int id) // ou Get(int id)
        {
            try
            {
                return Ok(_jogoRepository.ObterPorId(id));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles="Admin")]
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
                    Desconto = 0
                };

                VerificaSeJogoExiste(jogo.Nome);

                _jogoRepository.Cadastrar(jogo);
                return Ok(jogo);
            }
            catch (Exception e) when ((e.Message ?? string.Empty).Contains("O nome do jogo informado já existe em nossos servidores"))
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
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
                jogo.Nome = input.Nome;
                jogo.Empresa = input.Empresa;
                jogo.Descricao = input.Descricao;
                jogo.Preco = input.Preco;
                _jogoRepository.Alterar(jogo);
                return Ok();

            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete([FromRoute] int id)
        {
            try
            {
                _jogoRepository.Deletar(id);
                return Ok();

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("desconto")]
        [Authorize(Roles = "Admin")]
        public IActionResult AtualizarDesconto([FromBody] JogoDescontoInput input)
        {
            try
            {
                var jogo = _jogoRepository.ObterPorId(input.Id);
                if (jogo != null)
                {
                    if (input.Desconto > 100 || input.Desconto < 0)
                        return BadRequest("Valor de desconto indevido");

                    jogo.Desconto = input.Desconto/100;
                    _jogoRepository.Alterar(jogo);
                    return Ok();
                }
                else
                {
                    return BadRequest($"Id {input.Id} de Jogo inexistente.");
                }

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }



    }
}
