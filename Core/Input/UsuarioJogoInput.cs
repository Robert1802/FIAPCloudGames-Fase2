using System.ComponentModel.DataAnnotations;

namespace Core.Input
{
    public class UsuarioJogoInput
    {
        [Required]
        public int UsuarioId { get; set; }

        [Required]
        public int JogoId { get; set; }

        public int PromocaoId { get; set; }
    }
}
