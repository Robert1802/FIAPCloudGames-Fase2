using System.ComponentModel.DataAnnotations;

namespace FIAPCloudGames.Core.Input
{
    public class LoginInput
    {
        public required string Email { get; set; }
        public required string Senha { get; set; }
    }
}
