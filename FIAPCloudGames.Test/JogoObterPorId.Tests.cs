using Core.Entity;
using Core.Repository;
using Core.Responses;
using FIAPCloudGamesApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace FIAPCloudGames.Test
{
    public class JogoObterPorId
    {
        private readonly Mock<IJogoRepository> _jogoRepositoryMock;
        private readonly Mock<ILogger<JogoController>> _loggerMock;
        private readonly JogoController _controller;

        public JogoObterPorId()
        {
            _jogoRepositoryMock = new Mock<IJogoRepository>();
            _loggerMock = new Mock<ILogger<JogoController>>();
            _controller = new JogoController(_jogoRepositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public void Get_DeveRetornarOk_QuandoJogoExiste()
        {
            // Arrange
            int id = 1;
            var jogo = new Jogo
            {
                Id = id,
                Nome = "Minecraft",
                Empresa = "Mojang",
                Descricao = "Jogo de blocos",
                Preco = 100,                
                DataCriacao = DateTime.Now
            };

            _jogoRepositoryMock.Setup(r => r.ObterPorId(id)).Returns(jogo);

            // Act
            var resultado = _controller.Get(id) as OkObjectResult;

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(200, resultado!.StatusCode);
            var jogoRetornado = Assert.IsType<Jogo>(resultado.Value);
            Assert.Equal(id, jogoRetornado.Id);
            Assert.Equal("Minecraft", jogoRetornado.Nome);
        }

        [Fact]
        public void Get_DeveRetornarBadRequest_QuandoOcorreExcecao()
        {
            // Arrange
            int id = 99;
            _jogoRepositoryMock.Setup(r => r.ObterPorId(id))
                               .Throws(new Exception("Erro inesperado"));

            // Act
            var resultado = _controller.Get(id) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(400, resultado!.StatusCode);
            var response = Assert.IsType<ApiResponse<string>>(resultado.Value);
            Assert.False(response.Sucesso);
            Assert.Contains("Erro ao tentar trazer um Jogo utilizando o Id", response.Erro!.Mensagem);
        }
    }
}
