using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using FIAPCloudGamesApi.Controllers;
using Core.Repository;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Core.Input;
using Core.Entity;

public class JogoCreateTest
{
    private readonly Mock<IJogoRepository> _jogoRepositoryMock;
    private readonly Mock<ILogger<JogoController>> _loggerMock;
    private readonly JogoController _controller;

    public JogoCreateTest()
    {
        _jogoRepositoryMock = new Mock<IJogoRepository>();
        _loggerMock = new Mock<ILogger<JogoController>>();
        _controller = new JogoController(_jogoRepositoryMock.Object, _loggerMock.Object);                
    }

    [Fact]
    public void Post_DeveCadastrarJogo_QuandoDadosSaoValidos()
    {
        // Arrange
        var input = new JogoInput
        {
            Nome = "God of Code",
            Empresa = "Zitelli Games",
            Descricao = "Jogo de código",
            Preco = 199.90m
        };

        // Act
        var resultado = _controller.Post(input);

        // Assert
        var okResult = resultado as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);

        var jogo = okResult.Value as Jogo;
        jogo.Should().NotBeNull();
        jogo!.Nome.Should().Be("God of Code");
        jogo.Empresa.Should().Be("Zitelli Games");
        jogo.Preco.Should().Be(199.90m);
        jogo.Descricao.Should().Be("Jogo de código");
        jogo.UsuarioId.Should().Be(123);

        _jogoRepositoryMock.Verify(r => r.Cadastrar(It.IsAny<Jogo>()), Times.Once);
    }

    [Fact]
    public void Post_DeveRetornarBadRequest_QuandoJogoJaExiste()
    {
        // Arrange
        var input = new JogoInput
        {
            Nome = "God of Code",
            Empresa = "Zitelli Games",
            Descricao = "Jogo de código",
            Preco = 199.90m
        };

        _jogoRepositoryMock
            .Setup(r => r.Cadastrar(It.IsAny<Jogo>()))
            .Throws(new Exception("O jogo já existe em nossos servidores"));

        // Act
        var resultado = _controller.Post(input);

        // Assert
        var badRequest = resultado as BadRequestObjectResult;
        badRequest.Should().NotBeNull();
        badRequest!.StatusCode.Should().Be(400);

        badRequest.Value.Should().Be($"O jogo \"{input.Nome}\" já existe em nossos servidores.");

        _jogoRepositoryMock.Verify(r => r.Cadastrar(It.IsAny<Jogo>()), Times.Once);
    }    
}
