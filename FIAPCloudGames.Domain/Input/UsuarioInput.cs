using FIAPCloudGames.Domain.Input;
using System.ComponentModel.DataAnnotations;

namespace FIAPCloudGames.Domain.Input
{
    public class UsuarioInput
    {
        public required string Nome { get; set; }
        [EmailAddress(ErrorMessage = "Este e-mail não é válido")]
        public required string Email { get; set; }
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9]).{8,}$",
        ErrorMessage = "A senha deve ter pelo menos 8 caracteres e conter pelo menos um número, uma letra maiúscula, uma letra minúscula e um caractere especial.")]
        public required string Senha { get; set; }
    }
}
