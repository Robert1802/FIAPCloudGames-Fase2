using System.ComponentModel.DataAnnotations;

namespace FIAPCloudGames.Application.DTO.Request
{
    public record class UsuarioDepositoRequest
    {
        public int Id { get; init; }

        [Range(0.01, 999999999.99, ErrorMessage = "Deposito precisa ser maior do que zero")]
        public required decimal Deposito { get; init; }
    }
}
