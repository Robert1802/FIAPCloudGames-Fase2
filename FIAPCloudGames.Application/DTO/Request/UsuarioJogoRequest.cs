using System.ComponentModel.DataAnnotations;

namespace FIAPCloudGames.Application.DTO.Request
{
    public record class UsuarioJogoRequest
    {
        [Required]
        public int UsuarioId { get; set; }

        [Required]
        public int JogoId { get; set; }

        public int PromocaoId { get; set; }
    }
}
