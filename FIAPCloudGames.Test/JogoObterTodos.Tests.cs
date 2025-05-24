using Core.Entity;
using Core.Repository;
using FIAPCloudGamesApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Core.Input;
using Core.Responses;

namespace FIAPCloudGames.Test
{
    public class JogoObterTodos
    {
        private readonly Mock<IJogoRepository> _jogoRepositoryMock;
        private readonly Mock<ILogger<JogoController>> _loggerMock;
        private readonly JogoController _controller;

        public JogoObterTodos()
        {
            _jogoRepositoryMock = new Mock<IJogoRepository>();
            _loggerMock = new Mock<ILogger<JogoController>>();
            _controller = new JogoController(_jogoRepositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public void Get_SemFiltro_DeveRetornarListaDeJogos()
        {
            // Arrange
            var jogos = new List<Jogo>
            {
                new() { Id = 1, Nome = "Minecraft", Empresa = "Mojang", Descricao = "Blocos", Preco = 100, Desconto = 0, DataCriacao = DateTime.Now },
                new() { Id = 2, Nome = "CS2", Empresa = "Valve", Descricao = "FPS", Preco = 200, Desconto = 0, DataCriacao = DateTime.Now }
            };

            _jogoRepositoryMock.Setup(r => r.ObterTodos()).Returns(jogos);

            // Act
            var resultado = _controller.Get(null) as OkObjectResult;

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(200, resultado.StatusCode);
            var data = Assert.IsType<List<JogoDto>>(resultado.Value);
            Assert.Equal(2, data.Count);
        }

        [Fact]
        public void Get_ComFiltro_DeveRetornarJogosFiltrados()
        {
            // Arrange
            string filtro = "Minecraft";
            var jogos = new List<Jogo>
            {
                new() { Id = 1, Nome = "Minecraft", Empresa = "Mojang", Descricao = "Blocos", Preco = 100, Desconto = 0, DataCriacao = DateTime.Now }
            };

            _jogoRepositoryMock.Setup(r => r.ObterTodosFiltro(filtro)).Returns(jogos);

            // Act
            var resultado = _controller.Get(filtro) as OkObjectResult;

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(200, resultado.StatusCode);
            var data = Assert.IsType<List<JogoDto>>(resultado.Value);
            Assert.Single(data);
            Assert.Equal("Minecraft", data[0].Nome);
        }

        [Fact]
        public void Get_QuandoOcorreExcecao_DeveRetornarBadRequest()
        {
            // Arrange
            _jogoRepositoryMock.Setup(r => r.ObterTodos()).Throws(new Exception("Erro inesperado"));

            // Act
            var resultado = _controller.Get(null) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(400, resultado.StatusCode);
            var response = Assert.IsType<ApiResponse<string>>(resultado.Value);
            Assert.False(response.Sucesso);
            Assert.Contains("Erro ao tentar trazer todos os Jogos", response.Erro!.Mensagem);
        }
    }
}
