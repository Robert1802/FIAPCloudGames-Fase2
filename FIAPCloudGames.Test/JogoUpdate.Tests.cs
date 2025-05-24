using Core.Entity;
using Core.Input;
using Core.Repository;
using Core.Responses;
using FIAPCloudGamesApi.Controllers;
using FIAPCloudGamesApi.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;

public class JogoUpdateTest
{
    [Fact]
    public void Put_ValidInput_ReturnsOkWithSuccessMessage()
    {
        // Arrange
        var mockRepo = new Mock<IJogoRepository>();
        var mockLogger = new Mock<ILogger<JogoController>>();

        var jogoOriginal = new Jogo
        {
            Id = 1,
            Nome = "Antigo Jogo",
            Empresa = "Empresa X",
            Descricao = "Antiga descrição",
            Preco = 100,
            UsuarioId = 999
        };

        var input = new JogoUpdateInput
        {
            Id = 1,
            Nome = "  Novo Nome  ",
            Empresa = "Nova Empresa",
            Descricao = "  Nova descrição  ",
            Preco = 150
        };

        mockRepo.Setup(r => r.ObterPorId(1)).Returns(jogoOriginal);
        mockRepo.Setup(r => r.Alterar(It.IsAny<Jogo>()));

        var controller = new JogoController(mockRepo.Object, mockLogger.Object);

        // Act
        var result = controller.Put(input);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var mensagem = Assert.IsType<string>(okResult.Value);

        mockRepo.Verify(r => r.Alterar(It.Is<Jogo>(
            j => j.Id == 1 &&
                    j.Nome == "Novo Nome" &&
                    j.Empresa == "Nova Empresa" &&
                    j.Descricao == "Nova descrição" &&
                    j.Preco == 150 &&
                    j.UsuarioId == 1
        )), Times.Once);
    }


    [Fact]
    public void Put_ThrowsException_ReturnsBadRequest()
    {
        // Arrange
        var mockRepo = new Mock<IJogoRepository>();
        var mockLogger = new Mock<ILogger<JogoController>>();
        var input = new JogoUpdateInput { Id = 1, Nome = "Erro", Empresa = "X", Descricao = "Teste", Preco = 10 };

        mockRepo.Setup(r => r.ObterPorId(It.IsAny<int>())).Throws(new Exception("Erro de banco"));

        var controller = new JogoController(mockRepo.Object, mockLogger.Object);

        // Act
        var result = controller.Put(input);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        var response = Assert.IsType<ApiResponse<string>>(badRequest.Value);

        Assert.Equal(500, response.Erro.StatusCode);
        Assert.Contains("Um erro ocorreu ao tentar atualizar o jogo", response.Erro.Mensagem);
    }

}
