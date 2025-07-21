using FIAPCloudGames.Application.DTO;

namespace FIAPCloudGames.Application.DTO
{
    public class UsuarioJogoDto
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public int IdJogo { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}
