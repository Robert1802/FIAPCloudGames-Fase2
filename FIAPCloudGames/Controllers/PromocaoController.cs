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
        [Route("/ListaPromocoes")]
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
        [Route("CriaPromocao/")]
        [Authorize(Roles = "Admin")]
        public IActionResult Post([FromBody] PromocaoInput promocaoInput)
        {  
            try
            {
                List<Jogo> jogosNaPromocao = [];

                if(promocaoInput.IdJogo == null || !promocaoInput.IdJogo.Any())
                    return BadRequest("Informe pelo menos um jogo para inserir na promoção");

                foreach (var jogoId in promocaoInput.IdJogo)
                {
                    jogosNaPromocao.Add(_jogoRepository.ObterPorId(jogoId));
                }

                if (jogosNaPromocao.Count != promocaoInput.IdJogo.Count)
                    return NotFound("Um ou mais jogos não foram encontrados.");

                for (int i = 0; i < jogosNaPromocao.Count; i++)
                {
                    jogosNaPromocao[i].Desconto = promocaoInput.descontoJogo[i]/100;
                }


                var novaPromocao = new Promocao()
                {
                    Nome = promocaoInput.Nome,
                    DataInicio = promocaoInput.DataInicio,
                    DataFim = promocaoInput.DataFim,
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

        [HttpPut]
        [Route("AtualizaPromocoes/")]

        public IActionResult Put([FromBody] PromocaoDto promocaoDto)
        {
            try
            {
                Promocao promocao = _promocaoRepository.ObterPorId(promocaoDto.Id);
                List<Jogo> jogosDaPromocao = [];

                if (promocao is not null) 
                {
                    for (int i = 0; i < promocaoDto.IdJogo.Count; i++)
                    {
                        jogosDaPromocao.Add(_jogoRepository.ObterPorId(promocaoDto.IdJogo[i]));
                        jogosDaPromocao[i].Desconto = promocaoDto.descontoJogo[i];
                    }

                    promocao.Nome = promocaoDto.Nome;
                    promocao.DataInicio = promocaoDto.DataInicio;
                    promocao.DataFim = promocaoDto.DataFim;
                    promocao.Jogos = jogosDaPromocao;

                    return Ok(promocao);
                }
                return BadRequest("Id fornecido não é associado a nenhuma promocao");
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpDelete("DeletaPromocao/{id:int}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete([FromRoute] int id)
        {
            try
            {
                _promocaoRepository.Deletar(id);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
            
        }
    }
}
