using FIAPCloudGames.Application.DTO.Request;
using FIAPCloudGames.Domain.Entity;
using FIAPCloudGames.Domain.Repository;
using FIAPCloudGames.Domain.Responses;
using FIAPCloudGames.WebApi.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;

namespace FIAPCloudGames.Test;

public class JogoUpdateTest
{
    private readonly Mock<IJogoRepository> _mockRepo;
    private readonly Mock<ILogger<JogoController>> _mockLogger;
    private readonly JogoController _controller;

    public JogoUpdateTest()
    {
        _mockRepo = new Mock<IJogoRepository>();
        _mockLogger = new Mock<ILogger<JogoController>>();
        _controller = new JogoController(_mockRepo.Object, _mockLogger.Object);
                
        var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "1"),
            new Claim(ClaimTypes.Role, "Admin"),
            new Claim(ClaimTypes.Name, "Administrador")
        }, "mock"));

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
    }

    [Fact]
    public void Put_ValidInput_ReturnsOkWithSuccessMessage()
    {
        // Arrange
        var jogoOriginal = new Jogo
        {
            Id = 1,
            Nome = "Antigo Jogo",
            Empresa = "Empresa X",
            Descricao = "Antiga descrição",
            Preco = 100,
            UsuarioId = 999
        };

        var input = new JogoUpdateRequest(Id: 1, Descricao: "Nova descrição", Empresa: "Nova Empresa", Nome: "Novo Nome", Preco: 150);

        _mockRepo.Setup(r => r.ObterPorId(1)).Returns(jogoOriginal);
        _mockRepo.Setup(r => r.Alterar(It.IsAny<Jogo>()));

        // Act
        var result = _controller.Put(input);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<ApiResponse<string>>(okResult.Value);

        Assert.Equal(200, okResult.StatusCode);
        Assert.Contains("atualizado com sucesso", response.Dados);

        _mockRepo.Verify(r => r.Alterar(It.Is<Jogo>(
            j => j.Id == 1 &&
                 j.Nome == "Novo Nome" &&
                 j.Empresa == "Nova Empresa" &&
                 j.Descricao == "Nova descrição" &&
                 j.Preco == 150 &&
                 j.UsuarioId == 1 // conferindo que pegou o usuário logado
        )), Times.Once);
    }

    [Fact]
    public void Put_ThrowsException_ReturnsBadRequest()
    {
        // Arrange
        var input = new JogoUpdateRequest(Id:1, Descricao: "Teste", Empresa: "X", Nome: "Erro", Preco: 10);

        _mockRepo.Setup(r => r.ObterPorId(It.IsAny<int>()))
                 .Throws(new Exception("Erro de banco"));

        // Act
        var result = _controller.Put(input);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        var response = Assert.IsType<ApiResponse<string>>(badRequest.Value);

        Assert.Equal(500, response.Erro!.StatusCode);
        Assert.Contains("Um erro ocorreu ao tentar atualizar o jogo", response.Erro.Mensagem);
    }
}
