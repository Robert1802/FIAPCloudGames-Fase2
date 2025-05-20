using Core.Repository;
using Microsoft.AspNetCore.Mvc;

namespace FIAPCloudGamesApi.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class PromocaoController : Controller
    {
        private readonly IPromocaoRepository _promocaoRepository;

        public PromocaoController(IPromocaoRepository promocaoRepository)
        {
            _promocaoRepository = promocaoRepository;
        }


        [HttpGet]
        public IActionResult Get() 
        {
            try
            {
                //var r
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        //[HttpPost]

        //[HttpPut]

        //[HttpDelete]
    }
}
