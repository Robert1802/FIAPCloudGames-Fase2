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

    }
}
