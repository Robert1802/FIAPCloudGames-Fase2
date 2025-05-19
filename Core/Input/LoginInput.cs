using System.ComponentModel.DataAnnotations;

namespace Core.Input
{
    public class LoginInput
    {
        public required string Email { get; set; }
        public required string Senha { get; set; }
    }
}
