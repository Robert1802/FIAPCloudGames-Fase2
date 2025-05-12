using Core.Input;
using System.ComponentModel.DataAnnotations;

namespace Core.Input
{
    public class UsuarioInput
    {
        public required string Nome { get; set; }
        [EmailAddress(ErrorMessage = "Este e-mail não é válido")]
        public required string Email { get; set; }
        public required string Senha { get; set; }
    }
}
