using System.ComponentModel.DataAnnotations;

namespace Core.Input
{
    public class UsuarioDeposito
    {
        public int Id { get; set; }

        [Range(0.01, 999999999.99, ErrorMessage = "Deposito precisa ser maior do que zero")]
        public required decimal Deposito { get; set; }
    }
}
