using Core.Entity;
using Core.Input;
using Core.Repository;
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
        public IActionResult Get()
        {
            try
            {
                var jogosDto = new List<JogoDto>();
                var jogos = _jogoRepository.ObterTodos();

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

        // Fazer check para ver se Usuario é Administrador


    }
}
