using Core.Entity;
using Core.Input;
using Core.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FIAPCloudGamesApi.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class PromocaoController : Controller
    {
        private readonly IPromocaoRepository _promocaoRepository;
        private readonly IJogoRepository _jogoRepository;

        public PromocaoController(IPromocaoRepository promocaoRepository, IJogoRepository jogoRepository)
        {
            _promocaoRepository = promocaoRepository;
            _jogoRepository = jogoRepository;
        }


        [HttpGet]
        public IActionResult Get() 
        {
            try
            {
                return Ok(_promocaoRepository.ObterTodos());
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Post([FromBody] PromocaoDto promocaoDto)
        {  
            try
            {
                List<Jogo> jogosNaPromocao = [];

                if(promocaoDto.IdJogo == null || !promocaoDto.IdJogo.Any())
                    return BadRequest("Informe pelo menos um jogo para inserir na promoção");

                foreach (var jogoId in promocaoDto.IdJogo)
                {
                    jogosNaPromocao.Add(_jogoRepository.ObterPorId(jogoId));
                }

                if (jogosNaPromocao.Count != promocaoDto.IdJogo.Count)
                    return NotFound("Um ou mais jogos não foram encontrados.");

                for (int i = 0; i < jogosNaPromocao.Count; i++)
                {
                    jogosNaPromocao[i].Desconto = promocaoDto.descontoJogo[i]/100;
                }


                var novaPromocao = new Promocao()
                {
                    Nome = promocaoDto.Nome,
                    DataInicio = promocaoDto.DataInicio,
                    DataFim = promocaoDto.DataFim,
                    Jogos = jogosNaPromocao
                };

                VerificaSePromocaoExiste(novaPromocao.Nome);

                if (!ValidaPeriodoPromocao(novaPromocao.DataInicio, novaPromocao.DataFim))
                    return BadRequest("As datas de inicio e fim da promoção não são válidas");

                _promocaoRepository.Cadastrar(novaPromocao);

                return Ok(novaPromocao);
            }
            catch (Exception e) when ((e.Message ?? string.Empty).Contains("O nome da promocao informada já existe em nossos servidores"))
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

        }

        private void VerificaSePromocaoExiste(string nome) 
        {
            var promocao = _promocaoRepository.VerificaSePromocaoExiste(nome);
            if (promocao is not null) throw new Exception("O nome da promocao informada já existe em nossos servidores");
        }
        private bool ValidaPeriodoPromocao(DateTime DtInicio, DateTime DtFim)
        {
            if (DtInicio <= DtFim && DtFim >= DtInicio)
                return true;
            else
                return false;
        }

        //[HttpPut]

        //[HttpDelete]
    }
}
